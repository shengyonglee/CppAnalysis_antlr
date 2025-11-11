using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CppParser.Grammars.Generated;
using CppParser.Models;
using CppParser.Services.Interfaces;

namespace CppParser.Services.Implementation
{
    public class CppHeaderParser : ICppHeaderParser
    {
        public CodeHeaderFile ParseHeaderFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Header file not found: {filePath}");

            var content = File.ReadAllText(filePath);
            return ParseHeaderContent(content, Path.GetFileName(filePath));
        }

        public CodeHeaderFile ParseHeaderContent(string content, string fileName = "unknown.h")
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Content cannot be null or empty", nameof(content));

            var inputStream = new AntlrInputStream(content);
            var lexer = new CPP14Lexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new CPP14Parser(tokenStream);

            // 设置错误处理策略
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ThrowExceptionErrorListener());

            var tree = parser.translationUnit();

            var visitor = new CppHeaderVisitor(fileName);
            // 执行ANTLR4访问者模式遍历语法树，并将结果转换为CodeHeaderFile对象
            // Visit会递归遍历整个语法树，从根节点开始访问所有子节点
            // 在遍历过程中，会调用相应的 VisitXXX方法处理每种语法结构
            return visitor.Visit(tree) as CodeHeaderFile;
        }
    }

    public class ThrowExceptionErrorListener : BaseErrorListener
    {
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new ArgumentException($"Syntax error at line {line}:{charPositionInLine} - {msg}");
        }
    }
}