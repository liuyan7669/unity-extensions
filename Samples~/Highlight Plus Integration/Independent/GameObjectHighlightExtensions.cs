using HighlightPlus;
using UnityEngine;
using QualityLevel = HighlightPlus.QualityLevel;

namespace Cowart.CwExtensions
{
    /// <summary>为场景物体提供高亮控制扩展。此扩展类依赖 Highlight Plus。</summary>
    public static class GameObjectHighlightExtensions
    {
        /// <summary>开启或关闭物体的高亮。开启时显示描边（带呼吸效果），可选外发光和表面提亮。</summary>
        /// <param name="target">目标物体，需要带 Renderer；为 null 时不执行任何操作。</param>
        /// <param name="highlighted">true 开启高亮（带淡入）；false 立即关闭，不经过淡出。</param>
        /// <param name="includeBody">开启时物体表面是否也整体提亮。</param>
        /// <param name="includeGlow">开启时是否在描边外显示柔和外发光。</param>
        /// <param name="color">描边、外发光和提亮统一使用的颜色；为 null 时使用默认的暖白描边、金色外发光和白色提亮。</param>
        /// <remarks>首次开启时自动添加 HighlightEffect 和 CwHighlightBreath 组件并应用默认样式，关闭时不移除组件。已有 HighlightEffect 时保留其宽度、质量等设置，只按参数修改颜色、外发光和提亮。</remarks>
        public static void CwSetHighlight(this GameObject target, bool highlighted, bool includeBody = false, bool includeGlow = false, Color? color = null)
        {
            if (target == null)
            {
                return;
            }

            if (!target.TryGetComponent(out HighlightEffect effect))
            {
                if (!highlighted)
                {
                    return;
                }

                // 新添加的组件使用统一的交互提示样式：浅色描边勾勒轮廓，柔和暖光缓慢呼吸，表面只轻微提亮。
                effect = target.AddComponent<HighlightEffect>();
                effect.fadeInDuration = 0.25f;

                effect.outline = 1f;
                effect.outlineWidth = 0.6f;
                effect.outlineQuality = QualityLevel.Highest;

                effect.glowWidth = 2f;
                effect.glowQuality = QualityLevel.Highest;
                effect.glowAnimationSpeed = 0.6f;

                effect.overlayAnimationSpeed = 0.5f;

                effect.innerGlow = 0f;
            }

            if (highlighted)
            {
                // 实体提亮由 overlay 控制。HighlightEffect 刷新材质时会把 overlay 抬到 overlayMinIntensity 以上，
                // 因此关闭实体提亮时两者需同时置零。
                effect.overlay = includeBody ? 0.3f : 0f;
                effect.overlayMinIntensity = includeBody ? 0.3f : 0f;
                effect.glow = includeGlow ? 0.7f : 0f;
                effect.outlineColor = color ?? new Color(1f, 0.96f, 0.85f);
                effect.glowHQColor = color ?? new Color(1f, 0.78f, 0.35f);
                effect.overlayColor = color ?? Color.white;
                effect.enabled = true;
                effect.UpdateMaterialProperties();

                if (!target.TryGetComponent(out CwHighlightBreath breath))
                {
                    breath = target.AddComponent<CwHighlightBreath>();
                }

                breath.enabled = true;
            }
            else
            {
                if (target.TryGetComponent(out CwHighlightBreath breath))
                {
                    breath.enabled = false;
                }

                // 取消时立即关闭，不经过淡出：fadeOutDuration 为 0 时 Highlight Plus 会直接结束高亮。
                // 只在本次调用中临时置 0，随后恢复，保留物体原有的淡出配置。
                float fadeOutDuration = effect.fadeOutDuration;
                effect.fadeOutDuration = 0f;
                effect.highlighted = false;
                effect.fadeOutDuration = fadeOutDuration;
                return;
            }

            effect.highlighted = true;
        }
    }
}
