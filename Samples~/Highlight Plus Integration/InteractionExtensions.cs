using System;
using Cowart.EventRelays;
using HighlightPlus;
using UnityEngine;

/// <summary>
/// 提供场景对象的默认高亮样式和指针按下事件绑定扩展。
/// 此扩展类依赖 Highlight Plus。
/// </summary>
public static class InteractionExtensions
{
    /// <summary>
    /// 设置游戏对象的高亮状态，并应用默认高亮样式。
    /// </summary>
    /// <param name="target">目标游戏对象。</param>
    /// <param name="highlighted">是否启用高亮。</param>
    /// <returns>目标上的 HighlightEffect；目标为 null 时返回 null。</returns>
    /// <remarks>调用时始终确保目标存在 HighlightEffect，即使 highlighted 为 false。</remarks>
    public static HighlightEffect ExtSetHighlight(this GameObject target, bool highlighted)
    {
        if (target == null)
        {
            return null;
        }

        HighlightEffect highlightEffect = target.ExtGetOrAddComponent<HighlightEffect>();
        ApplyDefaultHighlightStyle(highlightEffect, highlighted);
        return highlightEffect;
    }

    /// <summary>
    /// 设置组件所在游戏对象的高亮状态，并应用默认高亮样式。
    /// </summary>
    /// <param name="target">目标组件。</param>
    /// <param name="highlighted">是否启用高亮。</param>
    /// <returns>目标上的 HighlightEffect；目标为 null 时返回 null。</returns>
    /// <remarks>调用时始终确保目标存在 HighlightEffect，即使 highlighted 为 false。</remarks>
    public static HighlightEffect ExtSetHighlight(this Component target, bool highlighted)
    {
        if (target == null)
        {
            return null;
        }

        return target.gameObject.ExtSetHighlight(highlighted);
    }

    /// <summary>
    /// 为游戏对象绑定指针按下回调。
    /// </summary>
    /// <param name="target">要监听指针按下事件的游戏对象。</param>
    /// <param name="callback">指针按下时执行的回调。</param>
    /// <param name="triggerOnce">首次触发后是否停止监听。</param>
    /// <param name="highlightBeforeFirstTrigger">绑定后是否保持高亮，直到首次触发。</param>
    /// <remarks>
    /// 此方法会覆盖 PointerEventRelay.pointerDownNoParamAction。
    /// triggerOnce 为 true 时，首次触发后会停用整个 PointerEventRelay。
    /// </remarks>
    public static void ExtBindPointerDown(
        this GameObject target,
        Action callback,
        bool triggerOnce = true,
        bool highlightBeforeFirstTrigger = true)
    {
        if (target == null)
        {
            return;
        }

        target.ExtSetHighlight(highlightBeforeFirstTrigger);

        PointerEventRelay pointerRelay = target.ExtGetOrAddComponent<PointerEventRelay>();
        pointerRelay.enable = true;
        pointerRelay.pointerDownNoParamAction = () =>
        {
            target.ExtSetHighlight(false);
            if (triggerOnce)
            {
                pointerRelay.enable = false;
            }

            callback?.Invoke();
        };
    }

    /// <summary>
    /// 为组件所在游戏对象绑定指针按下回调。
    /// </summary>
    /// <param name="target">要监听指针按下事件的组件。</param>
    /// <param name="callback">指针按下时执行的回调。</param>
    /// <param name="triggerOnce">首次触发后是否停止监听。</param>
    /// <param name="highlightBeforeFirstTrigger">绑定后是否保持高亮，直到首次触发。</param>
    /// <remarks>
    /// 此方法会覆盖 PointerEventRelay.pointerDownNoParamAction。
    /// triggerOnce 为 true 时，首次触发后会停用整个 PointerEventRelay。
    /// </remarks>
    public static void ExtBindPointerDown(
        this Component target,
        Action callback,
        bool triggerOnce = true,
        bool highlightBeforeFirstTrigger = true)
    {
        if (target == null)
        {
            return;
        }

        target.gameObject.ExtBindPointerDown(callback, triggerOnce, highlightBeforeFirstTrigger);
    }

    /// <summary>
    /// 应用工具箱默认高亮样式和目标状态。
    /// </summary>
    private static void ApplyDefaultHighlightStyle(HighlightEffect highlightEffect, bool highlighted)
    {
        highlightEffect.SetHighlighted(highlighted);
        highlightEffect.outlineColor = Color.cyan;
        highlightEffect.outlineWidth = 0.65f;
        highlightEffect.constantWidth = false;
        highlightEffect.overlayMinIntensity = 0f;
        highlightEffect.overlay = 0f;
        highlightEffect.glowDithering = false;
    }
}
