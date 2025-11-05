using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Enums
{
    public enum EnumCppMultiplicity
    {
        /// <summary>
        /// 无多重性
        /// </summary>
        None = 0,

        /// <summary>
        /// 单一（如 1、 0.1）
        /// </summary>
        ToOne,

        /// <summary>
        /// 固定数量（如 0、 3、 0..3）
        /// </summary>
        ToFixed,

        /// <summary>
        /// 多重（0..* 或 1..*）
        /// </summary>
        ToMany,

    }


}
