using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>
    /// 存放由多个独立扩展组合而成的 Transform 扩展方法；只依赖 Unity 内置模块。
    /// </summary>
    public static class TransformCompositeExtensions
    {
        /// <summary>把 target 设为 parent 的子物体，并把本地位置、旋转、缩放重置为默认值，常用于把生成的物体挂到挂点上。</summary>
        /// <param name="target">要挂载的物体；为 null 时不执行任何操作。</param>
        /// <param name="parent">新的父节点；为 null 时移到场景根节点。</param>
        /// <remarks>组合 SetParent 与 CwResetLocal。RectTransform 的 anchoredPosition 和 sizeDelta 不会被重置。</remarks>
        public static void CwSetParentAndResetLocal(this Transform target, Transform parent)
        {
            if (target == null)
            {
                return;
            }

            target.SetParent(parent, false);
            target.CwResetLocal();
        }
    }
}
