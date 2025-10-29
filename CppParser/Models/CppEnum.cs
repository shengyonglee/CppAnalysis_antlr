using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Models
{
    public class CppEnum : CppElement
    {
        /// <summary>
        /// 枚举值列表
        /// </summary>
        public List<string> Values { get; set; } = new List<string>();

        /// <summary>
        /// 是否为 scoped enum（enum class）
        /// </summary>
        public bool IsScoped { get; set; }

        /// <summary>
        /// 枚举的底层类型（如有指定int）
        /// </summary>
        public string UnderlyingType { get; set; } 
    }
}
