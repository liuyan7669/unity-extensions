using UnityEngine;

namespace Cowart.CwExtensions
{
    /// <summary>
    /// 激活状态扩展：设置、切换游戏对象自身及其直接子物体的 activeSelf。
    /// </summary>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "ActiveStateExtensions")]
    public static class ActiveStateExtensions
    {
        /// <summary>设置组件所在物体的激活状态（activeSelf），省去 .gameObject。</summary>
        /// <param name="target">目标组件；为 null 时不执行任何操作。</param>
        /// <param name="active">是否激活。</param>
        /// <remarks>父物体未激活时，即使设为 true，物体仍不会显示（activeInHierarchy 为 false）。</remarks>
        public static void CwSetActive(this Component target, bool active)
        {
            if (target == null)
            {
                return;
            }

            target.gameObject.SetActive(active);
        }

        /// <summary>切换物体自身的激活状态：已激活则关闭，已关闭则激活。</summary>
        /// <param name="target">目标物体。</param>
        /// <returns>切换后的 activeSelf；target 为 null 时返回 false。</returns>
        public static bool CwToggleActive(this GameObject target)
        {
            if (target == null)
            {
                return false;
            }

            bool active = !target.activeSelf;
            target.SetActive(active);
            return active;
        }

        /// <summary>切换组件所在物体的激活状态：已激活则关闭，已关闭则激活。</summary>
        /// <param name="target">目标组件。</param>
        /// <returns>切换后的 activeSelf；target 为 null 时返回 false。</returns>
        public static bool CwToggleActive(this Component target)
        {
            return target != null && target.gameObject.CwToggleActive();
        }

        /// <summary>设置全部直接子物体的激活状态，target 自身和孙物体的 activeSelf 不变。</summary>
        /// <param name="target">父节点。</param>
        /// <param name="active">是否激活。</param>
        /// <returns>处理的直接子物体数量；target 为 null 时返回 0。</returns>
        public static int CwSetChildrenActive(this Transform target, bool active)
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
    }
}
