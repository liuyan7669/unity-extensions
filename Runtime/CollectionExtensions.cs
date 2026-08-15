using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 提供只读列表的日志输出和随机取值扩展。
/// </summary>
public static class CollectionExtensions
{
    /// <summary>判断只读集合是否为 null 或不包含任何元素。</summary>
    public static bool ExtIsNullOrEmpty<T>(this IReadOnlyCollection<T> items)
    {
        return items == null || items.Count == 0;
    }

    /// <summary>安全获取指定索引的元素；索引无效时返回调用方提供的默认值。</summary>
    public static T ExtGetOrDefault<T>(
        this IReadOnlyList<T> items,
        int index,
        T defaultValue = default)
    {
        return items != null && index >= 0 && index < items.Count
            ? items[index]
            : defaultValue;
    }

    /// <summary>
    /// 在一条日志中输出列表的元素数量、索引和内容。
    /// </summary>
    /// <typeparam name="T">列表元素类型。</typeparam>
    /// <param name="items">要输出的只读列表。</param>
    /// <param name="context">与日志关联的 Unity 对象。</param>
    public static void ExtLogItems<T>(this IReadOnlyList<T> items, UnityEngine.Object context = null)
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

    /// <summary>
    /// 从只读列表中随机获取一个元素。
    /// </summary>
    /// <typeparam name="T">列表元素类型。</typeparam>
    /// <param name="items">要随机取值的只读列表。</param>
    /// <returns>随机元素；列表为 null 或空列表时返回类型默认值。</returns>
    public static T ExtGetRandomItem<T>(this IReadOnlyList<T> items)
    {
        int index = items.ExtGetRandomIndex();
        return index >= 0 ? items[index] : default;
    }

    /// <summary>
    /// 从只读列表中随机获取一个有效索引。
    /// </summary>
    /// <typeparam name="T">列表元素类型。</typeparam>
    /// <param name="items">要随机取索引的只读列表。</param>
    /// <returns>随机索引；列表为 null 或空列表时返回 -1。</returns>
    public static int ExtGetRandomIndex<T>(this IReadOnlyList<T> items)
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("集合为 null 或不包含任何元素。");
            return -1;
        }

        return Random.Range(0, items.Count);
    }

    /// <summary>使用 Fisher-Yates 算法原地随机打乱列表。</summary>
    /// <returns>原列表，便于链式调用；目标为 null 时返回 null。</returns>
    public static IList<T> ExtShuffle<T>(this IList<T> items)
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

    /// <summary>随机取出并删除列表中的一个元素。</summary>
    public static T ExtPopRandom<T>(this IList<T> items)
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

    /// <summary>按元素权重随机获取一个值。</summary>
    public static T ExtGetWeightedRandom<T>(
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

    /// <summary>从列表中移除 null 或已经销毁的 Unity 对象。</summary>
    /// <returns>实际移除的元素数量。</returns>
    public static int ExtRemoveNullObjects<T>(this IList<T> items)
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
