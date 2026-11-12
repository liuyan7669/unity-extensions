using UnityEngine;
using UnityEngine.UI;

namespace Cowart.CwExtensions
{
    /// <summary>
    /// Graphic 扩展：修改 UI 图形颜色的透明度并保留原有 RGB。
    /// </summary>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "GraphicExtensions")]
    public static class GraphicExtensions
    {
        /// <summary>只修改 Graphic（Image、RawImage、Text 等）颜色的透明度，保留原有 RGB。</summary>
        /// <param name="graphic">目标 Graphic；为 null 时不执行任何操作。</param>
        /// <param name="alpha">新的透明度，自动限制在 0 到 1。</param>
        /// <remarks>
        /// 只修改 Graphic.color，不影响 CanvasGroup.alpha，最终透明度为两者相乘。
        /// 透明度为 0 的 Graphic 仍会接收点击；Button 的颜色过渡和 CrossFadeAlpha 会叠加在此颜色之上。
        /// </remarks>
        public static void CwSetAlpha(this Graphic graphic, float alpha)
        {
            if (graphic == null)
            {
                return;
            }

            Color color = graphic.color;
            color.a = Mathf.Clamp01(alpha);
            graphic.color = color;
        }
    }
}
