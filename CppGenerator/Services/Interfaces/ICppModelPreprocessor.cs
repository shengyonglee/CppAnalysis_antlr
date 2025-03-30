using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>对后端传来的 CppClass 做净化/兜底/排序/修复（不改变业务语义）。</summary>
    public interface ICppModelPreprocessor
    {
        CppClass Process(CppClass model);
    }
}