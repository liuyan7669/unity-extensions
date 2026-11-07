# Highlight Plus Integration

导入此示例前，请先安装 Highlight Plus。示例中的类型位于 `Cowart.Extensions` 命名空间。

示例结构：

- `Independent/GameObjectHighlightExtensions`：`CwSetHighlight`，开启或取消高亮，开启时描边带呼吸效果，取消时立即关闭。
- `Independent/CwHighlightBreath`：由 `CwSetHighlight` 自动添加的内部呼吸组件，不出现在 Add Component 菜单中。
- `GameObjectHighlightCompositeExtensions`：`CwSetHighlightClick`，组合本示例的高亮扩展与包内的点击扩展 `CwSetClickAction`：高亮并等待一次点击，点击后自动取消高亮和监听再执行回调。

点击检测要求 3D 物体带 Collider，场景中有 EventSystem 和带 PhysicsRaycaster 的相机。

Highlight Plus 缺失时，导入的脚本会因为找不到 `HighlightPlus` 命名空间而产生编译错误。
