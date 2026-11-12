using UnityEngine;

namespace Cowart.CwExtensions
{
    /// <summary>
    /// 对象生命周期扩展：在运行模式下安全销毁 UnityEngine.Object。
    /// </summary>
    [UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "Cowart.Extensions", sourceAssembly: "Cowart.Extensions", sourceClassName: "ObjectLifetimeExtensions")]
    public static class ObjectLifetimeExtensions
    {
        /// <summary>在运行模式下销毁场景中的对象（GameObject 或组件），销毁在当前帧末生效。</summary>
        /// <param name="target">要销毁的对象。</param>
        /// <returns>已提交销毁时返回 true；对象为 null、已被销毁或处于编辑模式时返回 false。</returns>
        /// <remarks>编辑模式下不执行任何操作，避免误删场景、Prefab 或资源。不能用来删除项目中的资源文件，Unity 会报错。</remarks>
        public static bool CwDestroy(this UnityEngine.Object target)
        {
            if (target == null || !Application.isPlaying)
            {
                return false;
            }

            UnityEngine.Object.Destroy(target);
            return true;
        }
    }
}
