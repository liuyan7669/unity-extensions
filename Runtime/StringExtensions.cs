using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// 提供字符串缩进和字符过滤扩展。
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
}
