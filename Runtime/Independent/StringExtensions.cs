using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>
    /// 字符串扩展：多行缩进、过滤控制字符和空白、生成 Unity 富文本标签。
    /// </summary>
    /// <remarks>富文本方法只拼接标签，显示端需要开启 Rich Text（UGUI Text 的 Rich Text 选项，TMP 默认开启）才会生效，否则标签会原样显示。</remarks>
    public static class StringExtensions
    {
        /// <summary>在每一行开头插入缩进，常用于中文段落首行缩进。</summary>
        /// <param name="text">要处理的文本，按 \n 分行。</param>
        /// <param name="includeFirstLine">第一行是否也缩进；为 false 时只缩进第二行及之后的行。</param>
        /// <param name="indent">插入的缩进内容，默认两个全角空格。</param>
        /// <returns>缩进后的新字符串；text 为 null 或空时原样返回。</returns>
        public static string CwIndentLines(this string text, bool includeFirstLine = true, string indent = "　　")
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

        /// <summary>删除不可见的控制字符（如 \0、\a 等 char.IsControl 为 true 的字符），普通空格保留。常用于清理外部导入或粘贴的文本。</summary>
        /// <param name="text">要处理的文本。</param>
        /// <param name="preserveFormattingWhitespace">为 true（默认）时保留 \t、\r、\n，以保持换行和缩进；为 false 时一并删除。</param>
        /// <returns>过滤后的新字符串；text 为 null 或空时原样返回。</returns>
        public static string CwRemoveControlCharacters(this string text, bool preserveFormattingWhitespace = true)
        {
            return FilterCharacters(text, false, preserveFormattingWhitespace);
        }

        /// <summary>删除全部空白字符（包括空格、全角空格、制表符和换行）和控制字符，常用于比较或校验输入内容。</summary>
        /// <param name="text">要处理的文本。</param>
        /// <returns>过滤后的新字符串；text 为 null 或空时原样返回。</returns>
        public static string CwRemoveWhitespaceAndControlCharacters(this string text)
        {
            return FilterCharacters(text, true, false);
        }

        /// <summary>用 &lt;b&gt; 标签包裹文本，显示为粗体。可与其他富文本方法链式组合。</summary>
        /// <param name="text">要加粗的文本。</param>
        /// <returns>包裹标签后的新字符串；text 为 null 或空时原样返回。</returns>
        public static string CwRichBold(this string text)
        {
            return WrapRichText(text, "<b>", "</b>");
        }

        /// <summary>用 &lt;i&gt; 标签包裹文本，显示为斜体。可与其他富文本方法链式组合。</summary>
        /// <param name="text">要设为斜体的文本。</param>
        /// <returns>包裹标签后的新字符串；text 为 null 或空时原样返回。</returns>
        public static string CwRichItalic(this string text)
        {
            return WrapRichText(text, "<i>", "</i>");
        }

        /// <summary>用 &lt;size&gt; 标签包裹文本，单独设置这段文本的字号。可与其他富文本方法链式组合。</summary>
        /// <param name="text">要设置字号的文本。</param>
        /// <param name="fontSize">字号，必须大于 0。</param>
        /// <returns>包裹标签后的新字符串；text 为 null 或空时原样返回。</returns>
        /// <exception cref="ArgumentOutOfRangeException">text 非空且 fontSize 小于等于 0。</exception>
        public static string CwRichSize(this string text, int fontSize)
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

        /// <summary>用 &lt;color&gt; 标签包裹文本，设置这段文本的颜色。可与其他富文本方法链式组合。</summary>
        /// <param name="text">要设置颜色的文本。</param>
        /// <param name="color">颜色名称或带 # 的十六进制值，例如 red、#FF0000、#FF000080。不校验是否为有效颜色，无效值可能导致标签原样显示。</param>
        /// <returns>包裹标签后的新字符串；text 为 null 或空时原样返回。</returns>
        /// <exception cref="ArgumentException">text 非空且 color 为空白或包含 &lt; &gt;。</exception>
        public static string CwRichColor(this string text, string color)
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

        /// <summary>用 &lt;color&gt; 标签包裹文本，颜色由 UnityEngine.Color 转成十六进制值。可与其他富文本方法链式组合。</summary>
        /// <param name="text">要设置颜色的文本。</param>
        /// <param name="color">文本颜色。</param>
        /// <param name="includeAlpha">为 true（默认）时包含透明度；为 false 时只输出 RGB，透明度保持原样。</param>
        /// <returns>包裹标签后的新字符串；text 为 null 或空时原样返回。</returns>
        public static string CwRichColor(this string text, Color color, bool includeAlpha = true)
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
        /// <param name="text">要过滤的字符串。</param>
        /// <param name="removeWhitespace">是否移除所有空白字符。</param>
        /// <param name="preserveFormattingWhitespace">是否保留制表符、回车符和换行符。</param>
        /// <returns>过滤后的字符串；输入为 null 或空字符串时原样返回。</returns>
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
        /// <param name="text">要包裹的字符串。</param>
        /// <param name="openingTag">放在字符串前面的开始标签。</param>
        /// <param name="closingTag">放在字符串后面的结束标签。</param>
        /// <returns>包裹标签后的字符串；输入为 null 或空字符串时原样返回。</returns>
        private static string WrapRichText(string text, string openingTag, string closingTag)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return openingTag + text + closingTag;
        }
    }
}
