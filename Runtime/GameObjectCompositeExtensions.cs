using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>
    /// 存放由多个独立扩展组合而成的游戏对象扩展方法；只依赖 Unity 内置模块。
    /// </summary>
    public static class GameObjectCompositeExtensions
    {
        private const float MinimumColliderSize = 0.001f;

        /// <summary>在物体上添加或更新一个 BoxCollider，使其正好包住物体及全部子孙的模型和子碰撞体。</summary>
        /// <param name="target">承载 BoxCollider 的物体。</param>
        /// <param name="includeInactive">为 true（默认）时把未激活的子物体也计算在内。</param>
        /// <returns>添加或更新后的 BoxCollider；target 为 null 或层级中没有可计算内容时返回 null，且不会添加组件。</returns>
        /// <remarks>组合 CwTryGetHierarchyLocalBounds 与 CwGetOrAddComponent。已有 BoxCollider 会被复用，子物体的碰撞体不会删除。只按调用时的层级计算一次，模型变化后需要再次调用。</remarks>
        public static BoxCollider CwAddFittedBoxCollider(
            this GameObject target,
            bool includeInactive = true)
        {
            if (target == null ||
            !target.transform.CwTryGetHierarchyLocalBounds(out Bounds localBounds, includeInactive))
        {
            return null;
        }

        BoxCollider boxCollider = target.CwGetOrAddComponent<BoxCollider>();
        boxCollider.center = localBounds.center;
            boxCollider.size = ClampColliderSize(localBounds.size);
            return boxCollider;
        }

        /// <summary>在组件所在物体上添加或更新一个 BoxCollider，使其正好包住物体及全部子孙的模型和子碰撞体。</summary>
        /// <param name="target">承载 BoxCollider 的物体上的任意组件。</param>
        /// <param name="includeInactive">为 true（默认）时把未激活的子物体也计算在内。</param>
        /// <returns>添加或更新后的 BoxCollider；target 为 null 或层级中没有可计算内容时返回 null。</returns>
        public static BoxCollider CwAddFittedBoxCollider(
            this Component target,
            bool includeInactive = true)
        {
            return target != null
                ? target.gameObject.CwAddFittedBoxCollider(includeInactive)
                : null;
        }

        /// <summary>将 BoxCollider 尺寸的每个轴限制为 Unity 可用的最小正数。</summary>
        /// <param name="size">计算得到的原始 BoxCollider 尺寸。</param>
        /// <returns>每个轴都不小于最小碰撞体尺寸的结果。</returns>
        private static Vector3 ClampColliderSize(Vector3 size)
        {
            return new Vector3(
                Mathf.Max(size.x, MinimumColliderSize),
                Mathf.Max(size.y, MinimumColliderSize),
                Mathf.Max(size.z, MinimumColliderSize));
        }
    }
}
