using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using CppParser.Grammars.Generated;
using CppParser.Models;

namespace CppParser.Services
{
    /// 仅用 AST 构建模型
    public sealed class HeaderModelBuilder : CPP14ParserBaseVisitor<object?>
    {
        private readonly string _fileName;
        private readonly Stack<string> _ns = new();
        private readonly Stack<CppClass> _class = new();
        private readonly AccessControl _access = new();
        private readonly TypeBuilder _types = new();

        private readonly CppHeaderFile _header;

        public HeaderModelBuilder(string fileName)
        {
            _fileName = fileName ?? string.Empty;
            _header = new CppHeaderFile { FileName = _fileName };
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
            var cls = new CppClass
            {
                Stereotype = head.classKey().GetText(),
                Name = BuildQname(ResolveClassName(head))
            };

            // 继承
            var bc = head.baseClause()?.baseSpecifierList();
            if (bc != null)
            {
                foreach (var b in bc.baseSpecifier())
                {
                    var bt = b.baseTypeSpecifier();
                    if (bt != null)
                    {
                        var name = TypeBuilder.JoinTokens(bt);
                        if (!string.IsNullOrWhiteSpace(name)) cls.BaseClasses.Add(name);
                    }
                }
            }

            _header.Classes.Add(cls);

            _access.EnterClass(cls.Stereotype);
            _class.Push(cls);

            var ms = ctx.memberSpecification();
            if (ms != null) VisitMemberSpecification(ms);

            _class.Pop();
            _access.LeaveClass();
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
            var e = new CppEnum
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
                    if (!string.IsNullOrEmpty(id)) e.Values.Add(id!);
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

        // —— 关键：成员函数原型判定（外层无 pointerDeclarator 且 declarator 子树出现 '('）——
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
            if (idx < 0) return false;          // 没有参数括号，不是函数

            var head = txt.Substring(0, idx);   // 第一个 '(' 之前
            head = head.Trim();

            // 典型函数指针/成员函数指针的特征
            if (head.Contains("*")) return false;     // 包含 '*'（如 (*fp) / Class::(*pmf)）
            if (head.Contains("::)")) return false;   // 极端情形
            if (head.EndsWith(")")) return false;     // 形如 "( *name" 未被空格折叠的情况

            // 注意：这里不看 AST 的 pointerDeclarator/parametersAndQualifiers，
            // 仅用简单的 pre-'(' 片段判断，适配你当前生成器结构。
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
