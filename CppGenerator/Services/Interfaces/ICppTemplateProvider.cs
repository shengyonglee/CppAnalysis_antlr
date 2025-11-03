using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scriban;

namespace CppGenerator.Services
{
    /// <summary>提供 Scriban 模板。</summary>
    public interface ICppTemplateProvider
    {
        Template GetClassHeaderTemplate();
        Template GetClassSourceTemplate();
        Template GetEnumHeaderTemplate();
        Template GetInterfaceHeaderTemplate();
        Template GetStructHeaderTemplate();
    }
}