using UnityEngine;

/// <summary>
/// 提供 GameObject、Component 和 Transform 的通用扩展操作。
/// </summary>
public static class UnityObjectExtensions
{
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
}
