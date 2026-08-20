# Changelog

## [1.0.5] - 2026-08-20

- 新增组件批量添加、获取或添加、父子级查找及运行时安全移除扩展。
- 新增常用 Transform、激活状态、层级操作和运行时安全销毁扩展。
- 新增自动计算完整子层级并添加或更新 BoxCollider 的扩展，支持旋转、非等比缩放和未激活子物体。
- 所有删除操作限制为仅在运行模式下执行，避免编辑模式误改场景、Prefab 或资源。
- 补全 Runtime 与 Sample 全部方法、形参、泛型形参及返回值的 XML 注释。
- 最低 Unity 版本调整为 2019.4，并显式声明 Unity 3D Physics 内置模块依赖。
- 扩充 EditMode 自动化测试，并完成 Unity 2019.4、2021.3 和 2022.3 兼容性验证。

## [1.0.4] - 2026-08-15

- 新增集合空值、安全索引、打乱、随机弹出、权重随机和 Unity 空对象清理扩展。
- 新增 Transform 重置、递归 Layer、递归查找、层级路径、清空子物体、轴向位置和合并 Renderer Bounds 扩展。
- 新增 Vector、Color 和 LayerMask 值扩展，并加入 EditMode 自动化测试。

## [1.0.2] - 2026-08-07

- 新增 `ExtRichBold`、`ExtRichItalic` 和 `ExtRichSize` 字符串富文本扩展。
- 新增字符串颜色值及 `UnityEngine.Color` 两种 `ExtRichColor` 重载。
- 富文本扩展对 null 和空字符串保持原值，并验证字号及颜色参数。

## [1.0.1] - 2026-08-07

- 将 `UILayoutExtensions` 迁移到 UI Framework `1.0.3`，保留原类名和扩展方法签名。
- 移除 Extensions 运行时程序集及包清单对 Unity UI 的依赖。

## [1.0.0] - 2026-07-17

- Initial UPM package.
- Added the optional Highlight Plus integration sample.
