using System;
#if COWART_TMP
using TMPro;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cowart.Extensions
{
    /// <summary>
    /// UI 事件扩展：用 System.Action 设置 UGUI 组件、EventTrigger 和 TMP 组件（项目包含 TextMesh Pro 时）的事件监听。
    /// </summary>
    /// <remarks>
    /// 所有方法遵循同一规则：
    /// <list type="bullet">
    /// <item>removeAllListeners 为 true（默认）时，先移除该事件上所有通过代码注册的监听，包括其他脚本注册的，再添加回调；为 false 时追加。</item>
    /// <item>Inspector 中配置的持久化监听始终保留。</item>
    /// <item>回调会被包装后注册，不能用 RemoveListener 单独移除；要取消时传入 null 回调（并保持 removeAllListeners 为 true）。</item>
    /// <item>目标为 null 时不执行任何操作。</item>
    /// </list>
    /// </remarks>
    public static class UIEventExtensions
    {
        /// <summary>设置 Button 的点击事件 onClick。</summary>
        /// <param name="button">目标 Button。</param>
        /// <param name="callback">点击时执行的回调；为 null 时只清理监听、不添加。</param>
        /// <param name="removeAllListeners">true：先清空 onClick 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>Button 不可交互（interactable 为 false）时不会触发。</remarks>
        public static void CwSetOnClick(
            this Button button,
            Action callback,
            bool removeAllListeners = true)
        {
            if (button == null)
            {
                return;
            }

            SetListener(button.onClick, callback, removeAllListeners);
        }

        /// <summary>设置 Toggle 的开关变化事件 onValueChanged。</summary>
        /// <param name="toggle">目标 Toggle。</param>
        /// <param name="callback">开关变化时执行的回调，参数为新的 isOn；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onValueChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>代码修改 isOn 也会触发；不想触发时请使用 Toggle.SetIsOnWithoutNotify。</remarks>
        public static void CwSetOnValueChanged(
            this Toggle toggle,
            Action<bool> callback,
            bool removeAllListeners = true)
        {
            if (toggle == null)
            {
                return;
            }

            SetListener(toggle.onValueChanged, callback, removeAllListeners);
        }

        /// <summary>设置 Slider 的数值变化事件 onValueChanged。</summary>
        /// <param name="slider">目标 Slider。</param>
        /// <param name="callback">数值变化时执行的回调，参数为新的 value；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onValueChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>代码修改 value 也会触发；不想触发时请使用 Slider.SetValueWithoutNotify。</remarks>
        public static void CwSetOnValueChanged(
            this Slider slider,
            Action<float> callback,
            bool removeAllListeners = true)
        {
            if (slider == null)
            {
                return;
            }

            SetListener(slider.onValueChanged, callback, removeAllListeners);
        }

        /// <summary>设置 Scrollbar 的数值变化事件 onValueChanged。</summary>
        /// <param name="scrollbar">目标 Scrollbar。</param>
        /// <param name="callback">数值变化时执行的回调，参数为新的 value（0 到 1）；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onValueChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>代码修改 value，或关联的 ScrollRect 滚动时也会触发。</remarks>
        public static void CwSetOnValueChanged(
            this Scrollbar scrollbar,
            Action<float> callback,
            bool removeAllListeners = true)
        {
            if (scrollbar == null)
            {
                return;
            }

            SetListener(scrollbar.onValueChanged, callback, removeAllListeners);
        }

        /// <summary>设置 Dropdown 的选项变化事件 onValueChanged。</summary>
        /// <param name="dropdown">目标 Dropdown。</param>
        /// <param name="callback">选项变化时执行的回调，参数为新选项的索引；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onValueChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>代码修改 value 也会触发；不想触发时请使用 Dropdown.SetValueWithoutNotify。</remarks>
        public static void CwSetOnValueChanged(
            this Dropdown dropdown,
            Action<int> callback,
            bool removeAllListeners = true)
        {
            if (dropdown == null)
            {
                return;
            }

            SetListener(dropdown.onValueChanged, callback, removeAllListeners);
        }

        /// <summary>设置 ScrollRect 的滚动位置变化事件 onValueChanged。</summary>
        /// <param name="scrollRect">目标 ScrollRect。</param>
        /// <param name="callback">滚动时执行的回调，参数为 normalizedPosition（各轴 0 到 1）；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onValueChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>拖动、惯性滑动、回弹和代码修改位置都会触发，滚动期间可能每帧触发，回调中避免高开销操作。</remarks>
        public static void CwSetOnValueChanged(
            this ScrollRect scrollRect,
            Action<Vector2> callback,
            bool removeAllListeners = true)
        {
            if (scrollRect == null)
            {
                return;
            }

            SetListener(scrollRect.onValueChanged, callback, removeAllListeners);
        }

        /// <summary>设置 InputField 的文本变化事件 onValueChanged。</summary>
        /// <param name="inputField">目标 InputField。</param>
        /// <param name="callback">文本每次变化时执行的回调，参数为当前文本；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onValueChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>每输入一个字符都会触发；代码修改 text 也会触发。</remarks>
        public static void CwSetOnValueChanged(
            this InputField inputField,
            Action<string> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onValueChanged, callback, removeAllListeners);
        }

        /// <summary>设置 InputField 的编辑结束事件 onEndEdit。</summary>
        /// <param name="inputField">目标 InputField。</param>
        /// <param name="callback">编辑结束时执行的回调，参数为最终文本；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onEndEdit 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>按回车提交和失去焦点都会触发；只想响应回车提交时请使用 CwSetOnSubmit。</remarks>
        public static void CwSetOnEndEdit(
            this InputField inputField,
            Action<string> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onEndEdit, callback, removeAllListeners);
        }

        /// <summary>设置 InputField 的提交事件 onSubmit（按回车提交时触发）。</summary>
        /// <param name="inputField">目标 InputField。</param>
        /// <param name="callback">提交时执行的回调，参数为提交的文本；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空对应事件上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>Unity 2021 之前的 UGUI 没有 onSubmit，此时改为绑定 onEndEdit，失去焦点也会触发。</remarks>
        public static void CwSetOnSubmit(
            this InputField inputField,
            Action<string> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

#if UNITY_2021_1_OR_NEWER
            SetListener(inputField.onSubmit, callback, removeAllListeners);
#else
            SetListener(inputField.onEndEdit, callback, removeAllListeners);
#endif
        }

        /// <summary>设置 MaskableGraphic（Image、Text 等）的裁剪状态变化事件 onCullStateChanged。</summary>
        /// <param name="graphic">目标 MaskableGraphic。</param>
        /// <param name="callback">裁剪状态变化时执行的回调，参数为 true 表示已被裁剪（不可见）；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onCullStateChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>只在父级存在 RectMask2D 时触发，常用于滚动列表中判断元素是否移出可视区域。</remarks>
        public static void CwSetOnCullStateChanged(
            this MaskableGraphic graphic,
            Action<bool> callback,
            bool removeAllListeners = true)
        {
            if (graphic == null)
            {
                return;
            }

            SetListener(graphic.onCullStateChanged, callback, removeAllListeners);
        }

        /// <summary>设置 EventTrigger 上指定类型事件的监听；该类型条目不存在时自动创建。</summary>
        /// <param name="eventTrigger">目标 EventTrigger；需要已挂在物体上。</param>
        /// <param name="eventType">要设置的事件类型，例如 PointerEnter、PointerClick。</param>
        /// <param name="callback">事件触发时执行的回调，参数为事件数据，可按需转换为 PointerEventData；为 null 时只清理监听，且不会新建条目。</param>
        /// <param name="removeAllListeners">true：先清空该类型条目上代码注册的全部监听再添加；false：追加。其他事件类型不受影响。</param>
        /// <remarks>EventTrigger 会拦截它支持的全部指针事件，可能阻止事件继续传给父级的 ScrollRect 等组件。</remarks>
        public static void CwSetEventTrigger(
            this EventTrigger eventTrigger,
            EventTriggerType eventType,
            Action<BaseEventData> callback,
            bool removeAllListeners = true)
        {
            if (eventTrigger == null)
            {
                return;
            }

            EventTrigger.Entry entry = eventTrigger.triggers.Find(
                item => item.eventID == eventType);

            if (entry == null)
            {
                if (callback == null)
                {
                    return;
                }

                entry = new EventTrigger.Entry
                {
                    eventID = eventType
                };
                eventTrigger.triggers.Add(entry);
            }

            if (entry.callback == null)
            {
                entry.callback = new EventTrigger.TriggerEvent();
            }

            SetListener(entry.callback, callback, removeAllListeners);
        }

        /// <summary>按规则设置无参数 UnityEvent 的监听。</summary>
        /// <param name="targetEvent">要设置的事件。</param>
        /// <param name="callback">要添加的回调；为 null 时不添加。</param>
        /// <param name="removeAllListeners">是否先移除代码注册的全部监听。</param>
        private static void SetListener(
            UnityEvent targetEvent,
            Action callback,
            bool removeAllListeners)
        {
            if (targetEvent == null)
            {
                return;
            }

            if (removeAllListeners)
            {
                targetEvent.RemoveAllListeners();
            }

            if (callback != null)
            {
                targetEvent.AddListener(() => callback());
            }
        }

        /// <summary>按规则设置单参数 UnityEvent 的监听。</summary>
        /// <typeparam name="T">事件参数类型。</typeparam>
        /// <param name="targetEvent">要设置的事件。</param>
        /// <param name="callback">要添加的回调；为 null 时不添加。</param>
        /// <param name="removeAllListeners">是否先移除代码注册的全部监听。</param>
        private static void SetListener<T>(
            UnityEvent<T> targetEvent,
            Action<T> callback,
            bool removeAllListeners)
        {
            if (targetEvent == null)
            {
                return;
            }

            if (removeAllListeners)
            {
                targetEvent.RemoveAllListeners();
            }

            if (callback != null)
            {
                targetEvent.AddListener(value => callback(value));
            }
        }

#if COWART_TMP
        /// <summary>设置 TMP_Dropdown 的选项变化事件 onValueChanged。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="dropdown">目标 TMP_Dropdown。</param>
        /// <param name="callback">选项变化时执行的回调，参数为新选项的索引；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onValueChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>代码修改 value 也会触发；不想触发时请使用 TMP_Dropdown.SetValueWithoutNotify。</remarks>
        public static void CwSetOnValueChanged(
            this TMP_Dropdown dropdown,
            Action<int> callback,
            bool removeAllListeners = true)
        {
            if (dropdown == null)
            {
                return;
            }

            SetListener(dropdown.onValueChanged, callback, removeAllListeners);
        }

        /// <summary>设置 TMP_InputField 的文本变化事件 onValueChanged。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="inputField">目标 TMP_InputField。</param>
        /// <param name="callback">文本每次变化时执行的回调，参数为当前文本；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onValueChanged 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>每输入一个字符都会触发；代码修改 text 也会触发，不想触发时请使用 SetTextWithoutNotify。</remarks>
        public static void CwSetOnValueChanged(
            this TMP_InputField inputField,
            Action<string> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onValueChanged, callback, removeAllListeners);
        }

        /// <summary>设置 TMP_InputField 的编辑结束事件 onEndEdit。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="inputField">目标 TMP_InputField。</param>
        /// <param name="callback">编辑结束时执行的回调，参数为最终文本；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onEndEdit 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>按回车提交和失去焦点都会触发；只想响应回车提交时请使用 CwSetOnSubmit。</remarks>
        public static void CwSetOnEndEdit(
            this TMP_InputField inputField,
            Action<string> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onEndEdit, callback, removeAllListeners);
        }

        /// <summary>设置 TMP_InputField 的提交事件 onSubmit（按回车提交时触发）。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="inputField">目标 TMP_InputField。</param>
        /// <param name="callback">提交时执行的回调，参数为提交的文本；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onSubmit 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>lineType 为 MultiLineNewline 时回车用于换行，不会触发提交。</remarks>
        public static void CwSetOnSubmit(
            this TMP_InputField inputField,
            Action<string> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onSubmit, callback, removeAllListeners);
        }

        /// <summary>设置 TMP_InputField 的获得焦点事件 onSelect。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="inputField">目标 TMP_InputField。</param>
        /// <param name="callback">获得焦点时执行的回调，参数为当前文本；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onSelect 上代码注册的全部监听再添加；false：追加。</param>
        public static void CwSetOnSelect(
            this TMP_InputField inputField,
            Action<string> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onSelect, callback, removeAllListeners);
        }

        /// <summary>设置 TMP_InputField 的失去焦点事件 onDeselect。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="inputField">目标 TMP_InputField。</param>
        /// <param name="callback">失去焦点时执行的回调，参数为当前文本；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onDeselect 上代码注册的全部监听再添加；false：追加。</param>
        public static void CwSetOnDeselect(
            this TMP_InputField inputField,
            Action<string> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onDeselect, callback, removeAllListeners);
        }

        /// <summary>设置 TMP_InputField 的选区变化事件 onTextSelection。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="inputField">目标 TMP_InputField。</param>
        /// <param name="callback">选区变化时执行的回调，参数依次为当前文本和选区两端的字符索引（不保证前小后大）；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onTextSelection 上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>拖选过程中会连续触发；只关心最终选区时请使用 CwSetOnEndTextSelection。</remarks>
        public static void CwSetOnTextSelection(
            this TMP_InputField inputField,
            Action<string, int, int> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onTextSelection, callback, removeAllListeners);
        }

        /// <summary>设置 TMP_InputField 的选区结束事件 onEndTextSelection。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="inputField">目标 TMP_InputField。</param>
        /// <param name="callback">选区取消时执行的回调，参数依次为当前文本和选区两端的字符索引；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空 onEndTextSelection 上代码注册的全部监听再添加；false：追加。</param>
        public static void CwSetOnEndTextSelection(
            this TMP_InputField inputField,
            Action<string, int, int> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(inputField.onEndTextSelection, callback, removeAllListeners);
        }

        /// <summary>设置 TMP_InputField 的触屏键盘状态变化事件。仅在项目包含 TextMesh Pro 时可用。</summary>
        /// <param name="inputField">目标 TMP_InputField。</param>
        /// <param name="callback">触屏键盘状态变化时执行的回调，参数为新状态（Visible、Done、Canceled、LostFocus）；为 null 时只清理监听。</param>
        /// <param name="removeAllListeners">true：先清空该事件上代码注册的全部监听再添加；false：追加。</param>
        /// <remarks>只在 Android、iOS 等使用系统触屏键盘的平台触发，PC 和编辑器中不会触发。</remarks>
        public static void CwSetOnTouchScreenKeyboardStatusChanged(
            this TMP_InputField inputField,
            Action<TouchScreenKeyboard.Status> callback,
            bool removeAllListeners = true)
        {
            if (inputField == null)
            {
                return;
            }

            SetListener(
                inputField.onTouchScreenKeyboardStatusChanged,
                callback,
                removeAllListeners);
        }

        /// <summary>按规则设置三参数 UnityEvent 的监听。</summary>
        /// <typeparam name="T0">第一个事件参数类型。</typeparam>
        /// <typeparam name="T1">第二个事件参数类型。</typeparam>
        /// <typeparam name="T2">第三个事件参数类型。</typeparam>
        /// <param name="targetEvent">要设置的事件。</param>
        /// <param name="callback">要添加的回调；为 null 时不添加。</param>
        /// <param name="removeAllListeners">是否先移除代码注册的全部监听。</param>
        private static void SetListener<T0, T1, T2>(
            UnityEvent<T0, T1, T2> targetEvent,
            Action<T0, T1, T2> callback,
            bool removeAllListeners)
        {
            if (targetEvent == null)
            {
                return;
            }

            if (removeAllListeners)
            {
                targetEvent.RemoveAllListeners();
            }

            if (callback != null)
            {
                targetEvent.AddListener(
                    (value0, value1, value2) => callback(value0, value1, value2));
            }
        }
#endif
    }
}
