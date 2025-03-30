using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scriban;

namespace CppGenerator.Services
{
    /// <summary>提供头/源 Scriban 模板。</summary>
    public interface ITemplateProvider
    {
        Template GetHeaderTemplate();
        Template GetSourceTemplate();
    }
}