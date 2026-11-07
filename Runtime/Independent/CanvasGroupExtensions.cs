using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>
    /// CanvasGroup 扩展：一次设置透明度、交互和射线拦截状态。
    /// </summary>
    public static class CanvasGroupExtensions
    {
        /// <summary>一次设置 CanvasGroup 的透明度、可交互和射线拦截，常用于整体显示、隐藏或禁用一组 UI。</summary>
        /// <param name="target">目标 CanvasGroup；为 null 时不执行任何操作。</param>
        /// <param name="alpha">整组透明度，自动限制在 0 到 1。</param>
        /// <param name="interactable">子级 Button、Toggle 等 Selectable 是否可交互。</param>
        /// <param name="blocksRaycasts">是否接收点击；为 false 时点击会穿透到下层 UI。</param>
        /// <remarks>
        /// alpha 为 0 时 UI 仍会接收点击，完全隐藏时请同时把 interactable 和 blocksRaycasts 设为 false。
        /// 子级 CanvasGroup 勾选 ignoreParentGroups 时不受本组影响。
        /// </remarks>
        public static void CwSetCanvasGroupState(
            this CanvasGroup target,
            float alpha,
            bool interactable,
            bool blocksRaycasts)
        {
            if (target == null)
            {
                return;
            }

            target.alpha = Mathf.Clamp01(alpha);
            target.interactable = interactable;
            target.blocksRaycasts = blocksRaycasts;
        }
    }
}
