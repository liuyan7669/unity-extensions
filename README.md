# Extensions

Unity 常用扩展方法集合，涵盖集合、字符串、GameObject、Component、Transform 和 UGUI 布局处理。

所有方法统一使用 `Ext` 前缀，便于在 IntelliSense 中快速搜索并与其他扩展方法区分。

## 功能

### 集合扩展

- 在一条日志中输出集合数量、索引和内容
- 从 `IReadOnlyList<T>` 随机获取元素
- 从 `IReadOnlyList<T>` 随机获取有效索引
- 对 null 和空集合进行安全处理

### 字符串扩展

- 为多行文本添加缩进
- 移除控制字符
- 可选择保留换行、制表等格式空白
- 同时移除空白和控制字符

### Unity 对象扩展

- 通过 Component 设置 GameObject 激活状态
- 复制 Transform 的位置和旋转
- 单独复制位置或旋转
- 仅在水平面朝向指定目标
- 为 GameObject 或 Component 获取或添加指定组件

### UGUI 布局扩展

- 按子节点到父节点的顺序立即重建布局树
- 下一帧重建目标附近的布局控制节点
- 支持 `LayoutGroup` 和 `ContentSizeFitter`

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
https://github.com/liuyan7669/unity-extensions.git#1.0.0
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

### Unity 对象操作

```csharp
targetComponent.ExtSetActive(true);

targetTransform.ExtCopyPoseFrom(referenceTransform);
targetTransform.ExtCopyPositionFrom(referenceTransform);
targetTransform.ExtCopyRotationFrom(referenceTransform);
targetTransform.ExtLookAtHorizontal(referenceTransform);
```

### 获取或添加组件

```csharp
AudioSource audioSource = gameObject.ExtGetOrAddComponent<AudioSource>();
Rigidbody body = transform.ExtGetOrAddComponent<Rigidbody>();
```

目标为 null 时返回 null；目标已包含组件时直接返回已有组件。

### 重建 UGUI 布局

```csharp
RectTransform panel = GetComponent<RectTransform>();

panel.ExtRebuildLayoutTree();
panel.ExtRebuildNearestLayoutNextFrame(this);
```

延迟重建需要一个有效的 `MonoBehaviour` 启动协程；不传参数时会从目标父级自动查找。

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
| `ExtIndentLines` | 为多行文本添加缩进 |
| `ExtRemoveControlCharacters` | 移除控制字符 |
| `ExtSetActive` | 设置 Component 所在对象的激活状态 |
| `ExtCopyPoseFrom` | 复制位置和旋转 |
| `ExtLookAtHorizontal` | 水平朝向目标 |
| `ExtGetOrAddComponent` | 获取或添加组件 |
| `ExtRebuildLayoutTree` | 立即重建布局树 |
| `ExtRebuildNearestLayoutNextFrame` | 下一帧重建附近布局 |

## 环境要求

- Unity 2021.3 或更高版本
- Unity UI（`com.unity.ugui`）
- Highlight Plus，仅在导入对应 Sample 时需要
- Event Relays，仅在导入对应 Sample 时需要

## 许可证

本包使用 MIT License，详情参见仓库中的许可证文件。
