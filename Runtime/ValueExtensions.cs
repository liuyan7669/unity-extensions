using UnityEngine;

/// <summary>
/// 提供 Vector、Color 和 LayerMask 的常用值扩展。
/// </summary>
public static class ValueExtensions
{
    public static Vector2 ExtWithX(this Vector2 value, float x) { value.x = x; return value; }
    public static Vector2 ExtWithY(this Vector2 value, float y) { value.y = y; return value; }
    public static Vector2 ExtAddX(this Vector2 value, float x) { value.x += x; return value; }
    public static Vector2 ExtAddY(this Vector2 value, float y) { value.y += y; return value; }

    public static Vector3 ExtWithX(this Vector3 value, float x) { value.x = x; return value; }
    public static Vector3 ExtWithY(this Vector3 value, float y) { value.y = y; return value; }
    public static Vector3 ExtWithZ(this Vector3 value, float z) { value.z = z; return value; }
    public static Vector3 ExtAddX(this Vector3 value, float x) { value.x += x; return value; }
    public static Vector3 ExtAddY(this Vector3 value, float y) { value.y += y; return value; }
    public static Vector3 ExtAddZ(this Vector3 value, float z) { value.z += z; return value; }

    /// <summary>返回只修改 Alpha 的颜色，Alpha 自动限制在 0 到 1。</summary>
    public static Color ExtWithAlpha(this Color value, float alpha)
    {
        value.a = Mathf.Clamp01(alpha);
        return value;
    }

    /// <summary>转换为不带井号的 HTML 十六进制颜色字符串。</summary>
    public static string ExtToHex(this Color value, bool includeAlpha = true)
    {
        return includeAlpha
            ? ColorUtility.ToHtmlStringRGBA(value)
            : ColorUtility.ToHtmlStringRGB(value);
    }

    /// <summary>解析颜色名称和常用 HTML 十六进制颜色格式。</summary>
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

    public static bool ExtContains(this LayerMask mask, int layer)
    {
        return layer >= 0 && layer <= 31 && (mask.value & (1 << layer)) != 0;
    }

    public static bool ExtContains(this LayerMask mask, GameObject target)
    {
        return target != null && mask.ExtContains(target.layer);
    }

    public static LayerMask ExtAddLayer(this LayerMask mask, int layer)
    {
        if (layer >= 0 && layer <= 31) mask.value |= 1 << layer;
        return mask;
    }

    public static LayerMask ExtRemoveLayer(this LayerMask mask, int layer)
    {
        if (layer >= 0 && layer <= 31) mask.value &= ~(1 << layer);
        return mask;
    }
}
