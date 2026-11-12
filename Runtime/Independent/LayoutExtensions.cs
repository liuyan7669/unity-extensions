using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cowart.CwExtensions
{
    /// <summary>
    /// UGUI 布局扩展：立即重建整个布局树，或在下一帧重建最近的布局控制节点。用于刷新 ScrollRect 内容等动态布局。
    /// </summary>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "LayoutExtensions")]
    public static class LayoutExtensions
    {
        /// <summary>立即重建以 root 为根的整个布局树（先子节点、后父节点），调用后即可读取正确的尺寸和位置。</summary>
        /// <param name="root">布局树根节点，例如 ScrollRect 的 Content；为 null 时不执行任何操作。</param>
        /// <remarks>
        /// 用于动态增删子物体、修改文本后需要在同一帧拿到正确尺寸的场景，例如刷新后滚动到底部。
        /// 会对每个子节点都执行一次重建，开销随节点数量增长，避免每帧调用。
        /// </remarks>
        public static void CwRebuildLayoutTree(this RectTransform root)
        {
            if (root == null)
            {
                return;
            }

            Canvas.ForceUpdateCanvases();
            RebuildLayoutTree(root);
        }

        /// <summary>下一帧重建 target 自身或向上最近的 LayoutGroup / ContentSizeFitter 节点。</summary>
        /// <param name="target">需要刷新布局的节点；为 null 时不执行任何操作。</param>
        /// <param name="coroutineRunner">运行等待协程的组件；为 null 时取 target 父级链上第一个 MonoBehaviour，找不到时输出警告并放弃。</param>
        /// <remarks>
        /// 用于本帧尺寸还没算出来的情况，例如刚修改文本或刚激活物体。
        /// 只在运行模式下生效；coroutineRunner 所在物体在下一帧前被禁用或销毁时不会重建；target 在下一帧前被销毁时不执行重建。
        /// 向上一直找不到布局组件时，会重建到最顶层的 RectTransform。
        /// </remarks>
        public static void CwRebuildNearestLayoutNextFrame(
            this RectTransform target,
            MonoBehaviour coroutineRunner = null)
        {
            if (target == null)
            {
                return;
            }

            if (coroutineRunner == null)
            {
                coroutineRunner = target.GetComponentInParent<MonoBehaviour>();
            }

            if (coroutineRunner == null)
            {
                Debug.LogWarning("下一帧重建布局需要一个 MonoBehaviour 来运行协程。", target);
                return;
            }

            coroutineRunner.StartCoroutine(RebuildNearestLayoutNextFrame(target));
        }

        /// <summary>递归重建布局树，确保子节点先于父节点更新。</summary>
        /// <param name="root">当前要重建的节点。</param>
        private static void RebuildLayoutTree(RectTransform root)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                RectTransform child = root.GetChild(i) as RectTransform;
                if (child != null)
                {
                    RebuildLayoutTree(child);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(root);
        }

        /// <summary>等待一帧后重建最近的布局控制节点。</summary>
        /// <param name="target">需要刷新布局的节点。</param>
        /// <returns>协程迭代器。</returns>
        private static IEnumerator RebuildNearestLayoutNextFrame(RectTransform target)
        {
            yield return null;

            // 目标可能在等待期间被销毁。
            if (target != null)
            {
                RebuildNearestLayout(target);
            }
        }

        /// <summary>查找目标自身或最近的 LayoutGroup、ContentSizeFitter，并立即重建布局。</summary>
        /// <param name="target">查找起点。</param>
        private static void RebuildNearestLayout(RectTransform target)
        {
            RectTransform layoutRoot = target;
            while (layoutRoot.parent != null && layoutRoot.parent is RectTransform)
            {
                if (layoutRoot.GetComponent<LayoutGroup>() != null ||
                    layoutRoot.GetComponent<ContentSizeFitter>() != null)
                {
                    break;
                }

                layoutRoot = layoutRoot.parent as RectTransform;
            }

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
        }
    }
}
