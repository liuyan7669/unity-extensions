using UnityEngine;

namespace Cowart.CwExtensions
{
    /// <summary>
    /// RectTransform 扩展：获取 UI 组件的 RectTransform、锚点坐标与尺寸的单轴赋值、保持视觉位置修改 Pivot。
    /// </summary>
    /// <remarks>受 LayoutGroup 或 ContentSizeFitter 控制的元素，位置和尺寸会在下次布局重建时被覆盖。</remarks>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "RectTransformExtensions")]
    public static class RectTransformExtensions
    {
        /// <summary>获取组件所在物体的 RectTransform。</summary>
        /// <param name="component">任意组件，例如 Button、Image、TMP_Text。</param>
        /// <returns>物体是 UI 物体时返回其 RectTransform；组件为 null 或物体只有普通 Transform 时返回 null。</returns>
        public static RectTransform CwGetRect(this Component component)
        {
            return component != null ? component.transform as RectTransform : null;
        }

        /// <summary>只修改 anchoredPosition 的 x，保留 y。</summary>
        /// <param name="target">目标 RectTransform；为 null 时不执行任何操作。</param>
        /// <param name="value">新的 x，即 Pivot 相对锚点的水平偏移。</param>
        public static void CwSetAnchoredPositionX(this RectTransform target, float value)
        {
            if (target == null)
            {
                return;
            }

            Vector2 position = target.anchoredPosition;
            position.x = value;
            target.anchoredPosition = position;
        }

        /// <summary>只修改 anchoredPosition 的 y，保留 x。</summary>
        /// <param name="target">目标 RectTransform；为 null 时不执行任何操作。</param>
        /// <param name="value">新的 y，即 Pivot 相对锚点的垂直偏移。</param>
        public static void CwSetAnchoredPositionY(this RectTransform target, float value)
        {
            if (target == null)
            {
                return;
            }

            Vector2 position = target.anchoredPosition;
            position.y = value;
            target.anchoredPosition = position;
        }

        /// <summary>只修改 sizeDelta 的 x，保留 y。</summary>
        /// <param name="target">目标 RectTransform；为 null 时不执行任何操作。</param>
        /// <param name="value">新的 sizeDelta.x。</param>
        /// <remarks>左右锚点重合时等于宽度；左右锚点拉开时表示比锚点范围多出（负数为少出）的宽度，不等于实际宽度。</remarks>
        public static void CwSetSizeDeltaX(this RectTransform target, float value)
        {
            if (target == null)
            {
                return;
            }

            Vector2 size = target.sizeDelta;
            size.x = value;
            target.sizeDelta = size;
        }

        /// <summary>只修改 sizeDelta 的 y，保留 x。</summary>
        /// <param name="target">目标 RectTransform；为 null 时不执行任何操作。</param>
        /// <param name="value">新的 sizeDelta.y。</param>
        /// <remarks>上下锚点重合时等于高度；上下锚点拉开时表示比锚点范围多出（负数为少出）的高度，不等于实际高度。</remarks>
        public static void CwSetSizeDeltaY(this RectTransform target, float value)
        {
            if (target == null)
            {
                return;
            }

            Vector2 size = target.sizeDelta;
            size.y = value;
            target.sizeDelta = size;
        }

        /// <summary>修改 Pivot，同时移动位置补偿，使 UI 在屏幕上的位置和大小保持不变。</summary>
        /// <param name="target">目标 RectTransform；为 null 时不执行任何操作。</param>
        /// <param name="pivot">新的 Pivot，(0, 0) 为左下角，(1, 1) 为右上角。</param>
        /// <remarks>修改后 anchoredPosition 会随之变化。常用于在缩放或旋转前更换中心点。</remarks>
        public static void CwSetPivotKeepPosition(this RectTransform target, Vector2 pivot)
        {
            if (target == null)
            {
                return;
            }

            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners);
            Vector3 originalBottomLeft = corners[0];

            target.pivot = pivot;
            target.GetWorldCorners(corners);
            target.position += originalBottomLeft - corners[0];
        }
    }
}
