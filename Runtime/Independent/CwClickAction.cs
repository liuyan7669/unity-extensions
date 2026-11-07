using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cowart.Extensions
{
    /// <summary>将物体的左键点击转发给当前回调。由 <see cref="GameObjectClickExtensions"/> 自动添加和管理。</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    internal sealed class CwClickAction : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>每次点击时执行的回调。</summary>
        internal Action Callback { get; set; }

        /// <summary>响应左键点击。</summary>
        /// <param name="eventData">指针事件数据。</param>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Callback?.Invoke();
            }
        }
    }
}
