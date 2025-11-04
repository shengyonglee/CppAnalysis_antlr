using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using CppParser.Enums;
using CppParser.Grammars.Generated;
using CppParser.Models;

namespace CppParser.Services
{
    /// 仅用 AST 构建模型
    public sealed class HeaderModelBuilder : CPP14ParserBaseVisitor<object?>
    {
        private readonly string _fileName;
        private readonly Stack<string> _ns = new();
        private readonly Stack<CodeClass> _class = new();
        private readonly AccessControl _access = new();
        private readonly TypeBuilder _types = new();

        private readonly CodeHeaderFile _header;

        public HeaderModelBuilder(string fileName)
        {
            _fileName = fileName ?? string.Empty;
            _header = new CodeHeaderFile { FileName = _fileName };
        }

        public override object? Visit([NotNull] IParseTree tree)
        {
            base.Visit(tree);
            return _header;
        }

        public override object? VisitNamespaceDefinition([NotNull] CPP14Parser.NamespaceDefinitionContext ctx)
        {
            var name = ctx.Identifier()?.GetText() ?? ctx.originalNamespaceName()?.GetText() ?? "(anonymous)";
            _ns.Push(name);
            var r = base.VisitNamespaceDefinition(ctx);
            _ns.Pop();
            return r;
        }

        public override object? VisitClassSpecifier([NotNull] CPP14Parser.ClassSpecifierContext ctx)
        {
            var head = ctx.classHead();

            // 1) 映射 classKey 文本到枚举
            var key = head.classKey().GetText(); // "class" | "struct" | "union"
            var stereotype = key switch
            {
                "class" => EnumClassType.Class,
                "struct" => EnumClassType.Struct,
                _ => EnumClassType.Class
            };

            var cls = new CodeClass
            {
                Stereotype = stereotype,
                Name = BuildQname(ResolveClassName(head))
            };

            // 2) 继承  
            var bc = head.baseClause()?.baseSpecifierList();
            if (bc != null)
            {
                foreach (var b in bc.baseSpecifier())
                {
                    var bt = b.baseTypeSpecifier();
                    if (bt == null) continue;

                    // 原先：var name = TypeBuilder.JoinTokens(bt); if (!string.IsNullOrWhiteSpace(name)) cls.BaseClasses.Add(name);

                    var name = TypeBuilder.JoinTokens(bt).Trim();
                    if (string.IsNullOrWhiteSpace(name)) continue;

                    // 新：落到 Generalizations，保持原有“只记录名称”的逻辑，不引入新的语义字段
                    var gen = new CodeGeneralization
                    {
                        // 模型里最关键的是目标类型；其余字段（Multiplicity/RoleName等）此处不设置，保持原逻辑不变
                        TargetName = name
                    };
                    cls.Generalizations.Add(gen);
                }
            }


            _header.Classes.Add(cls);

            // 3) 进入类体：根据类键值设定默认可见性
            _access.EnterClass(cls.Stereotype);
            _class.Push(cls);

            var ms = ctx.memberSpecification();
            if (ms != null) VisitMemberSpecification(ms);

            _class.Pop();
            _access.LeaveClass();

            // 类成员收集完成后，可选择在外部调用 AnalyzeAsInterface()，这里保持原逻辑不动
            return cls;
        }

        public override object? VisitMemberSpecification([NotNull] CPP14Parser.MemberSpecificationContext ctx)
        {
            foreach (var ch in ctx.children ?? Enumerable.Empty<IParseTree>())
            {
                if (ch is CPP14Parser.AccessSpecifierContext acc)
                {
                    _access.Set(acc.GetText());
                }
                else if (ch is CPP14Parser.FunctionDefinitionContext fd)
                {
                    var m = _types.BuildMethodFromFunctionDefinition(fd, _access.Current);
                    _class.Peek().Methods.Add(m);
                }
                else if (ch is CPP14Parser.MemberdeclarationContext md)
                {
                    HandleMemberDeclaration(md);
                }
            }
            return null;
        }

        public override object? VisitEnumSpecifier([NotNull] CPP14Parser.EnumSpecifierContext ctx)
        {
            var head = ctx.enumHead();
            var e = new CodeEnum
            {
                IsScoped = TypeBuilder.JoinTokens(head.enumkey()).Contains("class") ||
                           TypeBuilder.JoinTokens(head.enumkey()).Contains("struct"),
                Name = BuildQname((head.Identifier()?.GetText() is { } n && n.Length > 0) ? n : "(anonymous enum)")
            };

            var enumbase = head.enumbase()?.typeSpecifierSeq();
            if (enumbase != null) e.UnderlyingType = TypeBuilder.JoinTokens(enumbase);

            var list = ctx.enumeratorList();
            if (list != null)
            {
                foreach (var def in list.enumeratorDefinition())
                {
                    var id = def.enumerator().Identifier()?.GetText();
                    //if (!string.IsNullOrEmpty(id)) e.Values.Add(id!);
                }
            }

            if (_class.Count == 0) _header.Enums.Add(e); else _class.Peek().Enums.Add(e);
            return e;
        }

        private void HandleMemberDeclaration(CPP14Parser.MemberdeclarationContext md)
        {
            if (IsMethodPrototype(md))
            {
                var m = _types.BuildMethodFromMemberDeclarationFunction(md, _access.Current);
                _class.Peek().Methods.Add(m);
                return;
            }

            if (md.declSpecifierSeq() != null && md.memberDeclaratorList() != null)
            {
                foreach (var p in _types.BuildFieldsFromMemberDeclaration(md, _access.Current))
                    _class.Peek().Properties.Add(p);
            }
        }

        // —— 成员函数原型判定（保持原有启发式/不改逻辑）——
        private static bool IsMethodPrototype(CPP14Parser.MemberdeclarationContext md)
        {
            var list = md.memberDeclaratorList();
            if (list == null) return false;

            foreach (var it in list.memberDeclarator())
            {
                var d = it.declarator();
                if (d == null) continue;
                if (IsDeclaratorMethodPrototype(d)) return true;
            }
            return false;
        }

        internal static bool IsDeclaratorMethodPrototype(CPP14Parser.DeclaratorContext d)
        {
            var txt = TypeBuilder.JoinTokens(d);
            var idx = txt.IndexOf('(');
            if (idx < 0) return false;

            var head = txt.Substring(0, idx).Trim();
            if (head.Contains("*")) return false;
            if (head.Contains("::)")) return false;
            if (head.EndsWith(")")) return false;
            return true;
        }

        private static string ResolveClassName(CPP14Parser.ClassHeadContext head)
        {
            var chn = head.classHeadName();
            return chn == null ? "(anonymous)" : chn.GetText();
        }

        private string BuildQname(string simple)
            => _ns.Count == 0 ? simple : string.Join("::", _ns.Reverse()) + "::" + simple;
    }
}
