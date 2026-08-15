# Changelog

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
