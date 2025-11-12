using CppParser.Models;
using CppParser.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Services.Implementation
{
    /// <summary>
    /// 头文件预处理器
    /// </summary>
    public class CppHeaderPreprocessor : ICppHeaderPreprocessor
    {
        /// <summary>
        /// C++类型到UML基础数据类型的映射字典
        /// </summary>
        private static readonly Dictionary<string, string> _typeMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["bool"] = "Boolean",
            ["double"] = "Real",
            ["std::string"] = "String",
            ["string"] = "String",
            ["double"] = "Double",
            ["float"] = "Float",
            ["char"] = "Char",
            ["long"] = "Long",
            ["short"] = "Short",
            ["std::byte"] = "Byte",
            ["byte"] = "Byte",
            ["int"] = "Integer"
        };

        /// <summary>
        /// 预处理头文件模型
        /// </summary>
        /// <param name="headerFile"></param>
        /// <returns>返回预处理之后的CodeHeaderFile</returns>
        public CodeHeaderFile Preprocess(CodeHeaderFile headerFile)
        {
            if (headerFile == null) return null;

            try
            {
                // 预处理类
                if (headerFile.Classes != null)
                {
                    foreach (var codeClass in headerFile.Classes)
                    {
                        PreprocessClass(codeClass);
                    }
                }

                return headerFile;
            }
            catch (Exception ex)
            {
                // 记录错误但不中断处理
                System.Diagnostics.Debug.WriteLine($"预处理头文件时出错: {ex.Message}");
                return headerFile;
            }
        }

        /// <summary>
        /// 预处理类模型
        /// </summary>
        /// <param name="codeClass"></param>
        private void PreprocessClass(CodeClass codeClass)
        {
            if (codeClass == null) return;

            // 预处理属性
            if (codeClass.Properties != null)
            {
                foreach (var property in codeClass.Properties)
                {
                    PreprocessProperty(property);
                }
            }

            // 预处理方法
            if (codeClass.Methods != null)
            {
                foreach (var method in codeClass.Methods)
                {
                    PreprocessMethod(method);
                }
            }
        }

        /// <summary>
        /// 预处理属性模型
        /// </summary>
        /// <param name="property"></param>
        private void PreprocessProperty(CodeProperty property)
        {
            if (property == null) return;
            (property.Type, property.CustomType) = GetMappedTypeInfo(property.Type);
        }

        /// <summary>
        /// 获取映射后的类型信息
        /// </summary>
        /// <param name="originalType"></param>
        /// <returns></returns>
        private (string type, string customType) GetMappedTypeInfo(string originalType)
        {
            string mappedType = MapTypeToUML(originalType);
            return string.IsNullOrEmpty(mappedType)
                ? (string.Empty, originalType)    // 自定义类型
                : (mappedType, string.Empty);     // 基础类型
        }
        /// <summary>
        /// 预处理方法模型
        /// </summary>
        /// <param name="method"></param>

        private void PreprocessMethod(CodeMethod method)
        {
            if (method == null) return;
            (method.ReturnType, method.CustomReturnType) = GetMappedTypeInfo(method.ReturnType);

            // 预处理参数
            if (method.Parameters != null)
            {
                foreach (var parameter in method.Parameters)
                {
                    PreprocessParameter(parameter);
                }
            }
        }

        /// <summary>
        /// 预处理方法参数模型
        /// </summary>
        /// <param name="parameter"></param>
        private void PreprocessParameter(CodeMethodParameter parameter)
        {
            if (parameter == null) return;
            (parameter.Type, parameter.CustomType) = GetMappedTypeInfo(parameter.Type);
        }

        /// <summary>
        /// 将C++类型映射为UML基础数据类型
        /// </summary>
        /// <param name="originalType">原始类型名称</param>
        /// <returns>映射后的C++类型，如果不是已知的基础类型返回 string.Empty</returns>
        private static string MapTypeToUML(string originalType)
        {
            if (string.IsNullOrWhiteSpace(originalType))
                return originalType;

            // 检查是否是已知的基础数据类型
            if (_typeMapping.ContainsKey(originalType))
            {
                return _typeMapping[originalType];
            }

            // 如果不是已知的基础类型，返回空字符串表示自定义类型
            return string.Empty;
        }
    }
}
