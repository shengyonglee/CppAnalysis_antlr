using System;
using System.Collections.Generic;
using CppParser.Enums;
using CppParser.Models;
using CppGenerator.Services;

namespace CppGenerator
{
    internal class Program
    {
        static void Main()
        {
            // 1) 依赖组装（后端建议用 DI 注册）
            var pre = new DefaultModelPreprocessor();
            var tpl = new FileTemplateProvider(
                headerTemplatePath: @"D:\work\learn\tools\vs\CppAnalysis_antlr\CppGenerator\Templates\class_header.sbn",
                sourceTemplatePath: @"D:\work\learn\tools\vs\CppAnalysis_antlr\CppGenerator\Templates\class_source.sbn");
            var renderer = new ScribanRenderer(tpl);
            var generator = new DefaultCodeGenerator(pre, renderer);

            // 2) 模拟后端传入的 CppClass
            var cppClass = new CppClass
            {
                Name = "Person",
                Stereotype = EnumClassType.Class,
                Properties = new List<CppProperty>
                {
                    new CppProperty { Name = "name", Type = "std::string", Visibility = EnumVisibility.Public, DefaultValue = "\"Tom\"" },
                    new CppProperty { Name = "age",  Type = "int",          Visibility = EnumVisibility.Private }
                },
                Methods = new List<CppMethod>
                {
                    new CppMethod { Name = "getName", ReturnType = "std::string", Visibility = EnumVisibility.Public },
                    new CppMethod {
                        Name = "setName", ReturnType = "void", Visibility = EnumVisibility.Public,
                        Parameters = new List<CppMethodParameter> { new() { Name = "v", Type = "const std::string&" } }
                    }
                }
            };

            // 3) 生成（仅字符串）
            var result = generator.Generate(cppClass);

            // 4) 输出（真实后端直接返回字符串即可）

            string outputDir = "D:\\work\\learn\\tools\\vs\\CppAnalysis_antlr\\CppGenerator\\Output\\";
            Directory.CreateDirectory(outputDir);
            File.WriteAllText(Path.Combine(outputDir, $"{cppClass.Name}.h"), result.HeaderCode);
            File.WriteAllText(Path.Combine(outputDir, $"{cppClass.Name}.cpp"), result.SourceCode);

            Console.WriteLine("Generated:");
            Console.WriteLine($" - {outputDir}/{cppClass.Name}.h");
            Console.WriteLine($" - {outputDir}/{cppClass.Name}.cpp");
        }
    }
}
