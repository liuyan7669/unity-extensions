# Changelog

## [Unreleased]

## [3.1.0] - 2026-11-14

- 接入 OpenUPM 官方 tag 发布工作流，自动触发扫描并检查注册表发布结果。

- 统一 Cowart 包版本为 3.1.0。

- 声明 Inspector Groups 3.1.0 依赖，随包安装统一 Inspector 支持。

## [3.0.0] - 2026-11-12

### 破坏性变更

- `com.cowart.extensions` 改为 `com.cowart.cw-extensions`；更新业务项目的安装地址、程序集和源码引用。
- 完成包标识、目录、程序集、命名空间、脚本及文档改名，保持资源 GUID 和包独立性。

## [2.0.1] - 2026-11-08

- 修复 Highlight Plus Integration 示例取消高亮时描边闪一下的问题：原先先恢复呼吸前的描边强度再淡出，描边会先跳到最亮；现在 `CwSetHighlight(false)` 和 `CwSetHighlightClick` 取消时立即关闭高亮，不经过淡出，物体原有的 `fadeOutDuration` 配置保持不变。
- 示例默认样式不再设置 `fadeOutDuration`。

## [2.0.0] - 2026-11-08

### 破坏性变更

- 扩展方法前缀由 `Ext` 改为 `Cw`（Cowart），例如 `ExtShuffle` → `CwShuffle`、`ExtGetOrAddComponent` → `CwGetOrAddComponent`；方法名其余部分不变。
- 全部扩展类移入 `Cowart.CwExtensions` 命名空间，调用方需要添加 `using Cowart.CwExtensions;`。
- 按“独立扩展 + 组合扩展”重组 Runtime：`Runtime/Independent` 中每个扩展类只覆盖一个功能范围，其中的方法功能唯一、完整、独立、闭合，不调用其他扩展方法（同一方法的重载转发和递归除外）；`Runtime/GameObjectCompositeExtensions` 与 `Runtime/TransformCompositeExtensions` 存放由多个独立扩展组合而成的用法。
- 拆分 `UnityObjectExtensions` 为 `TransformExtensions`、`HierarchyExtensions`、`ActiveStateExtensions`、`ObjectLifetimeExtensions`、`ComponentExtensions`、`BoundsExtensions`、`GameObjectCompositeExtensions` 和 `TransformCompositeExtensions`；拆分 `ValueExtensions` 为 `VectorExtensions`、`ColorExtensions` 和 `LayerExtensions`。`ExtSetLayerRecursive` 归入 `LayerExtensions`，`ExtSetChildrenActive` 归入 `ActiveStateExtensions`。
- 包新增 `com.unity.ugui` 依赖，用于点击扩展的 EventSystem 接口。

### 新增

- `CwSetClickAction`：基于 `IPointerClickHandler` 设置左键点击回调，重复设置时替换，传入 null 时取消监听。
- `CwTryGetHierarchyLocalBounds`：公开层级局部包围盒计算；`CwAddFittedBoxCollider` 改为由它和 `CwGetOrAddComponent` 组合实现，行为不变。
- 迁入 UI Framework 2.0.0 移除的 UGUI/TMP 扩展（23 个方法名、33 个方法），统一为 `Cw` 前缀并按功能范围拆分：`RectTransformExtensions`、`GraphicExtensions`（`CwSetAlpha` 由 Image 放宽为 Graphic）、`CanvasGroupExtensions`、`SelectableExtensions`、`LayoutExtensions`、`UIEventExtensions`，以及组合扩展 `TextCompositeExtensions.CwSetText`。
- `CwRebuildNearestLayoutNextFrame` 在目标于下一帧前被销毁时不再抛出空引用。
- TextMesh Pro 改为可选依赖：通过 asmdef `versionDefines` 在存在 `com.unity.textmeshpro` 或 `com.unity.ugui` 2.x 时定义 `COWART_TMP`，TMP 相关扩展仅在此时编译。

### Highlight Plus Integration 示例

- 重构为“独立扩展 + 组合扩展”结构，移除对 Event Relays 的依赖；点击扩展移入包内 Runtime，示例只保留依赖 Highlight Plus 的部分。
- `CwSetHighlight` 新增实体提亮、外发光和颜色参数，开启时描边带呼吸效果；新增组合扩展 `GameObjectHighlightCompositeExtensions.CwSetHighlightClick`。
- 移除 `InteractionExtensions`（`ExtBindPointerDown` 及 Component 重载）。触发方式由指针按下改为完整点击，默认高亮样式由青色描边改为暖白描边。已导入的旧示例不受影响，重新导入前请调整调用代码。

### 其他

- 重新设计包图标。

## [1.0.6] - 2026-08-23

- 新增包专用图标资源。

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
