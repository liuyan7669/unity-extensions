using UnityEngine;

namespace Cowart.Extensions
{
    /// <summary>Vector 扩展：返回替换或累加单个分量后的新 Vector2 / Vector3。</summary>
    /// <remarks>Vector 是值类型，方法返回新值、不修改原值，需要接收返回值，例如 transform.position = transform.position.CwWithY(0f)。</remarks>
    public static class VectorExtensions
    {
        /// <summary>返回把 X 分量替换为 x 的新 Vector2。</summary>
        /// <param name="value">原 Vector2。</param>
        /// <param name="x">新的 X 分量。</param>
        /// <returns>替换后的新值。</returns>
        public static Vector2 CwWithX(this Vector2 value, float x) { value.x = x; return value; }

        /// <summary>返回把 Y 分量替换为 y 的新 Vector2。</summary>
        /// <param name="value">原 Vector2。</param>
        /// <param name="y">新的 Y 分量。</param>
        /// <returns>替换后的新值。</returns>
        public static Vector2 CwWithY(this Vector2 value, float y) { value.y = y; return value; }

        /// <summary>返回在 X 分量上加上 x 的新 Vector2。</summary>
        /// <param name="value">原 Vector2。</param>
        /// <param name="x">要加到 X 分量上的值，可为负数。</param>
        /// <returns>累加后的新值。</returns>
        public static Vector2 CwAddX(this Vector2 value, float x) { value.x += x; return value; }

        /// <summary>返回在 Y 分量上加上 y 的新 Vector2。</summary>
        /// <param name="value">原 Vector2。</param>
        /// <param name="y">要加到 Y 分量上的值，可为负数。</param>
        /// <returns>累加后的新值。</returns>
        public static Vector2 CwAddY(this Vector2 value, float y) { value.y += y; return value; }

        /// <summary>返回把 X 分量替换为 x 的新 Vector3。</summary>
        /// <param name="value">原 Vector3。</param>
        /// <param name="x">新的 X 分量。</param>
        /// <returns>替换后的新值。</returns>
        public static Vector3 CwWithX(this Vector3 value, float x) { value.x = x; return value; }

        /// <summary>返回把 Y 分量替换为 y 的新 Vector3。</summary>
        /// <param name="value">原 Vector3。</param>
        /// <param name="y">新的 Y 分量。</param>
        /// <returns>替换后的新值。</returns>
        public static Vector3 CwWithY(this Vector3 value, float y) { value.y = y; return value; }

        /// <summary>返回把 Z 分量替换为 z 的新 Vector3。</summary>
        /// <param name="value">原 Vector3。</param>
        /// <param name="z">新的 Z 分量。</param>
        /// <returns>替换后的新值。</returns>
        public static Vector3 CwWithZ(this Vector3 value, float z) { value.z = z; return value; }

        /// <summary>返回在 X 分量上加上 x 的新 Vector3。</summary>
        /// <param name="value">原 Vector3。</param>
        /// <param name="x">要加到 X 分量上的值，可为负数。</param>
        /// <returns>累加后的新值。</returns>
        public static Vector3 CwAddX(this Vector3 value, float x) { value.x += x; return value; }

        /// <summary>返回在 Y 分量上加上 y 的新 Vector3。</summary>
        /// <param name="value">原 Vector3。</param>
        /// <param name="y">要加到 Y 分量上的值，可为负数。</param>
        /// <returns>累加后的新值。</returns>
        public static Vector3 CwAddY(this Vector3 value, float y) { value.y += y; return value; }

        /// <summary>返回在 Z 分量上加上 z 的新 Vector3。</summary>
        /// <param name="value">原 Vector3。</param>
        /// <param name="z">要加到 Z 分量上的值，可为负数。</param>
        /// <returns>累加后的新值。</returns>
        public static Vector3 CwAddZ(this Vector3 value, float z) { value.z += z; return value; }
    }
}
