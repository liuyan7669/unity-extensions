using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提供 RectTransform 布局树的即时和延迟重建扩展。
/// </summary>
public static class UILayoutExtensions
{
    /// <summary>
    /// 按照先子节点、后父节点的顺序立即重建整个布局树。
    /// </summary>
    /// <param name="root">要重建的布局树根节点。</param>
    public static void ExtRebuildLayoutTree(this RectTransform root)
    {
        if (root == null)
        {
            return;
        }

        Canvas.ForceUpdateCanvases();
        RebuildLayoutTree(root);
    }

    /// <summary>
    /// 在下一帧重建目标自身或其最近的布局控制节点。
    /// </summary>
    /// <param name="target">需要刷新布局的目标。</param>
    /// <param name="coroutineRunner">用于启动协程的组件；为空时从目标父级自动查找。</param>
    /// <remarks>目标需要保持有效至下一帧布局重建完成。</remarks>
    public static void ExtRebuildNearestLayoutNextFrame(
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

    /// <summary>
    /// 递归重建布局树，确保子节点先于父节点更新。
    /// </summary>
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

    /// <summary>
    /// 等待一帧后重建最近的布局控制节点。
    /// </summary>
    private static IEnumerator RebuildNearestLayoutNextFrame(RectTransform target)
    {
        yield return null;
        RebuildNearestLayout(target);
    }

    /// <summary>
    /// 查找目标自身或最近的 LayoutGroup、ContentSizeFitter，并立即重建布局。
    /// </summary>
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
