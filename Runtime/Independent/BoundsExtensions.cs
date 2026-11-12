using UnityEngine;

namespace Cowart.CwExtensions
{
    /// <summary>
    /// 提供层级 Renderer 世界包围盒和层级局部包围盒计算扩展。
    /// </summary>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "BoundsExtensions")]
    public static class BoundsExtensions
    {
        /// <summary>计算 target 及全部子孙物体上所有 Renderer 的世界空间合并包围盒（轴对齐）。</summary>
        /// <param name="target">层级根节点。</param>
        /// <param name="includeInactive">是否包含未激活物体上的 Renderer。</param>
        /// <returns>合并后的世界包围盒；没有 Renderer 时返回中心在 target 位置、尺寸为 0 的包围盒；target 为 null 时返回 default。</returns>
        /// <remarks>物体旋转后轴对齐包围盒会比模型实际大小更大；粒子、拖尾等 Renderer 也会参与计算。</remarks>
        public static Bounds CwGetCombinedRendererBounds(
            this Transform target,
            bool includeInactive = false)
        {
            if (target == null)
            {
                return default;
            }

            Renderer[] renderers = target.GetComponentsInChildren<Renderer>(includeInactive);
            if (renderers.Length == 0)
            {
                return new Bounds(target.position, Vector3.zero);
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

    /// <summary>计算 target 及全部子孙物体在 target 局部坐标系中的合并包围盒，结果可直接用于 target 上的 BoxCollider。</summary>
    /// <param name="target">层级根节点，同时提供局部坐标系。</param>
    /// <param name="localBounds">成功时为 target 局部坐标系中的包围盒，失败时为 default。</param>
    /// <param name="includeInactive">为 true（默认）时包含未激活物体。</param>
    /// <returns>target 不为 null 且层级中有可计算的内容时返回 true。</returns>
    /// <remarks>计算 target 及子孙的 Renderer 和子孙的 3D Collider，target 自身的 Collider 不参与；两者都没有时退回使用子节点位置。比 CwGetCombinedRendererBounds 更贴合旋转后的模型。</remarks>
    public static bool CwTryGetHierarchyLocalBounds(
        this Transform target,
        out Bounds localBounds,
        bool includeInactive = true)
    {
        if (target == null)
        {
            localBounds = default;
            return false;
        }

        return TryCalculateHierarchyLocalBounds(target, includeInactive, out localBounds);
    }

        /// <summary>计算目标层级在根节点局部坐标系中的包围盒。</summary>
        /// <param name="root">提供局部坐标系和搜索起点的根节点。</param>
        /// <param name="includeInactive">是否包含未激活的子物体。</param>
        /// <param name="localBounds">计算成功时返回根节点局部坐标系中的包围盒。</param>
        /// <returns>找到 Renderer、子 Collider 或子节点位置时返回 true，否则返回 false。</returns>
        private static bool TryCalculateHierarchyLocalBounds(
            Transform root,
            bool includeInactive,
            out Bounds localBounds)
        {
            LocalBoundsAccumulator accumulator = new LocalBoundsAccumulator(root);

            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(includeInactive);
            for (int i = 0; i < renderers.Length; i++)
            {
                EncapsulateRendererBounds(renderers[i], ref accumulator);
            }

            Collider[] colliders = root.GetComponentsInChildren<Collider>(includeInactive);
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider childCollider = colliders[i];
                if (childCollider.transform == root)
                {
                    continue;
                }

                EncapsulateColliderBounds(childCollider, ref accumulator);
            }

            if (!accumulator.HasBounds)
            {
                Transform[] children = root.GetComponentsInChildren<Transform>(includeInactive);
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i] != root)
                    {
                        accumulator.EncapsulateWorldPoint(children[i].position);
                    }
                }
            }

            return accumulator.TryGetBounds(out localBounds);
        }

        /// <summary>将 Renderer 的可见范围合并到目标局部包围盒。</summary>
        /// <param name="childRenderer">要合并的子物体 Renderer。</param>
        /// <param name="accumulator">负责转换坐标并累计包围范围的累加器。</param>
        private static void EncapsulateRendererBounds(
            Renderer childRenderer,
            ref LocalBoundsAccumulator accumulator)
        {
            if (childRenderer is SkinnedMeshRenderer skinnedMeshRenderer)
            {
                accumulator.EncapsulateLocalBounds(
                    skinnedMeshRenderer.localToWorldMatrix,
                    skinnedMeshRenderer.localBounds);
                return;
            }

            MeshFilter meshFilter = childRenderer.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                accumulator.EncapsulateLocalBounds(
                    childRenderer.localToWorldMatrix,
                    meshFilter.sharedMesh.bounds);
                return;
            }

            accumulator.EncapsulateWorldBounds(childRenderer.bounds);
        }

        /// <summary>将 3D Collider 的包围范围合并到目标局部包围盒。</summary>
        /// <param name="childCollider">要合并的子物体 Collider。</param>
        /// <param name="accumulator">负责转换坐标并累计包围范围的累加器。</param>
        private static void EncapsulateColliderBounds(
            Collider childCollider,
            ref LocalBoundsAccumulator accumulator)
        {
            if (childCollider is BoxCollider boxCollider)
            {
                accumulator.EncapsulateLocalBounds(
                    boxCollider.transform.localToWorldMatrix,
                    new Bounds(boxCollider.center, boxCollider.size));
                return;
            }

            if (childCollider is SphereCollider sphereCollider)
            {
                accumulator.EncapsulateLocalBounds(
                    sphereCollider.transform.localToWorldMatrix,
                    new Bounds(
                        sphereCollider.center,
                        Vector3.one * sphereCollider.radius * 2f));
                return;
            }

            if (childCollider is CapsuleCollider capsuleCollider)
            {
                Vector3 size = Vector3.one * capsuleCollider.radius * 2f;
                size[capsuleCollider.direction] = Mathf.Max(
                    capsuleCollider.height,
                    capsuleCollider.radius * 2f);
                accumulator.EncapsulateLocalBounds(
                    capsuleCollider.transform.localToWorldMatrix,
                    new Bounds(capsuleCollider.center, size));
                return;
            }

            if (childCollider is MeshCollider meshCollider && meshCollider.sharedMesh != null)
            {
                accumulator.EncapsulateLocalBounds(
                    meshCollider.transform.localToWorldMatrix,
                    meshCollider.sharedMesh.bounds);
                return;
            }

            accumulator.EncapsulateWorldBounds(childCollider.bounds);
        }

        /// <summary>
        /// 将不同局部坐标系中的包围盒转换并累计到指定根节点的局部坐标系。
        /// </summary>
        private struct LocalBoundsAccumulator
        {
            private readonly Transform root;
            private Bounds bounds;

            /// <summary>是否已经累计至少一个有效点。</summary>
            public bool HasBounds { get; private set; }

            /// <summary>创建以指定 Transform 作为局部坐标系的包围盒累加器。</summary>
            /// <param name="root">提供最终局部坐标系的根节点。</param>
            public LocalBoundsAccumulator(Transform root)
            {
                this.root = root;
                bounds = default;
                HasBounds = false;
            }

            /// <summary>合并一个世界空间轴对齐包围盒。</summary>
            /// <param name="worldBounds">要合并的世界空间包围盒。</param>
            public void EncapsulateWorldBounds(Bounds worldBounds)
            {
                EncapsulateLocalBounds(Matrix4x4.identity, worldBounds);
            }

            /// <summary>将指定局部空间包围盒的八个角转换到世界空间后进行合并。</summary>
            /// <param name="localToWorldMatrix">包围盒局部坐标到世界坐标的转换矩阵。</param>
            /// <param name="localBounds">要合并的局部空间包围盒。</param>
            public void EncapsulateLocalBounds(
                Matrix4x4 localToWorldMatrix,
                Bounds localBounds)
            {
                Vector3 center = localBounds.center;
                Vector3 extents = localBounds.extents;

                for (int xSign = -1; xSign <= 1; xSign += 2)
                {
                    for (int ySign = -1; ySign <= 1; ySign += 2)
                    {
                        for (int zSign = -1; zSign <= 1; zSign += 2)
                        {
                            Vector3 corner = center + Vector3.Scale(
                                extents,
                                new Vector3(xSign, ySign, zSign));
                            EncapsulateWorldPoint(localToWorldMatrix.MultiplyPoint3x4(corner));
                        }
                    }
                }
            }

            /// <summary>将一个世界坐标点转换到根节点局部坐标并合并。</summary>
            /// <param name="worldPoint">要合并的世界坐标点。</param>
            public void EncapsulateWorldPoint(Vector3 worldPoint)
            {
                Vector3 localPoint = root.InverseTransformPoint(worldPoint);
                if (!HasBounds)
                {
                    bounds = new Bounds(localPoint, Vector3.zero);
                    HasBounds = true;
                    return;
                }

                bounds.Encapsulate(localPoint);
            }

            /// <summary>尝试获取当前累计的根节点局部包围盒。</summary>
            /// <param name="localBounds">已经累计的根节点局部包围盒。</param>
            /// <returns>已经累计至少一个有效点时返回 true，否则返回 false。</returns>
            public bool TryGetBounds(out Bounds localBounds)
            {
                localBounds = bounds;
                return HasBounds;
            }
        }
    }
}
