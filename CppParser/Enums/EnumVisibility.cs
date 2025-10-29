using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Enums
{
    public enum EnumVisibility
    {
        /// <summary>
        /// 无可见性
        /// </summary>
        None = 0,

        /// <summary>
        /// 公共的
        /// </summary>
        Public,

        /// <summary>
        /// 受保护的
        /// </summary>
        Protected,

        /// <summary>
        /// 私有的
        /// </summary>
        Private
    }
}
