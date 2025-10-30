using System;
using System.Collections.Generic;
using System.IO;
using Scriban;
using CppParser.Models;
using Scriban.Runtime; 
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
                    new CppProperty { Name = "namea", Type = "std::string", Visibility = EnumVisibility.Public ,DefaultValue = "\"aaa\""},
                    new CppProperty { Name = "agea",  Type = "int",          Visibility = EnumVisibility.Private ,DefaultValue = "10"},
                    new CppProperty { Name = "a",  Type = "int", IsStatic = true,  Visibility = EnumVisibility.Public,DefaultValue = "10"},
                    new CppProperty { Name = "b",  Type = "int",  Visibility = EnumVisibility.Protected ,DefaultValue = "10" }

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
                        Visibility = EnumVisibility.Protected,
                        Parameters = new List<CppMethodParameter>
                        {
                            new CppMethodParameter { Name = "value", Type = "const std::string&" }
                        }
                    },
                    new CppMethod
                    {
                        Name = "setName1",
                        ReturnType = "string",
                        Visibility = EnumVisibility.Private,
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
                Multiplicity = EnumCppMultiplicity.ToOne,
                Visibility = EnumVisibility.Public
            });


            cppClass.Associations.Add(new CppAssociation
            {
                TargetClass = "Company2",
                RoleName = "employer2",
                Multiplicity = EnumCppMultiplicity.ToMany,
                Visibility = EnumVisibility.Protected
            });

            cppClass.Associations.Add(new CppAssociation
            {
                TargetClass = "Company3",
                RoleName = "employer3",
                Multiplicity = EnumCppMultiplicity.ToOne,
                Visibility = EnumVisibility.Protected
            });

            cppClass.Associations.Add(new CppAssociation
            {
                TargetClass = "Company4",
                RoleName = "employer4",
                Multiplicity = EnumCppMultiplicity.ToOne,
                Visibility = EnumVisibility.Private
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
