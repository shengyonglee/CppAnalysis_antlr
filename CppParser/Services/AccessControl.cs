using CppParser.Enums;
using CppParser.Models;

namespace CppParser.Services
{
    /// 管理类体内的访问控制块（public:/protected:/private:）
    public sealed class AccessControl
    {
        public EnumVisibility Current { get; private set; } = EnumVisibility.Private;

        /// 根据类键值设定初始可见性：class→private；struct/union→public
        public void EnterClass(EnumClassType stereotype)
        {
            Current = (stereotype == EnumClassType.Class)
                ? EnumVisibility.Private
                : EnumVisibility.Public; // struct/union/interface 默认 public 行为
        }

        public void LeaveClass() => Current = EnumVisibility.Private;

        /// 从 token 文本切换可见性（保持原有调用点：acc.GetText()）
        public void Set(string accessSpecifier)
        {
            switch (accessSpecifier)
            {
                case "public": Current = EnumVisibility.Public; break;
                case "protected": Current = EnumVisibility.Protected; break;
                case "private": Current = EnumVisibility.Private; break;
            }
        }
    }
}
