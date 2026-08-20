using UnityEngine;

/// <summary>
/// 提供 Vector、Color 和 LayerMask 的常用值扩展。
/// </summary>
public static class ValueExtensions
{
    /// <summary>返回将 X 分量替换为指定值的 Vector2。</summary>
    /// <param name="value">原始 Vector2。</param>
    /// <param name="x">新的 X 分量。</param>
    /// <returns>修改 X 分量后的 Vector2。</returns>
    public static Vector2 ExtWithX(this Vector2 value, float x) { value.x = x; return value; }

    /// <summary>返回将 Y 分量替换为指定值的 Vector2。</summary>
    /// <param name="value">原始 Vector2。</param>
    /// <param name="y">新的 Y 分量。</param>
    /// <returns>修改 Y 分量后的 Vector2。</returns>
    public static Vector2 ExtWithY(this Vector2 value, float y) { value.y = y; return value; }

    /// <summary>返回将指定值累加到 X 分量的 Vector2。</summary>
    /// <param name="value">原始 Vector2。</param>
    /// <param name="x">要累加到 X 分量的值。</param>
    /// <returns>累加 X 分量后的 Vector2。</returns>
    public static Vector2 ExtAddX(this Vector2 value, float x) { value.x += x; return value; }

    /// <summary>返回将指定值累加到 Y 分量的 Vector2。</summary>
    /// <param name="value">原始 Vector2。</param>
    /// <param name="y">要累加到 Y 分量的值。</param>
    /// <returns>累加 Y 分量后的 Vector2。</returns>
    public static Vector2 ExtAddY(this Vector2 value, float y) { value.y += y; return value; }

    /// <summary>返回将 X 分量替换为指定值的 Vector3。</summary>
    /// <param name="value">原始 Vector3。</param>
    /// <param name="x">新的 X 分量。</param>
    /// <returns>修改 X 分量后的 Vector3。</returns>
    public static Vector3 ExtWithX(this Vector3 value, float x) { value.x = x; return value; }

    /// <summary>返回将 Y 分量替换为指定值的 Vector3。</summary>
    /// <param name="value">原始 Vector3。</param>
    /// <param name="y">新的 Y 分量。</param>
    /// <returns>修改 Y 分量后的 Vector3。</returns>
    public static Vector3 ExtWithY(this Vector3 value, float y) { value.y = y; return value; }

    /// <summary>返回将 Z 分量替换为指定值的 Vector3。</summary>
    /// <param name="value">原始 Vector3。</param>
    /// <param name="z">新的 Z 分量。</param>
    /// <returns>修改 Z 分量后的 Vector3。</returns>
    public static Vector3 ExtWithZ(this Vector3 value, float z) { value.z = z; return value; }

    /// <summary>返回将指定值累加到 X 分量的 Vector3。</summary>
    /// <param name="value">原始 Vector3。</param>
    /// <param name="x">要累加到 X 分量的值。</param>
    /// <returns>累加 X 分量后的 Vector3。</returns>
    public static Vector3 ExtAddX(this Vector3 value, float x) { value.x += x; return value; }

    /// <summary>返回将指定值累加到 Y 分量的 Vector3。</summary>
    /// <param name="value">原始 Vector3。</param>
    /// <param name="y">要累加到 Y 分量的值。</param>
    /// <returns>累加 Y 分量后的 Vector3。</returns>
    public static Vector3 ExtAddY(this Vector3 value, float y) { value.y += y; return value; }

    /// <summary>返回将指定值累加到 Z 分量的 Vector3。</summary>
    /// <param name="value">原始 Vector3。</param>
    /// <param name="z">要累加到 Z 分量的值。</param>
    /// <returns>累加 Z 分量后的 Vector3。</returns>
    public static Vector3 ExtAddZ(this Vector3 value, float z) { value.z += z; return value; }

    /// <summary>返回只修改 Alpha 的颜色，Alpha 自动限制在 0 到 1。</summary>
    /// <param name="value">原始颜色。</param>
    /// <param name="alpha">新的 Alpha 值。</param>
    /// <returns>修改 Alpha 后的颜色。</returns>
    public static Color ExtWithAlpha(this Color value, float alpha)
    {
        value.a = Mathf.Clamp01(alpha);
        return value;
    }

    /// <summary>转换为不带井号的 HTML 十六进制颜色字符串。</summary>
    /// <param name="value">要转换的颜色。</param>
    /// <param name="includeAlpha">是否在结果中包含 Alpha 通道。</param>
    /// <returns>不带井号的 RGB 或 RGBA 十六进制颜色字符串。</returns>
    public static string ExtToHex(this Color value, bool includeAlpha = true)
    {
        return includeAlpha
            ? ColorUtility.ToHtmlStringRGBA(value)
            : ColorUtility.ToHtmlStringRGB(value);
    }

    /// <summary>解析颜色名称和常用 HTML 十六进制颜色格式。</summary>
    /// <param name="value">要解析的颜色名称或十六进制字符串。</param>
    /// <param name="color">解析成功时返回对应颜色；失败时返回默认颜色。</param>
    /// <returns>解析成功时返回 true，否则返回 false。</returns>
    public static bool ExtTryParseColor(this string value, out Color color)
    {
        color = default;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        string normalized = value.Trim();
        bool looksLikeHex = (normalized.Length == 3 || normalized.Length == 4 ||
                             normalized.Length == 6 || normalized.Length == 8) &&
                            IsHexString(normalized);
        if (looksLikeHex && normalized[0] != '#')
        {
            normalized = "#" + normalized;
        }

        return ColorUtility.TryParseHtmlString(normalized, out color);
    }

    /// <summary>判断字符串是否只包含十六进制字符。</summary>
    /// <param name="value">要检查的字符串。</param>
    /// <returns>全部字符都是十六进制字符时返回 true，否则返回 false。</returns>
    private static bool IsHexString(string value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            char character = value[i];
            bool isHex = character >= '0' && character <= '9' ||
                         character >= 'a' && character <= 'f' ||
                         character >= 'A' && character <= 'F';
            if (!isHex) return false;
        }
        return true;
    }

    /// <summary>判断 LayerMask 是否包含指定 Layer。</summary>
    /// <param name="mask">要检查的 LayerMask。</param>
    /// <param name="layer">要检查的 Layer 索引。</param>
    /// <returns>Layer 索引有效且包含在 LayerMask 中时返回 true，否则返回 false。</returns>
    public static bool ExtContains(this LayerMask mask, int layer)
    {
        return layer >= 0 && layer <= 31 && (mask.value & (1 << layer)) != 0;
    }

    /// <summary>判断 LayerMask 是否包含目标游戏对象所在的 Layer。</summary>
    /// <param name="mask">要检查的 LayerMask。</param>
    /// <param name="target">提供 Layer 的目标游戏对象。</param>
    /// <returns>目标不为 null 且其 Layer 包含在 LayerMask 中时返回 true，否则返回 false。</returns>
    public static bool ExtContains(this LayerMask mask, GameObject target)
    {
        return target != null && mask.ExtContains(target.layer);
    }

    /// <summary>返回包含指定 Layer 的 LayerMask；Layer 无效时原样返回。</summary>
    /// <param name="mask">原始 LayerMask。</param>
    /// <param name="layer">要添加的 Layer 索引。</param>
    /// <returns>添加指定 Layer 后的 LayerMask。</returns>
    public static LayerMask ExtAddLayer(this LayerMask mask, int layer)
    {
        if (layer >= 0 && layer <= 31) mask.value |= 1 << layer;
        return mask;
    }

    /// <summary>返回移除指定 Layer 后的 LayerMask；Layer 无效时原样返回。</summary>
    /// <param name="mask">原始 LayerMask。</param>
    /// <param name="layer">要移除的 Layer 索引。</param>
    /// <returns>移除指定 Layer 后的 LayerMask。</returns>
    public static LayerMask ExtRemoveLayer(this LayerMask mask, int layer)
    {
        if (layer >= 0 && layer <= 31) mask.value &= ~(1 << layer);
        return mask;
    }
}
