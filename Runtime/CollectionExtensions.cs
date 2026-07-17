using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 提供只读列表的日志输出和随机取值扩展。
/// </summary>
public static class CollectionExtensions
{
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
}
