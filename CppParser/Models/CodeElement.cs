using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CppParser.Enums;
namespace CppParser.Models
{
    public abstract class CodeElement
    {
        /// <summary>
        /// 元素名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 元素中文名称/label
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 元素可见性，默认为"public"
        /// </summary>
        public EnumVisibility Visibility { get; set; } = EnumVisibility.Public;

        /// <summary>
        /// 注释信息
        /// </summary>
        public string Comment { get; set; }
    }
}
