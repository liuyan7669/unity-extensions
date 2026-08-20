using System;
using UnityEngine;

/// <summary>
/// 提供 GameObject、Component 和 Transform 的通用扩展操作。
/// </summary>
public static class UnityObjectExtensions
{
    private const float MinimumColliderSize = 0.001f;

    /// <summary>重置 Transform 的本地位置。</summary>
    /// <param name="target">要重置本地位置的 Transform。</param>
    public static void ExtResetLocalPosition(this Transform target)
    {
        if (target == null)
        {
            return;
        }

        target.localPosition = Vector3.zero;
    }

    /// <summary>重置 Transform 的本地旋转。</summary>
    /// <param name="target">要重置本地旋转的 Transform。</param>
    public static void ExtResetLocalRotation(this Transform target)
    {
        if (target == null)
        {
            return;
        }

        target.localRotation = Quaternion.identity;
    }

    /// <summary>重置 Transform 的本地缩放。</summary>
    /// <param name="target">要重置本地缩放的 Transform。</param>
    public static void ExtResetLocalScale(this Transform target)
    {
        if (target == null)
        {
            return;
        }

        target.localScale = Vector3.one;
    }

    /// <summary>重置 Transform 的本地位置、旋转和缩放。</summary>
    /// <param name="target">要重置本地 Transform 数据的目标。</param>
    public static void ExtResetLocal(this Transform target)
    {
        if (target == null)
        {
            return;
        }

        target.ExtResetLocalPosition();
        target.ExtResetLocalRotation();
        target.ExtResetLocalScale();
    }

    /// <summary>
    /// 设置 Transform 的父节点，并重置本地位置、旋转和缩放。
    /// </summary>
    /// <param name="target">要设置父节点的 Transform。</param>
    /// <param name="parent">新的父节点；传入 null 时移动到场景根节点。</param>
    public static void ExtSetParentAndResetLocal(this Transform target, Transform parent)
    {
        if (target == null)
        {
            return;
        }

        target.SetParent(parent, false);
        target.ExtResetLocal();
    }

    /// <summary>
    /// 复制参考对象的本地位置、旋转和缩放。
    /// </summary>
    /// <param name="target">要修改的目标。</param>
    /// <param name="reference">提供本地 Transform 数据的参考对象。</param>
    public static void ExtCopyLocalTransformFrom(this Transform target, Transform reference)
    {
        if (target == null || reference == null)
        {
            return;
        }

        target.localPosition = reference.localPosition;
        target.localRotation = reference.localRotation;
        target.localScale = reference.localScale;
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
    /// 切换游戏对象自身的激活状态。
    /// </summary>
    /// <param name="target">目标游戏对象。</param>
    /// <returns>切换后的 activeSelf；目标为 null 时返回 false。</returns>
    public static bool ExtToggleActive(this GameObject target)
    {
        if (target == null)
        {
            return false;
        }

        bool active = !target.activeSelf;
        target.SetActive(active);
        return active;
    }

    /// <summary>
    /// 切换组件所在游戏对象自身的激活状态。
    /// </summary>
    /// <param name="target">目标组件。</param>
    /// <returns>切换后的 activeSelf；目标为 null 时返回 false。</returns>
    public static bool ExtToggleActive(this Component target)
    {
        return target != null && target.gameObject.ExtToggleActive();
    }

    /// <summary>
    /// 设置全部直接子物体的激活状态。
    /// </summary>
    /// <param name="target">目标父节点。</param>
    /// <param name="active">是否激活直接子物体。</param>
    /// <returns>设置的直接子物体数量；目标为 null 时返回 0。</returns>
    public static int ExtSetChildrenActive(this Transform target, bool active)
    {
        if (target == null)
        {
            return 0;
        }

        int childCount = target.childCount;
        for (int i = 0; i < childCount; i++)
        {
            target.GetChild(i).gameObject.SetActive(active);
        }

        return childCount;
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
        return target.ExtGetOrAddComponent<T>(out _);
    }

    /// <summary>
    /// 获取游戏对象上的指定组件；不存在时自动添加，并报告是否执行了添加。
    /// </summary>
    /// <typeparam name="T">组件类型。</typeparam>
    /// <param name="target">目标游戏对象。</param>
    /// <param name="added">返回时指示组件是否由本次调用添加。</param>
    /// <returns>已存在或新添加的组件；目标为 null 时返回 null。</returns>
    public static T ExtGetOrAddComponent<T>(this GameObject target, out bool added)
        where T : Component
    {
        added = false;
        if (target == null)
        {
            return null;
        }

        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.AddComponent<T>();
            added = true;
        }

        return component;
    }

    /// <summary>
    /// 按运行时类型获取游戏对象上的组件；不存在时自动添加。
    /// </summary>
    /// <param name="target">目标游戏对象。</param>
    /// <param name="componentType">组件类型。</param>
    /// <returns>已存在或新添加的组件；目标为 null 时返回 null。</returns>
    public static Component ExtGetOrAddComponent(this GameObject target, Type componentType)
    {
        if (target == null)
        {
            return null;
        }

        ValidateComponentType(componentType, nameof(componentType));
        Component component = target.GetComponent(componentType);
        return component != null ? component : target.AddComponent(componentType);
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

    /// <summary>
    /// 获取组件所在游戏对象上的指定组件；不存在时自动添加，并报告是否执行了添加。
    /// </summary>
    /// <typeparam name="T">组件类型。</typeparam>
    /// <param name="target">目标组件。</param>
    /// <param name="added">返回时指示组件是否由本次调用添加。</param>
    /// <returns>已存在或新添加的组件；目标为 null 时返回 null。</returns>
    public static T ExtGetOrAddComponent<T>(this Component target, out bool added)
        where T : Component
    {
        if (target == null)
        {
            added = false;
            return null;
        }

        return target.gameObject.ExtGetOrAddComponent<T>(out added);
    }

    /// <summary>
    /// 按运行时类型获取组件所在游戏对象上的组件；不存在时自动添加。
    /// </summary>
    /// <param name="target">目标组件。</param>
    /// <param name="componentType">组件类型。</param>
    /// <returns>已存在或新添加的组件；目标为 null 时返回 null。</returns>
    public static Component ExtGetOrAddComponent(this Component target, Type componentType)
    {
        return target != null
            ? target.gameObject.ExtGetOrAddComponent(componentType)
            : null;
    }

    /// <summary>
    /// 获取游戏对象上的必需组件，缺失时抛出包含层级路径的异常。
    /// </summary>
    /// <typeparam name="T">必需组件类型。</typeparam>
    /// <param name="target">目标游戏对象。</param>
    /// <returns>目标上的组件。</returns>
    /// <exception cref="ArgumentNullException">目标为 null。</exception>
    /// <exception cref="InvalidOperationException">目标不包含指定组件。</exception>
    public static T ExtGetComponentOrThrow<T>(this GameObject target) where T : Component
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        T component = target.GetComponent<T>();
        if (component == null)
        {
            throw new InvalidOperationException(
                $"游戏对象 '{target.ExtGetHierarchyPath()}' 缺少组件 {typeof(T).FullName}。");
        }

        return component;
    }

    /// <summary>
    /// 获取组件所在游戏对象上的必需组件，缺失时抛出包含层级路径的异常。
    /// </summary>
    /// <typeparam name="T">必需组件类型。</typeparam>
    /// <param name="target">目标组件。</param>
    /// <returns>目标游戏对象上的组件。</returns>
    /// <exception cref="ArgumentNullException">目标为 null。</exception>
    /// <exception cref="InvalidOperationException">目标游戏对象不包含指定组件。</exception>
    public static T ExtGetComponentOrThrow<T>(this Component target) where T : Component
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        return target.gameObject.ExtGetComponentOrThrow<T>();
    }

    /// <summary>
    /// 尝试从游戏对象自身或子物体获取指定组件。
    /// </summary>
    /// <typeparam name="T">要查找的组件类型。</typeparam>
    /// <param name="target">搜索起点游戏对象。</param>
    /// <param name="component">找到时返回组件，否则返回 null。</param>
    /// <param name="includeInactive">是否搜索未激活的子物体。</param>
    /// <returns>找到指定组件时返回 true，否则返回 false。</returns>
    public static bool ExtTryGetComponentInChildren<T>(
        this GameObject target,
        out T component,
        bool includeInactive = false)
        where T : Component
    {
        component = target != null ? target.GetComponentInChildren<T>(includeInactive) : null;
        return component != null;
    }

    /// <summary>
    /// 尝试从组件所在游戏对象自身或子物体获取指定组件。
    /// </summary>
    /// <typeparam name="T">要查找的组件类型。</typeparam>
    /// <param name="target">提供搜索起点游戏对象的组件。</param>
    /// <param name="component">找到时返回组件，否则返回 null。</param>
    /// <param name="includeInactive">是否搜索未激活的子物体。</param>
    /// <returns>找到指定组件时返回 true，否则返回 false。</returns>
    public static bool ExtTryGetComponentInChildren<T>(
        this Component target,
        out T component,
        bool includeInactive = false)
        where T : Component
    {
        component = target != null
            ? target.gameObject.GetComponentInChildren<T>(includeInactive)
            : null;
        return component != null;
    }

    /// <summary>
    /// 尝试从游戏对象自身或父级获取指定组件。
    /// </summary>
    /// <typeparam name="T">要查找的组件类型。</typeparam>
    /// <param name="target">搜索起点游戏对象。</param>
    /// <param name="component">找到时返回组件，否则返回 null。</param>
    /// <param name="includeInactive">是否允许从未激活的父级对象获取组件。</param>
    /// <returns>找到指定组件时返回 true，否则返回 false。</returns>
    public static bool ExtTryGetComponentInParent<T>(
        this GameObject target,
        out T component,
        bool includeInactive = false)
        where T : Component
    {
        component = GetComponentInParentCompatible<T>(target, includeInactive);
        return component != null;
    }

    /// <summary>
    /// 尝试从组件所在游戏对象自身或父级获取指定组件。
    /// </summary>
    /// <typeparam name="T">要查找的组件类型。</typeparam>
    /// <param name="target">提供搜索起点游戏对象的组件。</param>
    /// <param name="component">找到时返回组件，否则返回 null。</param>
    /// <param name="includeInactive">是否允许从未激活的父级对象获取组件。</param>
    /// <returns>找到指定组件时返回 true，否则返回 false。</returns>
    public static bool ExtTryGetComponentInParent<T>(
        this Component target,
        out T component,
        bool includeInactive = false)
        where T : Component
    {
        component = GetComponentInParentCompatible<T>(
            target != null ? target.gameObject : null,
            includeInactive);
        return component != null;
    }

    /// <summary>
    /// 移除游戏对象上的指定组件。
    /// </summary>
    /// <typeparam name="T">要移除的组件类型。</typeparam>
    /// <param name="target">目标游戏对象。</param>
    /// <param name="removeAll">是否移除全部同类型组件；为 false 时只移除第一个。</param>
    /// <returns>运行模式下提交移除的组件数量；非运行模式或目标为 null 时返回 0。</returns>
    /// <remarks>为避免误改场景和 Prefab，非运行模式下不会执行任何删除操作。</remarks>
    public static int ExtRemoveComponent<T>(this GameObject target, bool removeAll = true)
        where T : Component
    {
        if (target == null || !Application.isPlaying)
        {
            return 0;
        }

        T[] components = target.GetComponents<T>();
        int removeCount = removeAll ? components.Length : Math.Min(components.Length, 1);
        for (int i = 0; i < removeCount; i++)
        {
            UnityEngine.Object.Destroy(components[i]);
        }

        return removeCount;
    }

    /// <summary>
    /// 移除组件所在游戏对象上的指定组件。
    /// </summary>
    /// <typeparam name="T">要移除的组件类型。</typeparam>
    /// <param name="target">目标组件。</param>
    /// <param name="removeAll">是否移除全部同类型组件；为 false 时只移除第一个。</param>
    /// <returns>运行模式下提交移除的组件数量；非运行模式或目标为 null 时返回 0。</returns>
    /// <remarks>为避免误改场景和 Prefab，非运行模式下不会执行任何删除操作。</remarks>
    public static int ExtRemoveComponent<T>(this Component target, bool removeAll = true)
        where T : Component
    {
        return target != null ? target.gameObject.ExtRemoveComponent<T>(removeAll) : 0;
    }

    /// <summary>
    /// 按传入顺序向游戏对象添加多个组件。
    /// </summary>
    /// <param name="target">目标游戏对象。</param>
    /// <param name="componentTypes">要添加的组件类型。</param>
    /// <returns>新添加的组件；目标为 null 或没有传入类型时返回空数组。</returns>
    public static Component[] ExtAddComponents(this GameObject target, params Type[] componentTypes)
    {
        if (target == null || componentTypes == null || componentTypes.Length == 0)
        {
            return Array.Empty<Component>();
        }

        for (int i = 0; i < componentTypes.Length; i++)
        {
            ValidateComponentType(componentTypes[i], nameof(componentTypes));
        }

        Component[] components = new Component[componentTypes.Length];
        for (int i = 0; i < componentTypes.Length; i++)
        {
            components[i] = target.AddComponent(componentTypes[i]);
        }

        return components;
    }

    /// <summary>
    /// 按传入顺序向组件所在的游戏对象添加多个组件。
    /// </summary>
    /// <param name="target">目标组件。</param>
    /// <param name="componentTypes">要添加的组件类型。</param>
    /// <returns>新添加的组件；目标为 null 或没有传入类型时返回空数组。</returns>
    public static Component[] ExtAddComponents(this Component target, params Type[] componentTypes)
    {
        return target != null
            ? target.gameObject.ExtAddComponents(componentTypes)
            : Array.Empty<Component>();
    }

    /// <summary>为目标和全部子物体递归设置 Layer。</summary>
    /// <param name="target">要设置 Layer 的根游戏对象。</param>
    /// <param name="layer">要应用的 Layer 索引，必须位于 0 到 31。</param>
    /// <exception cref="ArgumentOutOfRangeException">Layer 索引不在 0 到 31 范围内。</exception>
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
    /// <param name="target">搜索起点 Transform。</param>
    /// <param name="childName">要匹配的子节点名称，使用区分大小写的完整名称比较。</param>
    /// <param name="includeSelf">是否同时检查搜索起点自身。</param>
    /// <returns>第一个匹配的 Transform；没有找到或参数无效时返回 null。</returns>
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
    /// <param name="target">要生成层级路径的 Transform。</param>
    /// <returns>使用斜杠分隔的完整层级路径；目标为 null 时返回空字符串。</returns>
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
    /// <param name="target">要生成层级路径的游戏对象。</param>
    /// <returns>使用斜杠分隔的完整层级路径；目标为 null 时返回空字符串。</returns>
    public static string ExtGetHierarchyPath(this GameObject target)
    {
        return target != null ? target.transform.ExtGetHierarchyPath() : string.Empty;
    }

    /// <summary>清空全部直接子物体。</summary>
    /// <param name="target">要清空直接子物体的父节点。</param>
    /// <returns>运行模式下提交删除的子物体数量；非运行模式或目标为 null 时返回 0。</returns>
    /// <remarks>为避免误改场景和 Prefab，非运行模式下不会执行任何删除或层级变更。</remarks>
    public static int ExtClearChildren(this Transform target)
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

    /// <summary>设置 Transform 的世界坐标 X 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的世界坐标 X 值。</param>
    public static void ExtSetPositionX(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.position;
        position.x = value;
        target.position = position;
    }

    /// <summary>设置 Transform 的世界坐标 Y 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的世界坐标 Y 值。</param>
    public static void ExtSetPositionY(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.position;
        position.y = value;
        target.position = position;
    }

    /// <summary>设置 Transform 的世界坐标 Z 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的世界坐标 Z 值。</param>
    public static void ExtSetPositionZ(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.position;
        position.z = value;
        target.position = position;
    }

    /// <summary>设置 Transform 的本地坐标 X 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的本地坐标 X 值。</param>
    public static void ExtSetLocalPositionX(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.localPosition;
        position.x = value;
        target.localPosition = position;
    }

    /// <summary>设置 Transform 的本地坐标 Y 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的本地坐标 Y 值。</param>
    public static void ExtSetLocalPositionY(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.localPosition;
        position.y = value;
        target.localPosition = position;
    }

    /// <summary>设置 Transform 的本地坐标 Z 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的本地坐标 Z 值。</param>
    public static void ExtSetLocalPositionZ(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 position = target.localPosition;
        position.z = value;
        target.localPosition = position;
    }

    /// <summary>设置 Transform 的本地缩放 X 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的本地缩放 X 值。</param>
    public static void ExtSetLocalScaleX(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 scale = target.localScale;
        scale.x = value;
        target.localScale = scale;
    }

    /// <summary>设置 Transform 的本地缩放 Y 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的本地缩放 Y 值。</param>
    public static void ExtSetLocalScaleY(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 scale = target.localScale;
        scale.y = value;
        target.localScale = scale;
    }

    /// <summary>设置 Transform 的本地缩放 Z 分量。</summary>
    /// <param name="target">要修改的 Transform。</param>
    /// <param name="value">新的本地缩放 Z 值。</param>
    public static void ExtSetLocalScaleZ(this Transform target, float value)
    {
        if (target == null) return;
        Vector3 scale = target.localScale;
        scale.z = value;
        target.localScale = scale;
    }

    /// <summary>计算目标及其子物体全部 Renderer 的世界空间合并包围盒。</summary>
    /// <param name="target">要计算包围盒的层级根节点。</param>
    /// <param name="includeInactive">是否包含未激活子物体上的 Renderer。</param>
    /// <returns>合并后的世界空间包围盒；没有 Renderer 时返回以目标位置为中心的零尺寸包围盒。</returns>
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

    /// <summary>
    /// 添加或更新能够包裹目标及其全部子物体的 BoxCollider。
    /// </summary>
    /// <param name="target">承载 BoxCollider 和局部包围盒坐标系的目标游戏对象。</param>
    /// <param name="includeInactive">是否包含未激活子物体上的 Renderer、Collider 和 Transform。</param>
    /// <returns>
    /// 新添加或更新后的 BoxCollider；目标为 null，或者层级中没有可计算内容时返回 null。
    /// </returns>
    /// <remarks>
    /// 计算会包含目标及子物体上的 Renderer，并包含子物体上的 3D Collider；
    /// 目标自身已有的 Collider 不参与计算，现有 BoxCollider 会被复用且不会删除子碰撞体。
    /// </remarks>
    public static BoxCollider ExtAddFittedBoxCollider(
        this GameObject target,
        bool includeInactive = true)
    {
        if (target == null ||
            !TryCalculateHierarchyLocalBounds(target.transform, includeInactive, out Bounds localBounds))
        {
            return null;
        }

        BoxCollider boxCollider = target.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = target.AddComponent<BoxCollider>();
        }

        boxCollider.center = localBounds.center;
        boxCollider.size = ClampColliderSize(localBounds.size);
        return boxCollider;
    }

    /// <summary>
    /// 在组件所在游戏对象上添加或更新能够包裹其全部子物体的 BoxCollider。
    /// </summary>
    /// <param name="target">提供目标游戏对象的组件。</param>
    /// <param name="includeInactive">是否包含未激活子物体上的 Renderer、Collider 和 Transform。</param>
    /// <returns>
    /// 新添加或更新后的 BoxCollider；目标为 null，或者层级中没有可计算内容时返回 null。
    /// </returns>
    public static BoxCollider ExtAddFittedBoxCollider(
        this Component target,
        bool includeInactive = true)
    {
        return target != null
            ? target.gameObject.ExtAddFittedBoxCollider(includeInactive)
            : null;
    }

    /// <summary>
    /// 在运行模式下延迟销毁指定 Unity 对象。
    /// </summary>
    /// <param name="target">要销毁的 Unity 对象。</param>
    /// <returns>是否提交了销毁；非运行模式、目标为 null 或已经销毁时返回 false。</returns>
    /// <remarks>为避免误改场景、Prefab 或资源，非运行模式下不会执行任何删除操作。</remarks>
    public static bool ExtDestroy(this UnityEngine.Object target)
    {
        if (target == null || !Application.isPlaying)
        {
            return false;
        }

        UnityEngine.Object.Destroy(target);
        return true;
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

    /// <summary>使用兼容 Unity 2019.4 的方式从自身向父级查找组件。</summary>
    /// <typeparam name="T">要查找的组件类型。</typeparam>
    /// <param name="target">搜索起点游戏对象。</param>
    /// <param name="includeInactive">是否检查未激活层级中的游戏对象。</param>
    /// <returns>找到的第一个组件；目标为 null 或没有找到时返回 null。</returns>
    private static T GetComponentInParentCompatible<T>(
        GameObject target,
        bool includeInactive)
        where T : Component
    {
        Transform current = target != null ? target.transform : null;
        while (current != null)
        {
            if (includeInactive || current.gameObject.activeInHierarchy)
            {
                T component = current.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            current = current.parent;
        }

        return null;
    }

    /// <summary>验证类型是否可以作为 Unity 组件添加到游戏对象。</summary>
    /// <param name="componentType">要验证的组件类型。</param>
    /// <param name="parameterName">类型无效时写入异常的参数名称。</param>
    /// <exception cref="ArgumentException">类型不是有效的具体 Unity 组件类型。</exception>
    private static void ValidateComponentType(Type componentType, string parameterName)
    {
        if (componentType == null || !typeof(Component).IsAssignableFrom(componentType))
        {
            throw new ArgumentException("传入类型必须是 UnityEngine.Component。", parameterName);
        }

        if (componentType.IsAbstract || componentType.ContainsGenericParameters)
        {
            throw new ArgumentException(
                "不能添加抽象组件或包含未绑定泛型参数的组件。",
            parameterName);
        }
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
