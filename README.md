# Extensions

Unity 常用扩展方法集合，涵盖集合、字符串、GameObject、Component 和 Transform。

所有方法统一使用 `Ext` 前缀，便于在 IntelliSense 中快速搜索并与其他扩展方法区分。

## 功能

### 集合扩展

- 在一条日志中输出集合数量、索引和内容
- 从 `IReadOnlyList<T>` 随机获取元素
- 从 `IReadOnlyList<T>` 随机获取有效索引
- 判断集合是否为空并安全获取索引
- 原地打乱、随机取出并删除元素
- 按权重随机选择元素
- 清理 null 或已经销毁的 Unity 对象
- 对 null 和空集合进行安全处理

### 字符串扩展

- 为多行文本添加缩进
- 移除控制字符
- 可选择保留换行、制表等格式空白
- 同时移除空白和控制字符
- 使用粗体、斜体、字号和颜色 Unity 富文本标签包裹字符串
- 富文本颜色支持颜色名称、HTML 十六进制值和 `UnityEngine.Color`

### Unity 对象扩展

- 通过 Component 设置 GameObject 激活状态
- 复制 Transform 的位置和旋转
- 单独复制位置或旋转
- 仅在水平面朝向指定目标
- 为 GameObject 或 Component 获取或添加指定组件
- 批量添加组件，并在运行模式下移除一个或全部同类型组件
- 重置本地 Transform、递归设置 Layer、递归查找子物体
- 获取完整 Hierarchy 路径、清空子物体和计算合并 Renderer Bounds
- 自动添加或更新能够包裹完整子层级的 BoxCollider
- 单独设置世界或本地位置的 X/Y/Z

### Vector、Color 和 LayerMask

- 返回只改变或累加单个轴的 Vector2/Vector3
- 修改 Color Alpha、生成十六进制颜色并解析 HTML 颜色
- 判断、增加和移除 LayerMask 中的 Layer

## 安装

### 通过 OpenUPM 安装

在 Unity 的 `Edit > Project Settings > Package Manager` 中添加：

```text
Name: package.openupm.com
URL: https://package.openupm.com
Scope: com.cowart.extensions
```

然后通过 `Add package by name` 安装：

```text
com.cowart.extensions
```

### 通过 Git URL 安装

```text
https://github.com/liuyan7669/unity-extensions.git#1.0.5
```

## 使用方法

核心扩展类位于全局命名空间，安装后无需额外添加 `using Cowart.Extensions`。

### 集合日志和随机取值

```csharp
using System.Collections.Generic;

List<string> names = new List<string> { "A", "B", "C" };

names.ExtLogItems();
string randomName = names.ExtGetRandomItem();
int randomIndex = names.ExtGetRandomIndex();
names.ExtShuffle();
string removedName = names.ExtPopRandom();
```

### 字符串处理

```csharp
string text = "第一行\n第二行";

string indented = text.ExtIndentLines();
string cleanText = text.ExtRemoveControlCharacters();
string compactText = text.ExtRemoveWhitespaceAndControlCharacters();
```

`ExtIndentLines` 默认使用两个全角空格缩进，并包含第一行。可以自定义：

```csharp
string result = text.ExtIndentLines(
    includeFirstLine: false,
    indent: "    ");
```

### 字符串富文本

```csharp
string bold = "重点".ExtRichBold();
string italic = "说明".ExtRichItalic();
string large = "标题".ExtRichSize(36);
string red = "警告".ExtRichColor("red");
string translucent = "提示".ExtRichColor(new Color(1f, 0.5f, 0f, 0.8f));

string combined = "重要提示"
    .ExtRichBold()
    .ExtRichSize(32)
    .ExtRichColor(Color.yellow);
```

这些方法只生成 Unity 富文本标签，不依赖 UGUI 或 TMP 组件。显示端仍需启用 Rich Text。`ExtRichColor(Color)` 默认包含 Alpha，可通过 `includeAlpha: false` 只输出 RGB。

### Unity 对象操作

```csharp
targetComponent.ExtSetActive(true);

targetTransform.ExtCopyPoseFrom(referenceTransform);
targetTransform.ExtCopyPositionFrom(referenceTransform);
targetTransform.ExtCopyRotationFrom(referenceTransform);
targetTransform.ExtLookAtHorizontal(referenceTransform);
targetTransform.ExtResetLocal();
targetTransform.gameObject.ExtSetLayerRecursive(7);
Transform child = targetTransform.ExtFindChildRecursive("Target");
string hierarchyPath = targetTransform.ExtGetHierarchyPath();
```

### 获取或添加组件

```csharp
AudioSource audioSource = gameObject.ExtGetOrAddComponent<AudioSource>();
Rigidbody body = transform.ExtGetOrAddComponent<Rigidbody>();
```

目标为 null 时返回 null；目标已包含组件时直接返回已有组件。

### 自动包裹层级的 BoxCollider

```csharp
BoxCollider collider = gameObject.ExtAddFittedBoxCollider();
BoxCollider activeOnlyCollider = transform.ExtAddFittedBoxCollider(
    includeInactive: false);
```

该方法会在调用时计算目标及全部子物体的 Renderer 和子级 3D Collider，
将结果转换为目标物体的局部包围盒，然后添加或更新目标上的 BoxCollider。
默认包含未激活子物体；目标已有的 BoxCollider 会被复用，子碰撞体不会被删除。
这是运行时按需计算方法，层级或模型发生变化后需要再次调用，不会每帧自动更新。

## UI 布局扩展迁移

从 `1.0.1` 开始，`UILayoutExtensions` 已迁移到 UI Framework `1.0.3`。类名、方法名和全局命名空间保持不变；需要使用布局重建扩展时，请安装 `com.cowart.ui-framework`。

## Highlight Plus Integration Sample

Package Manager 的 Samples 区域提供：

```text
Highlight Plus Integration
```

导入前必须先安装：

- Highlight Plus
- Event Relays（`com.cowart.event-relays`）

导入后增加：

- `ExtSetHighlight`：添加或获取 `HighlightEffect`，应用默认高亮样式并切换状态
- `ExtBindPointerDown`：绑定指针按下回调，可在首次触发前保持高亮并支持单次触发

```csharp
target.ExtSetHighlight(true);

target.ExtBindPointerDown(
    () => Debug.Log("目标被点击"),
    triggerOnce: true,
    highlightBeforeFirstTrigger: true);
```

Sample 未导入时不会参与编译，也不会要求项目安装 Highlight Plus。

## 主要扩展

| 扩展方法 | 说明 |
| --- | --- |
| `ExtLogItems` | 输出集合内容 |
| `ExtGetRandomItem` | 随机获取集合元素 |
| `ExtGetRandomIndex` | 随机获取有效索引 |
| `ExtShuffle` | 原地随机打乱列表 |
| `ExtGetWeightedRandom` | 按权重随机获取元素 |
| `ExtRemoveNullObjects` | 清理 Unity 空对象 |
| `ExtIndentLines` | 为多行文本添加缩进 |
| `ExtRemoveControlCharacters` | 移除控制字符 |
| `ExtRichBold` | 生成粗体富文本 |
| `ExtRichItalic` | 生成斜体富文本 |
| `ExtRichSize` | 生成字号富文本 |
| `ExtRichColor` | 使用字符串或 Color 生成颜色富文本 |
| `ExtSetActive` | 设置 Component 所在对象的激活状态 |
| `ExtCopyPoseFrom` | 复制位置和旋转 |
| `ExtLookAtHorizontal` | 水平朝向目标 |
| `ExtGetOrAddComponent` | 获取或添加组件 |
| `ExtAddFittedBoxCollider` | 添加或更新包裹完整子层级的 BoxCollider |
| `ExtResetLocal` | 重置本地 Transform |
| `ExtSetLayerRecursive` | 递归设置 Layer |
| `ExtFindChildRecursive` | 递归查找子节点 |
| `ExtGetHierarchyPath` | 获取完整层级路径 |
| `ExtWithX/Y/Z` | 修改 Vector 单个轴 |
| `ExtWithAlpha` | 修改 Color Alpha |
| `ExtContains` | 判断 LayerMask 是否包含 Layer |

## 环境要求

- Unity 2019.4 或更高版本
- Unity 3D Physics（通过包依赖自动引入）
- Highlight Plus，仅在导入对应 Sample 时需要
- Event Relays，仅在导入对应 Sample 时需要

## 许可证

本包使用 MIT License，详情参见仓库中的许可证文件。
