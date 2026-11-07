using System;
using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>Layer 扩展：判断、添加和移除 LayerMask 中的 Layer，以及为整个层级递归设置 Layer。</summary>
    /// <remarks>LayerMask 是值类型，CwAddLayer 和 CwRemoveLayer 返回新值，需要接收返回值，例如 mask = mask.CwAddLayer(8)。</remarks>
    public static class LayerExtensions
    {
        /// <summary>判断 LayerMask 是否包含指定 Layer。</summary>
        /// <param name="mask">要检查的 LayerMask。</param>
        /// <param name="layer">Layer 索引（0 到 31），可用 LayerMask.NameToLayer 获取。</param>
        /// <returns>包含时返回 true；layer 超出 0 到 31 时返回 false。</returns>
        public static bool CwContains(this LayerMask mask, int layer)
        {
            return layer >= 0 && layer <= 31 && (mask.value & (1 << layer)) != 0;
        }

        /// <summary>判断 LayerMask 是否包含物体所在的 Layer，常用于在碰撞或射线回调中过滤物体。</summary>
        /// <param name="mask">要检查的 LayerMask。</param>
        /// <param name="target">要检查的物体。</param>
        /// <returns>包含时返回 true；target 为 null 时返回 false。</returns>
        public static bool CwContains(this LayerMask mask, GameObject target)
        {
            return target != null && mask.CwContains(target.layer);
        }

        /// <summary>返回加入指定 Layer 后的新 LayerMask，原值不变。</summary>
        /// <param name="mask">原 LayerMask。</param>
        /// <param name="layer">要加入的 Layer 索引（0 到 31）；超出范围时原样返回。</param>
        /// <returns>加入后的 LayerMask。</returns>
        public static LayerMask CwAddLayer(this LayerMask mask, int layer)
        {
            if (layer >= 0 && layer <= 31) mask.value |= 1 << layer;
            return mask;
        }

        /// <summary>返回移除指定 Layer 后的新 LayerMask，原值不变。</summary>
        /// <param name="mask">原 LayerMask。</param>
        /// <param name="layer">要移除的 Layer 索引（0 到 31）；超出范围时原样返回。</param>
        /// <returns>移除后的 LayerMask。</returns>
        public static LayerMask CwRemoveLayer(this LayerMask mask, int layer)
        {
            if (layer >= 0 && layer <= 31) mask.value &= ~(1 << layer);
            return mask;
        }

        /// <summary>把物体自身及全部子孙物体（包括未激活的）设置到同一个 Layer。</summary>
        /// <param name="target">层级根物体；为 null 时不执行任何操作。</param>
        /// <param name="layer">Layer 索引（0 到 31），可用 LayerMask.NameToLayer 获取。</param>
        /// <exception cref="ArgumentOutOfRangeException">layer 不在 0 到 31 之间。</exception>
        /// <remarks>子物体原有的 Layer 会被覆盖。</remarks>
        public static void CwSetLayerRecursive(this GameObject target, int layer)
        {
            if (target == null)
            {
                return;
            }

            if (layer < 0 || layer > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(layer), "Layer 必须位于 0 到 31。");
            }

            target.layer = layer;
            Transform root = target.transform;
            for (int i = 0; i < root.childCount; i++)
            {
                root.GetChild(i).gameObject.CwSetLayerRecursive(layer);
            }
        }
    }
}
