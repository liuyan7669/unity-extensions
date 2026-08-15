using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 提供字符串缩进、字符过滤和 Unity 富文本扩展。
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 为字符串的每一行添加指定缩进。
    /// </summary>
    /// <param name="text">要处理的字符串。</param>
    /// <param name="includeFirstLine">是否为第一行添加缩进。</param>
    /// <param name="indent">每行使用的缩进字符串。</param>
    /// <returns>添加缩进后的字符串；输入为 null 或空字符串时原样返回。</returns>
    public static string ExtIndentLines(this string text, bool includeFirstLine = true, string indent = "　　")
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        if (includeFirstLine)
        {
            return Regex.Replace(text, "^", indent, RegexOptions.Multiline);
        }

        return Regex.Replace(text, "(?<=\n)", _ => indent, RegexOptions.Multiline);
    }

    /// <summary>
    /// 移除控制字符，可选择保留制表符和换行符。
    /// </summary>
    /// <param name="text">要处理的字符串。</param>
    /// <param name="preserveFormattingWhitespace">是否保留制表符、回车符和换行符。</param>
    /// <returns>过滤后的字符串；输入为 null 或空字符串时原样返回。</returns>
    public static string ExtRemoveControlCharacters(this string text, bool preserveFormattingWhitespace = true)
    {
        return FilterCharacters(text, false, preserveFormattingWhitespace);
    }

    /// <summary>
    /// 移除所有空白字符和控制字符。
    /// </summary>
    /// <param name="text">要处理的字符串。</param>
    /// <returns>过滤后的字符串；输入为 null 或空字符串时原样返回。</returns>
    public static string ExtRemoveWhitespaceAndControlCharacters(this string text)
    {
        return FilterCharacters(text, true, false);
    }

    /// <summary>
    /// 使用 Unity 富文本粗体标签包裹字符串。
    /// </summary>
    /// <param name="text">要设置为粗体的字符串。</param>
    /// <returns>包裹粗体标签后的字符串；输入为 null 或空字符串时原样返回。</returns>
    public static string ExtRichBold(this string text)
    {
        return WrapRichText(text, "<b>", "</b>");
    }

    /// <summary>
    /// 使用 Unity 富文本斜体标签包裹字符串。
    /// </summary>
    /// <param name="text">要设置为斜体的字符串。</param>
    /// <returns>包裹斜体标签后的字符串；输入为 null 或空字符串时原样返回。</returns>
    public static string ExtRichItalic(this string text)
    {
        return WrapRichText(text, "<i>", "</i>");
    }

    /// <summary>
    /// 使用 Unity 富文本字号标签包裹字符串。
    /// </summary>
    /// <param name="text">要设置字号的字符串。</param>
    /// <param name="fontSize">大于 0 的字号。</param>
    /// <returns>包裹字号标签后的字符串；输入为 null 或空字符串时原样返回。</returns>
    /// <exception cref="ArgumentOutOfRangeException">字号小于或等于 0。</exception>
    public static string ExtRichSize(this string text, int fontSize)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        if (fontSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fontSize), "富文本字号必须大于 0。");
        }

        return WrapRichText(text, $"<size={fontSize}>", "</size>");
    }

    /// <summary>
    /// 使用 Unity 富文本颜色标签包裹字符串。
    /// </summary>
    /// <param name="text">要设置颜色的字符串。</param>
    /// <param name="color">颜色名称或 HTML 十六进制颜色，例如 red、#FF0000、#FF000080。</param>
    /// <returns>包裹颜色标签后的字符串；输入为 null 或空字符串时原样返回。</returns>
    /// <exception cref="ArgumentException">颜色参数为空或包含标签分隔符。</exception>
    public static string ExtRichColor(this string text, string color)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        if (string.IsNullOrWhiteSpace(color))
        {
            throw new ArgumentException("富文本颜色不能为空。", nameof(color));
        }

        string colorValue = color.Trim();
        if (colorValue.IndexOf('<') >= 0 || colorValue.IndexOf('>') >= 0)
        {
            throw new ArgumentException("富文本颜色不能包含尖括号。", nameof(color));
        }

        return WrapRichText(text, $"<color={colorValue}>", "</color>");
    }

    /// <summary>
    /// 使用 UnityEngine.Color 生成十六进制富文本颜色标签。
    /// </summary>
    /// <param name="text">要设置颜色的字符串。</param>
    /// <param name="color">要转换为 HTML 十六进制值的颜色。</param>
    /// <param name="includeAlpha">是否在颜色值中包含 Alpha 通道。</param>
    /// <returns>包裹颜色标签后的字符串；输入为 null 或空字符串时原样返回。</returns>
    public static string ExtRichColor(this string text, Color color, bool includeAlpha = true)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        string htmlColor = includeAlpha
            ? ColorUtility.ToHtmlStringRGBA(color)
            : ColorUtility.ToHtmlStringRGB(color);
        return WrapRichText(text, $"<color=#{htmlColor}>", "</color>");
    }

    /// <summary>
    /// 按指定规则过滤字符串中的空白字符和控制字符。
    /// </summary>
    private static string FilterCharacters(string text, bool removeWhitespace, bool preserveFormattingWhitespace)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return new string(text.Where(character =>
        {
            if (removeWhitespace && char.IsWhiteSpace(character))
            {
                return false;
            }

            if (!char.IsControl(character))
            {
                return true;
            }

            return preserveFormattingWhitespace &&
                   (character == '\t' || character == '\n' || character == '\r');
        }).ToArray());
    }

    /// <summary>使用一对 Unity 富文本标签包裹非空字符串。</summary>
    private static string WrapRichText(string text, string openingTag, string closingTag)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return openingTag + text + closingTag;
    }
}
