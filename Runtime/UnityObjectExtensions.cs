using System;
using UnityEngine;

/// <summary>
/// 提供 GameObject、Component 和 Transform 的通用扩展操作。
/// </summary>
public static class UnityObjectExtensions
{
    /// <summary>重置 Transform 的本地位置、旋转和缩放。</summary>
    public static void ExtResetLocal(this Transform target)
    {
        if (target == null)
        {
            return;
        }

        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.identity;
        target.localScale = Vector3.one;
    }

    /// <summary>
    /// 设置组件所属游戏对象的激活状态。
    /// </summary>
    /// <param name="target">目标组件。</param>
    /// <param name="active">是否激活游戏对象。</param>
    public static void ExtSetActive(this Component target, bool active)
    {
        if (target == null)
        {
            return;
        }

        target.gameObject.SetActive(active);
    }

    /// <summary>
    /// 将目标的世界坐标和旋转复制为参考对象的世界坐标和旋转。
    /// </summary>
    /// <param name="target">要修改的目标。</param>
    /// <param name="reference">提供世界坐标和旋转的参考对象。</param>
    public static void ExtCopyPoseFrom(this Transform target, Transform reference)
    {
        if (target == null || reference == null)
        {
            return;
        }

        target.SetPositionAndRotation(reference.position, reference.rotation);
    }

    /// <summary>
    /// 将目标的世界坐标复制为参考对象的世界坐标。
    /// </summary>
    /// <param name="target">要修改的目标。</param>
    /// <param name="reference">提供世界坐标的参考对象。</param>
    public static void ExtCopyPositionFrom(this Transform target, Transform reference)
    {
        if (target == null || reference == null)
        {
            return;
        }

        target.position = reference.position;
    }

    /// <summary>
    /// 将目标的世界欧拉角复制为参考对象的世界欧拉角。
    /// </summary>
    /// <param name="target">要修改的目标。</param>
    /// <param name="reference">提供世界欧拉角的参考对象。</param>
    public static void ExtCopyRotationFrom(this Transform target, Transform reference)
    {
        if (target == null || reference == null)
        {
            return;
        }

        target.eulerAngles = reference.eulerAngles;
    }

    /// <summary>
    /// 使目标仅在水平面上朝向参考对象。
    /// </summary>
    /// <param name="target">要调整朝向的目标。</param>
    /// <param name="reference">朝向参考对象。</param>
    public static void ExtLookAtHorizontal(this Transform target, Transform reference)
    {
        if (target == null || reference == null)
        {
            return;
        }

        Vector3 lookPosition = reference.position;
        lookPosition.y = target.position.y;
        target.LookAt(lookPosition);
    }

    /// <summary>
    /// 获取游戏对象上的指定组件；不存在时自动添加。
    /// </summary>
    /// <typeparam name="T">组件类型。</typeparam>
    /// <param name="target">目标游戏对象。</param>
    /// <returns>已存在或新添加的组件；目标为 null 时返回 null。</returns>
    public static T ExtGetOrAddComponent<T>(this GameObject target) where T : Component
    {
        if (target == null)
        {
            return null;
        }

        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.AddComponent<T>();
        }

        return component;
    }

    /// <summary>
    /// 获取组件所在游戏对象上的指定组件；不存在时自动添加。
    /// </summary>
    /// <typeparam name="T">组件类型。</typeparam>
    /// <param name="target">目标组件。</param>
    /// <returns>已存在或新添加的组件；目标为 null 时返回 null。</returns>
    public static T ExtGetOrAddComponent<T>(this Component target) where T : Component
    {
        if (target == null)
        {
            return null;
        }

        return target.gameObject.ExtGetOrAddComponent<T>();
    }

    /// <summary>为目标和全部子物体递归设置 Layer。</summary>
    public static void ExtSetLayerRecursive(this GameObject target, int layer)
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
            root.GetChild(i).gameObject.ExtSetLayerRecursive(layer);
        }
    }

    /// <summary>按名称深度优先递归查找子节点。</summary>
    public static Transform ExtFindChildRecursive(
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

            Transform result = child.ExtFindChildRecursive(childName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>获取 Transform 从场景根节点开始的完整层级路径。</summary>
    public static string ExtGetHierarchyPath(this Transform target)
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

    /// <summary>获取 GameObject 从场景根节点开始的完整层级路径。</summary>
    public static string ExtGetHierarchyPath(this GameObject target)
    {
        return target != null ? target.transform.ExtGetHierarchyPath() : string.Empty;
    }

    /// <summary>清空全部直接子物体。</summary>
    /// <returns>提交删除的子物体数量。</returns>
    public static int ExtClearChildren(this Transform target)
    {
        if (target == null)
        {
            return 0;
        }

        int childCount = target.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = target.GetChild(i).gameObject;
            if (Application.isPlaying)
            {
                child.transform.SetParent(null, true);
                UnityEngine.Object.Destroy(child);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(child);
            }
        }

        return childCount;
    }

    public static void ExtSetPositionX(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.position;
        position.x = value;
        target.position = position;
    }

    public static void ExtSetPositionY(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.position;
        position.y = value;
        target.position = position;
    }

    public static void ExtSetPositionZ(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.position;
        position.z = value;
        target.position = position;
    }

    public static void ExtSetLocalPositionX(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.localPosition;
        position.x = value;
        target.localPosition = position;
    }

    public static void ExtSetLocalPositionY(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.localPosition;
        position.y = value;
        target.localPosition = position;
    }

    public static void ExtSetLocalPositionZ(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.localPosition;
        position.z = value;
        target.localPosition = position;
    }

    /// <summary>计算目标及其子物体全部 Renderer 的世界空间合并包围盒。</summary>
    public static Bounds ExtGetCombinedRendererBounds(
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
}
