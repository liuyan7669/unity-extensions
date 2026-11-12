using UnityEngine.UI;

namespace Cowart.CwExtensions
{
    /// <summary>
    /// Selectable 扩展：关闭键盘和手柄导航。
    /// </summary>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "SelectableExtensions")]
    public static class SelectableExtensions
    {
        /// <summary>把 Selectable（Button、Toggle、InputField 等）的导航模式设为 None，方向键和手柄无法再移动焦点到它或从它移出。</summary>
        /// <param name="target">目标 Selectable；为 null 时不执行任何操作。</param>
        /// <remarks>
        /// 不影响鼠标和触摸点击。原导航模式不会保存，需要恢复时请自行设置 navigation。
        /// 常用于避免按下空格或回车时误触发当前选中的按钮。
        /// </remarks>
        public static void CwSetNavigationNone(this Selectable target)
        {
            if (target == null)
            {
                return;
            }

            Navigation navigation = target.navigation;
            navigation.mode = Navigation.Mode.None;
            target.navigation = navigation;
        }
    }
}
