using System;
using UnityEngine;

namespace Cowart.CwExtensions
{
    /// <summary>为场景物体提供点击回调扩展。</summary>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "GameObjectClickExtensions")]
    public static class GameObjectClickExtensions
    {
        /// <summary>设置物体被鼠标左键点击（或触摸点按）时执行的回调，适用于没有 Button 的 3D 物体或 UI 物体。</summary>
        /// <param name="target">目标物体；为 null 时不执行任何操作。</param>
        /// <param name="onClicked">每次点击执行的回调；重复调用会替换上一次的回调，传入 null 时停止监听。</param>
        /// <remarks>首次设置时会在物体上添加隐藏的 CwClickAction 组件。点击由 EventSystem 检测：场景需要 EventSystem；3D 物体需要 Collider 且相机带 PhysicsRaycaster；UI 物体需要 Canvas 上的 GraphicRaycaster。被其他物体遮挡时不会触发。</remarks>
        public static void CwSetClickAction(this GameObject target, Action onClicked)
        {
            if (target == null)
            {
                return;
            }

            if (!target.TryGetComponent(out CwClickAction click))
            {
                if (onClicked == null)
                {
                    return;
                }

                click = target.AddComponent<CwClickAction>();
            }

            click.Callback = onClicked;
            click.enabled = onClicked != null;
        }
    }
}
