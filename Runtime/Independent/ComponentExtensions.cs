using System;
using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>
    /// 组件扩展：获取或添加组件、获取必需组件、从自身向子级或父级查找组件、批量添加组件和运行时安全移除组件。
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>获取物体上的 T 组件，没有时自动添加一个。</summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="target">目标物体。</param>
        /// <returns>已有或新添加的组件；target 为 null 时返回 null。存在多个 T 时返回第一个。</returns>
        public static T CwGetOrAddComponent<T>(this GameObject target) where T : Component
        {
            return target.CwGetOrAddComponent<T>(out _);
        }

        /// <summary>获取物体上的 T 组件，没有时自动添加，并通过 added 告知是否新添加，便于只对新组件做初始化。</summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="target">目标物体。</param>
        /// <param name="added">本次调用新添加了组件时为 true。</param>
        /// <returns>已有或新添加的组件；target 为 null 时返回 null。</returns>
        public static T CwGetOrAddComponent<T>(this GameObject target, out bool added)
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

        /// <summary>按运行时类型获取物体上的组件，没有时自动添加，适用于类型在运行时才确定的情况。</summary>
        /// <param name="target">目标物体。</param>
        /// <param name="componentType">组件类型，必须是非抽象的 Component 子类。</param>
        /// <returns>已有或新添加的组件；target 为 null 时返回 null。</returns>
        /// <exception cref="ArgumentException">componentType 为 null、不是 Component 子类，或是抽象类型。</exception>
        public static Component CwGetOrAddComponent(this GameObject target, Type componentType)
        {
            if (target == null)
            {
                return null;
            }

            ValidateComponentType(componentType, nameof(componentType));
            Component component = target.GetComponent(componentType);
            return component != null ? component : target.AddComponent(componentType);
        }

        /// <summary>获取组件所在物体上的 T 组件，没有时自动添加一个。</summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="target">目标物体上的任意组件。</param>
        /// <returns>已有或新添加的组件；target 为 null 时返回 null。</returns>
        public static T CwGetOrAddComponent<T>(this Component target) where T : Component
        {
            if (target == null)
            {
                return null;
            }

            return target.gameObject.CwGetOrAddComponent<T>();
        }

        /// <summary>获取组件所在物体上的 T 组件，没有时自动添加，并通过 added 告知是否新添加。</summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="target">目标物体上的任意组件。</param>
        /// <param name="added">本次调用新添加了组件时为 true。</param>
        /// <returns>已有或新添加的组件；target 为 null 时返回 null。</returns>
        public static T CwGetOrAddComponent<T>(this Component target, out bool added)
            where T : Component
        {
            if (target == null)
            {
                added = false;
                return null;
            }

            return target.gameObject.CwGetOrAddComponent<T>(out added);
        }

        /// <summary>按运行时类型获取组件所在物体上的组件，没有时自动添加。</summary>
        /// <param name="target">目标物体上的任意组件。</param>
        /// <param name="componentType">组件类型，必须是非抽象的 Component 子类。</param>
        /// <returns>已有或新添加的组件；target 为 null 时返回 null。</returns>
        /// <exception cref="ArgumentException">componentType 为 null、不是 Component 子类，或是抽象类型。</exception>
        public static Component CwGetOrAddComponent(this Component target, Type componentType)
        {
            return target != null
                ? target.gameObject.CwGetOrAddComponent(componentType)
                : null;
        }

        /// <summary>获取必需的 T 组件，缺失时立即抛出带层级路径的异常，便于在初始化时尽早发现配置错误。</summary>
        /// <typeparam name="T">必需的组件类型。</typeparam>
        /// <param name="target">目标物体。</param>
        /// <returns>物体上的 T 组件。</returns>
        /// <exception cref="ArgumentNullException">target 为 null。</exception>
        /// <exception cref="InvalidOperationException">物体上没有 T 组件。</exception>
        public static T CwGetComponentOrThrow<T>(this GameObject target) where T : Component
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            T component = target.GetComponent<T>();
            if (component == null)
            {
                throw new InvalidOperationException(
                    $"游戏对象 '{BuildHierarchyPath(target)}' 缺少组件 {typeof(T).FullName}。");
            }

            return component;
        }

        /// <summary>获取组件所在物体上必需的 T 组件，缺失时立即抛出带层级路径的异常。</summary>
        /// <typeparam name="T">必需的组件类型。</typeparam>
        /// <param name="target">目标物体上的任意组件。</param>
        /// <returns>物体上的 T 组件。</returns>
        /// <exception cref="ArgumentNullException">target 为 null。</exception>
        /// <exception cref="InvalidOperationException">物体上没有 T 组件。</exception>
        public static T CwGetComponentOrThrow<T>(this Component target) where T : Component
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return target.gameObject.CwGetComponentOrThrow<T>();
        }

        /// <summary>在 target 自身及全部子孙物体中深度优先查找第一个 T 组件。</summary>
        /// <typeparam name="T">要查找的组件类型。</typeparam>
        /// <param name="target">查找起点：目标物体；为 null 时找不到。</param>
        /// <param name="component">找到时为该组件，否则为 null。</param>
        /// <param name="includeInactive">为 false（默认）时只在激活的物体上查找；为 true 时包括未激活的物体。</param>
        /// <returns>找到时返回 true。</returns>
        public static bool CwTryGetComponentInChildren<T>(
            this GameObject target,
            out T component,
            bool includeInactive = false)
            where T : Component
        {
            component = target != null ? target.GetComponentInChildren<T>(includeInactive) : null;
            return component != null;
        }

        /// <summary>在 target 自身及全部子孙物体中深度优先查找第一个 T 组件。</summary>
        /// <typeparam name="T">要查找的组件类型。</typeparam>
        /// <param name="target">查找起点：目标物体上的任意组件；为 null 时找不到。</param>
        /// <param name="component">找到时为该组件，否则为 null。</param>
        /// <param name="includeInactive">为 false（默认）时只在激活的物体上查找；为 true 时包括未激活的物体。</param>
        /// <returns>找到时返回 true。</returns>
        public static bool CwTryGetComponentInChildren<T>(
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

        /// <summary>从 target 自身开始逐级向父物体查找第一个 T 组件。</summary>
        /// <typeparam name="T">要查找的组件类型。</typeparam>
        /// <param name="target">查找起点：目标物体；为 null 时找不到。</param>
        /// <param name="component">找到时为该组件，否则为 null。</param>
        /// <param name="includeInactive">为 false（默认）时跳过未激活（activeInHierarchy 为 false）的物体。</param>
        /// <returns>找到时返回 true。</returns>
        public static bool CwTryGetComponentInParent<T>(
            this GameObject target,
            out T component,
            bool includeInactive = false)
            where T : Component
        {
            component = GetComponentInParentCompatible<T>(target, includeInactive);
            return component != null;
        }

        /// <summary>从 target 自身开始逐级向父物体查找第一个 T 组件。</summary>
        /// <typeparam name="T">要查找的组件类型。</typeparam>
        /// <param name="target">查找起点：目标物体上的任意组件；为 null 时找不到。</param>
        /// <param name="component">找到时为该组件，否则为 null。</param>
        /// <param name="includeInactive">为 false（默认）时跳过未激活（activeInHierarchy 为 false）的物体。</param>
        /// <returns>找到时返回 true。</returns>
        public static bool CwTryGetComponentInParent<T>(
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

        /// <summary>在运行模式下移除物体上的 T 组件。</summary>
        /// <typeparam name="T">要移除的组件类型。</typeparam>
        /// <param name="target">目标物体。</param>
        /// <param name="removeAll">为 true（默认）时移除全部 T 组件；为 false 时只移除第一个。</param>
        /// <returns>提交移除的组件数量；编辑模式下或 target 为 null 时返回 0。</returns>
        /// <remarks>只在运行模式下执行，编辑模式下不做任何修改。移除在帧末生效，本帧内 GetComponent 仍可能取到它；被其他组件 RequireComponent 依赖的组件无法移除。</remarks>
        public static int CwRemoveComponent<T>(this GameObject target, bool removeAll = true)
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

        /// <summary>在运行模式下移除物体上的 T 组件。</summary>
        /// <typeparam name="T">要移除的组件类型。</typeparam>
        /// <param name="target">目标物体上的任意组件。</param>
        /// <param name="removeAll">为 true（默认）时移除全部 T 组件；为 false 时只移除第一个。</param>
        /// <returns>提交移除的组件数量；编辑模式下或 target 为 null 时返回 0。</returns>
        /// <remarks>只在运行模式下执行，编辑模式下不做任何修改。移除在帧末生效，本帧内 GetComponent 仍可能取到它；被其他组件 RequireComponent 依赖的组件无法移除。</remarks>
        public static int CwRemoveComponent<T>(this Component target, bool removeAll = true)
            where T : Component
        {
            return target != null ? target.gameObject.CwRemoveComponent<T>(removeAll) : 0;
        }

        /// <summary>按顺序一次添加多个组件。</summary>
        /// <param name="target">目标物体。</param>
        /// <param name="componentTypes">要添加的组件类型，必须都是非抽象的 Component 子类；任一类型无效时一个都不会添加。</param>
        /// <returns>新添加的组件，顺序与 componentTypes 一致；target 为 null 或未传类型时返回空数组。</returns>
        /// <exception cref="ArgumentException">componentTypes 中存在 null、非 Component 或抽象类型。</exception>
        /// <remarks>不检查是否已有同类组件，会重复添加；需要避免重复时请使用 CwGetOrAddComponent。</remarks>
        public static Component[] CwAddComponents(this GameObject target, params Type[] componentTypes)
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

        /// <summary>按顺序一次添加多个组件。</summary>
        /// <param name="target">目标物体上的任意组件。</param>
        /// <param name="componentTypes">要添加的组件类型，必须都是非抽象的 Component 子类；任一类型无效时一个都不会添加。</param>
        /// <returns>新添加的组件，顺序与 componentTypes 一致；target 为 null 或未传类型时返回空数组。</returns>
        /// <exception cref="ArgumentException">componentTypes 中存在 null、非 Component 或抽象类型。</exception>
        /// <remarks>不检查是否已有同类组件，会重复添加；需要避免重复时请使用 CwGetOrAddComponent。</remarks>
        public static Component[] CwAddComponents(this Component target, params Type[] componentTypes)
        {
            return target != null
                ? target.gameObject.CwAddComponents(componentTypes)
                : Array.Empty<Component>();
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

        /// <summary>生成游戏对象从场景根节点开始的层级路径，仅用于异常信息。</summary>
        /// <param name="target">要生成路径的游戏对象。</param>
        /// <returns>使用斜杠分隔的层级路径。</returns>
        private static string BuildHierarchyPath(GameObject target)
        {
            string path = target.name;
            for (Transform parent = target.transform.parent; parent != null; parent = parent.parent)
            {
                path = parent.name + "/" + path;
            }

            return path;
        }
    }
}
