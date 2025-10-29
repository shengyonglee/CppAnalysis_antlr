using System;
using System.Collections.Generic;
using System.IO;
using Scriban;
using CppParser.Models;
using Scriban.Runtime; // 用你提供的模型类
using CppParser.Enums;
namespace CppGenerator
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== UML to C++ Code Generator ===");

            // 构造一个“确定不为空”的示例模型
            var cppClass = new CppClass
            {
                Name = "Person",
                Stereotype = EnumClassType.Class,
                Properties = new List<CppProperty>
                {
                    new CppProperty { Name = "name", Type = "std::string", Visibility = EnumVisibility.Public },
                    new CppProperty { Name = "age",  Type = "int",          Visibility = EnumVisibility.Private }
                },
                Methods = new List<CppMethod>
                {
                    new CppMethod
                    {
                        Name = "getName",
                        ReturnType = "std::string",
                        Visibility =  EnumVisibility.Public,
                        Parameters = new List<CppMethodParameter>()
                    },
                    new CppMethod
                    {
                        Name = "setName",
                        ReturnType = "void",
                        Visibility = EnumVisibility.Public,
                        Parameters = new List<CppMethodParameter>
                        {
                            new CppMethodParameter { Name = "value", Type = "const std::string&" }
                        }
                    }
                },

            };
            cppClass.Associations.Add(new CppAssociation
            {
                TargetClass = "Company1",
                RoleName = "employer1",
                Multiplicity = EnumCppMultiplicity.ToOne
            });

            cppClass.Associations.Add(new CppAssociation
            {
                TargetClass = "Company2",
                RoleName = "employer2",
                Multiplicity = EnumCppMultiplicity.ToOne
            });

            cppClass.Dependencies.Add(new CppDependency
            {
                TargetClass = "Dependency1",
                RoleName = "dependency1",
                Multiplicity = EnumCppMultiplicity.ToOne
            });

            cppClass.Compositions.Add(new CppComposition
            {
                TargetClass = "Address",
                RoleName = "address",
                Multiplicity = EnumCppMultiplicity.ToOne
            });

            // 加载模板
            string headerTemplateText = File.ReadAllText(@"D:\work\learn\tools\vs\CppAnalysis_antlr\CppGenerator\Templates\class_header.sbn");
            string sourceTemplateText = File.ReadAllText(@"D:\work\learn\tools\vs\CppAnalysis_antlr\CppGenerator\Templates\class_source.sbn");
            var headerTemplate = Template.Parse(headerTemplateText);
            var sourceTemplate = Template.Parse(sourceTemplateText);

            var scribanContext = new TemplateContext
            {
                MemberRenamer = member => member.Name
            };
            var globals = new ScriptObject();
            globals.SetValue("c", cppClass, true);
            globals.SetValue("class", cppClass, true);

            // 预计算，模板不再调用 string.upper/downcase
            globals.SetValue("NAME_UPPER", cppClass.Name.ToUpperInvariant(), true);
            globals.SetValue("NAME_LOWER", cppClass.Name.ToLowerInvariant(), true);
            globals.SetValue("HEADER_GUARD", $"_{cppClass.Name.ToUpperInvariant()}_H_", true);

            scribanContext.PushGlobal(globals);



            // 渲染（注意这里传的是 scribanContext，而不是匿名对象/字典）
            string headerCode = headerTemplate.Render(scribanContext);
            string sourceCode = sourceTemplate.Render(scribanContext);

            // 输出
            string outputDir = "D:\\work\\learn\\tools\\vs\\CppAnalysis_antlr\\CppGenerator\\Output\\";
            Directory.CreateDirectory(outputDir);
            File.WriteAllText(Path.Combine(outputDir, $"{cppClass.Name}.h"), headerCode);
            File.WriteAllText(Path.Combine(outputDir, $"{cppClass.Name}.cpp"), sourceCode);

            Console.WriteLine("Generated:");
            Console.WriteLine($" - {outputDir}/{cppClass.Name}.h");
            Console.WriteLine($" - {outputDir}/{cppClass.Name}.cpp");
        }
    }
}
