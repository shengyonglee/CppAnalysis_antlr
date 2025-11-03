using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using CppParser.Enums;
using CppParser.Grammars.Generated;
using CppParser.Models;

namespace CppParser.Services
{
    /// 仅用 AST 填充模型
    public sealed class TypeBuilder
    {
        // ---------- 类内字段 ----------
        public IEnumerable<CodeProperty> BuildFieldsFromMemberDeclaration(
            CPP14Parser.MemberdeclarationContext md, EnumVisibility visibility)
        {
            var ds = md.declSpecifierSeq();
            var baseType = ds != null ? JoinTokens(ds) : string.Empty;

            var list = md.memberDeclaratorList();
            if (list == null) yield break;

            foreach (var m in list.memberDeclarator())
            {
                var d = m.declarator();
                if (d == null) continue;

                // 排除成员函数原型（保持原逻辑）
                if (HeaderModelBuilder.IsDeclaratorMethodPrototype(d)) continue;

                var p = new CodeProperty { Visibility = visibility };
                FillNamePtrRefArray(p, d);

                p.FullType = (baseType + " " + JoinTokens(d)).Trim();
                p.Type = baseType.Trim();
                MarkTypeFlagsFromDeclSpecs(p, baseType);

                var ini = m.braceOrEqualInitializer();
                if (ini != null) p.DefaultValue = JoinTokens(ini);

                yield return p;
            }
        }

        // ---------- 顶层字段（保留） ----------
        public IEnumerable<CodeProperty> BuildFieldsFromSimpleDeclaration(
            CPP14Parser.SimpleDeclarationContext ctx, EnumVisibility visibility)
        {
            var declSpecs = ctx.declSpecifierSeq();
            var baseType = declSpecs != null ? JoinTokens(declSpecs) : string.Empty;

            var list = ctx.initDeclaratorList();
            if (list == null) yield break;

            foreach (var id in list.initDeclarator())
            {
                var p = BuildFieldFromInitDeclarator(baseType, id);
                p.Visibility = visibility;
                yield return p;
            }
        }

        // ---------- 方法 ----------
        public CodeMethod BuildMethodFromFunctionDefinition(
            CPP14Parser.FunctionDefinitionContext ctx, EnumVisibility visibility)
        {
            var m = new CodeMethod { Visibility = visibility };

            var before = ctx.declSpecifierSeq() != null ? JoinTokens(ctx.declSpecifierSeq()) : string.Empty;
            FillMethodSignatureFromDeclarator(m, before, ctx.declarator());

            // override/final 在 functionDefinition 上（若语法提供）
            var vss = ctx.virtualSpecifierSeq();
            if (vss != null)
            {
                var vtxt = JoinTokens(vss);
                if (vtxt.Contains("override")) m.IsOverride = true;
                if (vtxt.Contains("final")) m.IsFinal = true;
            }
            return m;
        }

        public CodeMethod BuildMethodFromMemberDeclarationFunction(
            CPP14Parser.MemberdeclarationContext ctx,
            EnumVisibility visibility)
        {
            var m = new CodeMethod { Visibility = visibility };

            var before = ctx.declSpecifierSeq() != null ? JoinTokens(ctx.declSpecifierSeq()) : string.Empty;

            var declList = ctx.memberDeclaratorList();
            if (declList == null) return m;

            var md = declList.memberDeclarator()
                             .FirstOrDefault(x =>
                             {
                                 var dec = x.declarator();
                                 return dec != null && HeaderModelBuilder.IsDeclaratorMethodPrototype(dec);
                             });

            if (md == null || md.declarator() == null) return m;

            FillMethodSignatureFromDeclarator(m, before, md.declarator());

            var tailText = (JoinTokens(md.pureSpecifier()) + " " + JoinTokens(md.virtualSpecifierSeq())).Trim();
            if (!string.IsNullOrEmpty(tailText))
            {
                if (tailText.Contains("=0")) { m.IsPureVirtual = true; m.IsVirtual = true; }
                if (tailText.Contains("=default")) m.IsDefaultImplementation = true;
                if (tailText.Contains("=delete")) m.IsDeleted = true;
                if (tailText.Contains("override")) m.IsOverride = true;
                if (tailText.Contains("final")) m.IsFinal = true;
            }
            return m;
        }

        // ---------- 内部：字段 ----------
        private CodeProperty BuildFieldFromInitDeclarator(string baseType, CPP14Parser.InitDeclaratorContext init)
        {
            var p = new CodeProperty();
            var decl = init.declarator();
            if (decl != null) FillNamePtrRefArray(p, decl);

            p.FullType = (baseType + " " + JoinTokens(decl)).Trim();
            p.Type = baseType.Trim();
            MarkTypeFlagsFromDeclSpecs(p, baseType);

            if (init.initializer() != null) p.DefaultValue = JoinTokens(init.initializer());
            return p;
        }

        // ---------- 内部：方法 ----------
        private void FillMethodSignatureFromDeclarator(CodeMethod m, string before, CPP14Parser.DeclaratorContext declarator)
        {
            MarkMethodPrefixFlags(m, before);

            var stripped = StripKeywords(before, new[]
            { "inline","static","explicit","friend","constexpr","virtual","extern","mutable","register" }).Trim();

            if (!string.IsNullOrEmpty(stripped))
            {
                m.ReturnType = stripped;
                if (stripped.Contains("*")) m.ReturnTypeIsPointer = true;
                if (stripped.Contains("&")) m.ReturnTypeIsReference = true;
                if (stripped.Contains("const")) m.IsReturnConst = true;
            }

            // 名称（常规路径失败则兜底找 IdExpression）
            var id = FindIdInDeclarator(declarator);
            if (string.IsNullOrEmpty(id))
            {
                var idExpr = FindNode<CPP14Parser.IdExpressionContext>(declarator);
                if (idExpr != null) id = JoinTokens(idExpr);
            }
            m.Name = string.IsNullOrEmpty(id) ? "(anonymous)" : id;

            // 参数/尾部 const
            var pq = FindNode<CPP14Parser.ParametersAndQualifiersContext>(declarator);
            if (pq != null)
            {
                var clause = pq.parameterDeclarationClause();
                var list = clause?.parameterDeclarationList();
                if (list != null)
                    foreach (var pd in list.parameterDeclaration())
                        m.Parameters.Add(BuildParameter(pd));

                if (pq.cvqualifierseq() != null && JoinTokens(pq.cvqualifierseq()).Contains("const"))
                    m.IsConst = true;
            }
            else if (ContainsTerminal(declarator, "("))
            {
                // 空参
            }
        }

        private CodeMethodParameter BuildParameter(CPP14Parser.ParameterDeclarationContext pd)
        {
            var par = new CodeMethodParameter();

            var ds = pd.declSpecifierSeq();
            var dsText = ds != null ? JoinTokens(ds) : string.Empty;

            var pdecl = pd.declarator();
            if (pdecl != null)
            {
                FillNamePtrRefArray(par, pdecl);
                if (JoinTokens(pdecl).Contains("&&")) par.IsRValueReference = true;
            }
            else { par.Name = string.Empty; }

            par.FullType = (dsText + " " + JoinTokens(pdecl)).Trim();
            par.Type = dsText.Trim();
            MarkTypeFlagsFromDeclSpecs(par, dsText);

            return par;
        }

        // ---------- 共享 ----------
        private static string FindIdInDeclarator(CPP14Parser.DeclaratorContext declarator)
        {
            var id = declarator?.noPointerDeclarator()?.declaratorid()?.idExpression();
            if (id != null) return JoinTokens(id);

            var inner = declarator?.pointerDeclarator()?.noPointerDeclarator()?.declaratorid()?.idExpression();
            if (inner != null) return JoinTokens(inner);

            return string.Empty;
        }

        private void FillNamePtrRefArray(CodeProperty target, CPP14Parser.DeclaratorContext decl)
        {
            var id = FindIdInDeclarator(decl);
            if (string.IsNullOrEmpty(id))
            {
                var idExpr = FindNode<CPP14Parser.IdExpressionContext>(decl);
                if (idExpr != null) id = JoinTokens(idExpr);
            }
            target.Name = string.IsNullOrEmpty(id) ? "(anonymous)" : id;

            var text = JoinTokens(decl);
            if (text.Contains("*")) target.IsPointer = true;
            if (text.Contains("&")) target.IsReference = true;

            var noptr = decl.noPointerDeclarator() ?? decl.pointerDeclarator()?.noPointerDeclarator();
            var (hasArr, size) = FindArraySuffix(noptr);
            if (hasArr) { target.IsArray = true; target.ArraySize = size; }
        }

        private static (bool hasArray, string? size) FindArraySuffix(CPP14Parser.NoPointerDeclaratorContext? noptr)
        {
            var cur = noptr;
            while (cur != null)
            {
                try
                {
                    var lb = cur.LeftBracket(); // ITerminalNode?（有些语法没有该方法）
                    if (lb != null)
                    {
                        var ce = cur.constantExpression();
                        var sizeText = ce != null ? JoinTokens(ce) : null;
                        return (true, sizeText);
                    }
                }
                catch
                {
                    if (ContainsTerminal(cur, "[")) // 兜底：查找 '[' 终结符
                    {
                        var ce = cur.constantExpression();
                        var sizeText = ce != null ? JoinTokens(ce) : null;
                        return (true, sizeText);
                    }
                }
                cur = cur.noPointerDeclarator();
            }
            return (false, null);
        }

        private static void MarkTypeFlagsFromDeclSpecs(CodeProperty p, string t)
        {
            if (t.Contains("const")) p.IsConst = true;
            if (t.Contains("volatile")) p.IsVolatile = true;
            if (t.Contains("mutable")) p.IsMutable = true;
            if (t.Contains("static")) p.IsStatic = true;
            if (t.Contains("signed")) p.IsSigned = true;
            if (t.Contains("unsigned")) p.IsUnsigned = true;
            if (t.Contains("short")) p.IsShort = true;
            if (t.Contains("long")) p.IsLong = true;
        }

        private static void MarkMethodPrefixFlags(CodeMethod m, string before)
        {
            if (before.Contains("inline")) m.IsInline = true;
            if (before.Contains("static")) m.IsStatic = true;
            if (before.Contains("explicit")) m.IsExplicit = true;
            if (before.Contains("friend")) m.IsFriend = true;
            if (before.Contains("constexpr")) m.IsConstexpr = true;
            if (before.Contains("virtual")) m.IsVirtual = true;
        }

        private static string StripKeywords(string text, IEnumerable<string> keywords)
        {
            var r = " " + (text ?? string.Empty) + " ";
            foreach (var k in keywords) r = r.Replace(" " + k + " ", " ");
            return r.Trim();
        }

        // ---- 通用树工具/Token 拼接 ----
        public static TNode? FindNode<TNode>(IParseTree root) where TNode : class, IParseTree
        {
            if (root is TNode tn) return tn;
            for (int i = 0; i < root.ChildCount; i++)
            {
                var r = FindNode<TNode>(root.GetChild(i));
                if (r != null) return r;
            }
            return null;
        }

        public static bool ContainsTerminal(IParseTree root, string tokenText)
        {
            if (root.ChildCount == 0) return root.GetText() == tokenText;
            for (int i = 0; i < root.ChildCount; i++)
                if (ContainsTerminal(root.GetChild(i), tokenText)) return true;
            return false;
        }

        public static string JoinTokens(IParseTree? node)
        {
            if (node == null) return string.Empty;
            var parts = new List<string>();
            Collect(node, parts);
            return string.Join(" ", parts.Where(s => !string.IsNullOrWhiteSpace(s))).Trim();

            static void Collect(IParseTree n, List<string> acc)
            {
                if (n.ChildCount == 0)
                {
                    var t = n.GetText();
                    if (!string.IsNullOrEmpty(t)) acc.Add(t);
                    return;
                }
                for (int i = 0; i < n.ChildCount; i++)
                    Collect(n.GetChild(i), acc);
            }
        }
    }
}
