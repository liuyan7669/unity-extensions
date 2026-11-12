using System;
using UnityEngine;

namespace Cowart.CwExtensions
{
    /// <summary>
    /// 层级结构扩展：按名称递归查找子节点、获取层级路径、清空直接子物体。
    /// </summary>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "HierarchyExtensions")]
    public static class HierarchyExtensions
    {
        /// <summary>按名称在全部子孙节点中深度优先查找第一个匹配的节点，未激活的物体也会被找到。</summary>
        /// <param name="target">查找起点；为 null 时返回 null。</param>
        /// <param name="childName">要查找的名称，区分大小写且必须完全相等；为 null 或空时返回 null。</param>
        /// <param name="includeSelf">为 true 时先检查 target 自身的名称。</param>
        /// <returns>最先找到的同名节点；找不到时返回 null。</returns>
        /// <remarks>存在多个同名节点时只返回最先遇到的一个。节点很多时开销较大，避免每帧调用，查到后请缓存结果。</remarks>
        public static Transform CwFindChildRecursive(
            this Transform target,
            string childName,
            bool includeSelf = false)
        {
            if (target == null || string.IsNullOrEmpty(childName))
            {
                return null;
            }

            if (includeSelf && string.Equals(target.name, childName, StringComparison.Ordinal))
            {
                return target;
            }

            for (int i = 0; i < target.childCount; i++)
            {
                Transform child = target.GetChild(i);
                if (string.Equals(child.name, childName, StringComparison.Ordinal))
                {
                    return child;
                }

                Transform result = child.CwFindChildRecursive(childName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>获取从场景根节点到 target 的完整路径，例如 Canvas/Panel/Button，常用于日志和排查问题。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <returns>以 / 分隔的路径，不含场景名；target 为 null 时返回空字符串。</returns>
        public static string CwGetHierarchyPath(this Transform target)
        {
            if (target == null)
            {
                return string.Empty;
            }

            string path = target.name;
            Transform parent = target.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }

        /// <summary>获取从场景根节点到 target 的完整路径，例如 Canvas/Panel/Button，常用于日志和排查问题。</summary>
        /// <param name="target">目标物体。</param>
        /// <returns>以 / 分隔的路径，不含场景名；target 为 null 时返回空字符串。</returns>
        public static string CwGetHierarchyPath(this GameObject target)
        {
            return target != null ? target.transform.CwGetHierarchyPath() : string.Empty;
        }

        /// <summary>销毁 target 的全部直接子物体（连同它们的子孙）。</summary>
        /// <param name="target">父节点。</param>
        /// <returns>销毁的直接子物体数量；编辑模式下或 target 为 null 时返回 0。</returns>
        /// <remarks>只在运行模式下执行，编辑模式下不做任何修改，避免误删场景或 Prefab。子物体会先移出层级再销毁，调用后 childCount 立即为 0，对象在帧末才真正销毁。</remarks>
        public static int CwClearChildren(this Transform target)
        {
            if (target == null || !Application.isPlaying)
            {
                return 0;
            }

            int childCount = target.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject child = target.GetChild(i).gameObject;
                child.transform.SetParent(null, true);
                UnityEngine.Object.Destroy(child);
            }

            return childCount;
        }
    }
}
