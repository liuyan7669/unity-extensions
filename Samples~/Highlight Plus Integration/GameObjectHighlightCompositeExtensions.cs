using System;
using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>存放依赖 Highlight Plus 的组合扩展：由包内点击扩展和本示例的高亮扩展组合而成。</summary>
    public static class GameObjectHighlightCompositeExtensions
    {
        /// <summary>高亮物体并等待一次左键点击；点击后自动取消高亮和点击监听，再执行回调。常用于引导用户点击指定物体。</summary>
        /// <param name="target">目标物体；为 null 时不执行任何操作。</param>
        /// <param name="onClicked">点击后执行的回调；重复调用会替换上一次的回调，传入 null 时取消高亮和监听。</param>
        /// <param name="includeBody">高亮时物体表面是否也整体提亮。</param>
        /// <param name="includeGlow">高亮时是否显示外发光。</param>
        /// <param name="color">高亮颜色；为 null 时使用默认样式颜色。</param>
        /// <remarks>组合 CwSetHighlight 与 CwSetClickAction，点击条件与 CwSetClickAction 相同。只响应一次点击；需要每次点击都触发时，请分别调用这两个方法。</remarks>
        public static void CwSetHighlightClick(this GameObject target, Action onClicked, bool includeBody = false, bool includeGlow = false, Color? color = null)
        {
            if (target == null)
            {
                return;
            }

            if (onClicked == null)
            {
                target.CwSetHighlight(false);
                target.CwSetClickAction(null);
                return;
            }

            target.CwSetHighlight(true, includeBody, includeGlow, color);
            target.CwSetClickAction(() =>
            {
                target.CwSetHighlight(false);
                target.CwSetClickAction(null);
                onClicked();
            });
        }
    }
}
