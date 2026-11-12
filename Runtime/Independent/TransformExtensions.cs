using UnityEngine;

namespace Cowart.CwExtensions
{
    /// <summary>Transform 扩展：单个 Transform 的位置、旋转、缩放的重置、单轴赋值、复制和水平朝向，不涉及父子层级。</summary>
    /// <remarks>全部方法直接修改 Transform，目标为 null 时不执行任何操作。带 Rigidbody 的物体需要物理移动时，请改用 Rigidbody.MovePosition / MoveRotation。</remarks>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "TransformExtensions")]
    public static class TransformExtensions
    {
        /// <summary>把本地位置设为 (0, 0, 0)，使物体与父节点原点重合；没有父节点时即世界原点。</summary>
        /// <param name="target">目标 Transform。</param>
        public static void CwResetLocalPosition(this Transform target)
        {
            if (target == null)
            {
                return;
            }

            target.localPosition = Vector3.zero;
        }

        /// <summary>把本地旋转设为无旋转（Quaternion.identity），使朝向与父节点一致。</summary>
        /// <param name="target">目标 Transform。</param>
        public static void CwResetLocalRotation(this Transform target)
        {
            if (target == null)
            {
                return;
            }

            target.localRotation = Quaternion.identity;
        }

        /// <summary>把本地缩放设为 (1, 1, 1)。</summary>
        /// <param name="target">目标 Transform。</param>
        public static void CwResetLocalScale(this Transform target)
        {
            if (target == null)
            {
                return;
            }

            target.localScale = Vector3.one;
        }

        /// <summary>把本地位置、旋转、缩放全部重置为默认值（0、无旋转、1），常用于把物体对齐到父节点。</summary>
        /// <param name="target">目标 Transform。</param>
        public static void CwResetLocal(this Transform target)
        {
            if (target == null)
            {
                return;
            }

            target.localPosition = Vector3.zero;
            target.localRotation = Quaternion.identity;
            target.localScale = Vector3.one;
        }

        /// <summary>把 reference 的本地位置、旋转、缩放数值复制给 target。</summary>
        /// <param name="target">要修改的 Transform。</param>
        /// <param name="reference">提供数值的参考对象；为 null 时不执行任何操作。</param>
        /// <remarks>复制的是相对各自父节点的数值；两者父节点不同时世界位置不会相同，需要世界位置一致时请使用 CwCopyPoseFrom。</remarks>
        public static void CwCopyLocalTransformFrom(this Transform target, Transform reference)
        {
            if (target == null || reference == null)
            {
                return;
            }

            target.localPosition = reference.localPosition;
            target.localRotation = reference.localRotation;
            target.localScale = reference.localScale;
        }

        /// <summary>把 target 移动并旋转到 reference 的世界位置和世界旋转，缩放不变。</summary>
        /// <param name="target">要移动的 Transform。</param>
        /// <param name="reference">提供世界位置和旋转的参考对象；为 null 时不执行任何操作。</param>
        public static void CwCopyPoseFrom(this Transform target, Transform reference)
        {
            if (target == null || reference == null)
            {
                return;
            }

            target.SetPositionAndRotation(reference.position, reference.rotation);
        }

        /// <summary>把 target 移到 reference 的世界位置，旋转和缩放不变。</summary>
        /// <param name="target">要移动的 Transform。</param>
        /// <param name="reference">提供世界位置的参考对象；为 null 时不执行任何操作。</param>
        public static void CwCopyPositionFrom(this Transform target, Transform reference)
        {
            if (target == null || reference == null)
            {
                return;
            }

            target.position = reference.position;
        }

        /// <summary>把 target 的世界旋转设为与 reference 相同，位置和缩放不变。</summary>
        /// <param name="target">要旋转的 Transform。</param>
        /// <param name="reference">提供世界旋转的参考对象；为 null 时不执行任何操作。</param>
        public static void CwCopyRotationFrom(this Transform target, Transform reference)
        {
            if (target == null || reference == null)
            {
                return;
            }

            target.eulerAngles = reference.eulerAngles;
        }

        /// <summary>让 target 在水平面上朝向 reference（只绕世界 Y 轴转动，不抬头低头），常用于角色或 NPC 转身面向目标。</summary>
        /// <param name="target">要转向的 Transform。</param>
        /// <param name="reference">朝向的目标；为 null 时不执行任何操作。</param>
        /// <remarks>target 的 X、Z 轴旋转会被清零。两者水平位置重合时无法确定朝向，调用前请自行排除。</remarks>
        public static void CwLookAtHorizontal(this Transform target, Transform reference)
        {
            if (target == null || reference == null)
            {
                return;
            }

            Vector3 lookPosition = reference.position;
            lookPosition.y = target.position.y;
            target.LookAt(lookPosition);
        }

        /// <summary>只修改世界坐标的 X，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的世界坐标 X。</param>
        public static void CwSetPositionX(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 position = target.position;
            position.x = value;
            target.position = position;
        }

        /// <summary>只修改世界坐标的 Y，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的世界坐标 Y。</param>
        public static void CwSetPositionY(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 position = target.position;
            position.y = value;
            target.position = position;
        }

        /// <summary>只修改世界坐标的 Z，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的世界坐标 Z。</param>
        public static void CwSetPositionZ(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 position = target.position;
            position.z = value;
            target.position = position;
        }

        /// <summary>只修改本地坐标（相对父节点）的 X，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的本地坐标 X。</param>
        public static void CwSetLocalPositionX(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 position = target.localPosition;
            position.x = value;
            target.localPosition = position;
        }

        /// <summary>只修改本地坐标（相对父节点）的 Y，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的本地坐标 Y。</param>
        public static void CwSetLocalPositionY(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 position = target.localPosition;
            position.y = value;
            target.localPosition = position;
        }

        /// <summary>只修改本地坐标（相对父节点）的 Z，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的本地坐标 Z。</param>
        public static void CwSetLocalPositionZ(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 position = target.localPosition;
            position.z = value;
            target.localPosition = position;
        }

        /// <summary>只修改本地缩放的 X，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的本地缩放 X。</param>
        public static void CwSetLocalScaleX(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 scale = target.localScale;
            scale.x = value;
            target.localScale = scale;
        }

        /// <summary>只修改本地缩放的 Y，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的本地缩放 Y。</param>
        public static void CwSetLocalScaleY(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 scale = target.localScale;
            scale.y = value;
            target.localScale = scale;
        }

        /// <summary>只修改本地缩放的 Z，其余分量不变。</summary>
        /// <param name="target">目标 Transform。</param>
        /// <param name="value">新的本地缩放 Z。</param>
        public static void CwSetLocalScaleZ(this Transform target, float value)
        {
            if (target == null) return;
            Vector3 scale = target.localScale;
            scale.z = value;
            target.localScale = scale;
        }
    }
}
