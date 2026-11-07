using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cowart.Extensions
{
    /// <summary>
    /// 集合扩展：判空、安全索引、日志输出、随机取值、原地打乱、随机弹出、按权重随机和清理已销毁对象。
    /// </summary>
    /// <remarks>随机相关方法使用 UnityEngine.Random，只能在主线程调用，结果受 Random.InitState 影响。</remarks>
    public static class CollectionExtensions
    {
        /// <summary>判断集合是否为 null 或不包含任何元素。</summary>
        /// <typeparam name="T">集合元素类型。</typeparam>
        /// <param name="items">要检查的集合，可传入 List、数组、HashSet 等。</param>
        /// <returns>集合为 null 或不包含元素时返回 true，否则返回 false。</returns>
        public static bool CwIsNullOrEmpty<T>(this IReadOnlyCollection<T> items)
        {
            return items == null || items.Count == 0;
        }

        /// <summary>按索引安全读取元素，列表为 null 或索引越界时不抛异常，返回 defaultValue。</summary>
        /// <typeparam name="T">列表元素类型。</typeparam>
        /// <param name="items">要读取的列表或数组。</param>
        /// <param name="index">从 0 开始的索引。</param>
        /// <param name="defaultValue">读取失败时返回的值，默认为类型默认值。</param>
        /// <returns>索引处的元素；读取失败时返回 defaultValue。</returns>
        public static T CwGetOrDefault<T>(
            this IReadOnlyList<T> items,
            int index,
            T defaultValue = default)
        {
            return items != null && index >= 0 && index < items.Count
                ? items[index]
                : defaultValue;
        }

        /// <summary>用一条 Debug.Log 输出列表的元素数量，以及每个元素的索引和 ToString() 内容，便于调试。</summary>
        /// <typeparam name="T">列表元素类型。</typeparam>
        /// <param name="items">要输出的列表或数组；为 null 时输出一条警告。</param>
        /// <param name="context">日志关联的对象，在 Console 中点击日志可定位到它；可为 null。</param>
        /// <remarks>元素很多时日志会很长，且会为每个元素生成字符串，避免在每帧调用。</remarks>
        public static void CwLogItems<T>(this IReadOnlyList<T> items, UnityEngine.Object context = null)
        {
            if (items == null)
            {
                Debug.LogWarning("集合为 null。", context);
                return;
            }

            if (items.Count == 0)
            {
                Debug.Log("集合元素数量: 0", context);
                return;
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"集合元素数量: {items.Count}");
            for (int i = 0; i < items.Count; i++)
            {
                T item = items[i];
                string content = item == null ? "null" : item.ToString();
                builder.AppendLine($"[{i}] {content}");
            }

            Debug.Log(builder.ToString(), context);
        }

        /// <summary>等概率随机返回列表中的一个元素，不修改列表。</summary>
        /// <typeparam name="T">列表元素类型。</typeparam>
        /// <param name="items">要取值的列表或数组。</param>
        /// <returns>随机元素；列表为 null 或为空时输出警告并返回类型默认值。</returns>
        /// <remarks>返回默认值无法区分“列表为空”和“取到的元素本身是默认值”，需要区分时请先用 CwIsNullOrEmpty 判断。</remarks>
        public static T CwGetRandomItem<T>(this IReadOnlyList<T> items)
        {
            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("集合为 null 或不包含任何元素。");
                return default;
            }

            return items[Random.Range(0, items.Count)];
        }

        /// <summary>等概率随机返回列表的一个有效索引。</summary>
        /// <typeparam name="T">列表元素类型。</typeparam>
        /// <param name="items">要取索引的列表或数组。</param>
        /// <returns>0 到 Count - 1 之间的随机索引；列表为 null 或为空时输出警告并返回 -1。</returns>
        public static int CwGetRandomIndex<T>(this IReadOnlyList<T> items)
        {
            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("集合为 null 或不包含任何元素。");
                return -1;
            }

            return Random.Range(0, items.Count);
        }

        /// <summary>用 Fisher-Yates 算法原地随机打乱元素顺序，每种排列概率相同。</summary>
        /// <typeparam name="T">列表元素类型。</typeparam>
        /// <param name="items">要打乱的列表或数组，会被直接修改；需要保留原顺序时请先复制。</param>
        /// <returns>传入的同一个列表，便于链式调用；传入 null 时返回 null。</returns>
        public static IList<T> CwShuffle<T>(this IList<T> items)
        {
            if (items == null)
            {
                return null;
            }

            for (int i = items.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                if (randomIndex == i)
                {
                    continue;
                }

                T item = items[i];
                items[i] = items[randomIndex];
                items[randomIndex] = item;
            }

            return items;
        }

        /// <summary>随机取出一个元素并从列表中删除，可用于不重复抽取。</summary>
        /// <typeparam name="T">列表元素类型。</typeparam>
        /// <param name="items">要抽取的列表，会被直接修改；不能传入数组等固定长度集合，否则抛出 NotSupportedException。</param>
        /// <returns>被删除的元素；列表为 null 或为空时返回类型默认值。</returns>
        public static T CwPopRandom<T>(this IList<T> items)
        {
            if (items == null || items.Count == 0)
            {
                return default;
            }

            int index = Random.Range(0, items.Count);
            T item = items[index];
            items.RemoveAt(index);
            return item;
        }

        /// <summary>按权重随机返回一个元素，权重越大被选中的概率越高，不修改列表。</summary>
        /// <typeparam name="T">列表元素类型。</typeparam>
        /// <param name="items">要取值的列表或数组。</param>
        /// <param name="weightSelector">返回每个元素权重的函数；小于等于 0、NaN 和无穷大的权重视为不参与抽取。每个元素会被调用两次，需要对同一元素返回相同结果。</param>
        /// <returns>选中的元素；列表为空、weightSelector 为 null 或没有有效权重时返回类型默认值。</returns>
        public static T CwGetWeightedRandom<T>(
            this IReadOnlyList<T> items,
            Func<T, float> weightSelector)
        {
            if (items == null || items.Count == 0 || weightSelector == null)
            {
                return default;
            }

            float totalWeight = 0f;
            for (int i = 0; i < items.Count; i++)
            {
                float weight = weightSelector(items[i]);
                if (weight > 0f && !float.IsNaN(weight) && !float.IsInfinity(weight))
                {
                    totalWeight += weight;
                }
            }

            if (totalWeight <= 0f || float.IsInfinity(totalWeight))
            {
                return default;
            }

            float randomWeight = Random.value * totalWeight;
            T lastValidItem = default;
            bool hasValidItem = false;
            for (int i = 0; i < items.Count; i++)
            {
                T item = items[i];
                float weight = weightSelector(item);
                if (weight <= 0f || float.IsNaN(weight) || float.IsInfinity(weight))
                {
                    continue;
                }

                lastValidItem = item;
                hasValidItem = true;
                randomWeight -= weight;
                if (randomWeight <= 0f)
                {
                    return item;
                }
            }

            return hasValidItem ? lastValidItem : default;
        }

        /// <summary>从列表中删除 null 和已被 Destroy 的 Unity 对象，保留其余元素的顺序。</summary>
        /// <typeparam name="T">Unity 对象类型，例如 GameObject、Transform。</typeparam>
        /// <param name="items">要清理的列表，会被直接修改；不能传入数组等固定长度集合，否则抛出 NotSupportedException。</param>
        /// <returns>删除的元素数量；列表为 null 时返回 0。</returns>
        public static int CwRemoveNullObjects<T>(this IList<T> items)
            where T : UnityEngine.Object
        {
            if (items == null)
            {
                return 0;
            }

            int removedCount = 0;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i] != null)
                {
                    continue;
                }

                items.RemoveAt(i);
                removedCount++;
            }

            return removedCount;
        }
    }
}
