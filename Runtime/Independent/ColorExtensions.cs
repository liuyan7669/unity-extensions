using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>
    /// 提供颜色 Alpha 修改、十六进制转换和颜色字符串解析扩展。
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>返回只修改透明度的新颜色，原值不变。</summary>
        /// <param name="value">原颜色。</param>
        /// <param name="alpha">新的透明度，自动限制在 0 到 1。</param>
        /// <returns>修改透明度后的新颜色。</returns>
        public static Color CwWithAlpha(this Color value, float alpha)
        {
            value.a = Mathf.Clamp01(alpha);
            return value;
        }

        /// <summary>把颜色转换为不带 # 的大写十六进制字符串，例如 FF0000FF。</summary>
        /// <param name="value">要转换的颜色。</param>
        /// <param name="includeAlpha">为 true（默认）时输出 RRGGBBAA；为 false 时输出 RRGGBB。</param>
        /// <returns>十六进制颜色字符串。</returns>
        /// <remarks>各通道会被限制在 0 到 1 后换算为 0 到 255，HDR 颜色超出 1 的部分会丢失。</remarks>
        public static string CwToHex(this Color value, bool includeAlpha = true)
        {
            return includeAlpha
                ? ColorUtility.ToHtmlStringRGBA(value)
                : ColorUtility.ToHtmlStringRGB(value);
        }

        /// <summary>把颜色名称或十六进制字符串解析为 Color，十六进制可以省略 #。</summary>
        /// <param name="value">颜色名称（red、blue 等 Unity 支持的名称）或十六进制 RGB、RGBA、RRGGBB、RRGGBBAA；前后空白会被忽略。</param>
        /// <param name="color">成功时为解析出的颜色，失败时为 default。</param>
        /// <returns>解析成功时返回 true。</returns>
        public static bool CwTryParseColor(this string value, out Color color)
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
    }
}
