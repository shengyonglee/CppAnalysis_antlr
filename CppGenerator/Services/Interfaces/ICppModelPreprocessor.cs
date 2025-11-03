using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppParser.Models;

using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>对模型做净化/兜底/排序/修复（不改变业务语义）。</summary>
    public interface ICppModelPreprocessor
    {
        CodeClass ProcessClass(CodeClass model);
        CodeEnum ProcessEnum(CodeEnum model);
    }
}
