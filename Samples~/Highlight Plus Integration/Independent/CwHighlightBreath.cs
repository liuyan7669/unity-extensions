using HighlightPlus;
using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>让物体的高亮描边平滑明暗交替，形成呼吸效果。由 <see cref="GameObjectHighlightExtensions"/> 自动添加和管理。</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    internal sealed class CwHighlightBreath : MonoBehaviour
    {
        /// <summary>一次由亮到暗再到亮的时长（秒）。</summary>
        private const float Period = 1.5f;

        /// <summary>最暗时描边强度相对原强度的比例；平滑描边在 0.4 以上亮度变化不明显，取 0.25 保证明暗对比。</summary>
        private const float MinRatio = 0.25f;

        private HighlightEffect effect;

        /// <summary>启用时记录的描边原强度，呼吸在此基础上变化，停用时恢复。</summary>
        private float baseOutline;

        private float startTime;

        private void OnEnable()
        {
            effect = GetComponent<HighlightEffect>();
            if (effect == null)
            {
                enabled = false;
                return;
            }

            baseOutline = effect.outline;
            startTime = Time.time;
        }

        // HighlightEffect 每帧渲染时按 outline 计算描边透明度，直接修改即可生效，无需刷新材质。
        private void Update()
        {
            if (effect == null)
            {
                enabled = false;
                return;
            }

            float wave = 0.5f + 0.5f * Mathf.Cos((Time.time - startTime) * 2f * Mathf.PI / Period);
            effect.outline = baseOutline * Mathf.Lerp(MinRatio, 1f, wave);
        }

        private void OnDisable()
        {
            if (effect != null)
            {
                effect.outline = baseOutline;
            }
        }
    }
}
