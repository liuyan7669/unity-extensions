using System.Collections.Generic;
using Cowart.Extensions;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// 验证 Extensions 包的集合、Unity 对象和值类型扩展。
/// </summary>
public sealed class ExtensionsTests
{
    /// <summary>验证集合空值保护、索引读取和随机变更操作。</summary>
    [Test]
    public void CollectionSafetyAndMutation_WorkAsExpected()
    {
        List<int> values = new List<int> { 1, 2, 3 };
        Assert.IsFalse(values.CwIsNullOrEmpty());
        Assert.AreEqual(2, values.CwGetOrDefault(1));
        Assert.AreEqual(99, values.CwGetOrDefault(10, 99));

        int count = values.Count;
        values.CwShuffle();
        CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, values);
        _ = values.CwPopRandom();
        Assert.AreEqual(count - 1, values.Count);
    }

    /// <summary>验证加权随机会忽略零权重和负权重元素。</summary>
    [Test]
    public void WeightedRandom_IgnoresInvalidWeights()
    {
        string[] values = { "zero", "only", "negative" };
        string result = values.CwGetWeightedRandom(item => item == "only" ? 10f : item == "negative" ? -1f : 0f);
        Assert.AreEqual("only", result);
    }

    /// <summary>验证集合能够移除 null 和已经销毁的 Unity 对象。</summary>
    [Test]
    public void RemoveNullObjects_RemovesDestroyedUnityObjects()
    {
        GameObject live = new GameObject("Live");
        GameObject destroyed = new GameObject("Destroyed");
        List<GameObject> values = new List<GameObject> { live, destroyed, null };
        Object.DestroyImmediate(destroyed);

        Assert.AreEqual(2, values.CwRemoveNullObjects());
        Assert.AreEqual(1, values.Count);
        Assert.AreSame(live, values[0]);
        Object.DestroyImmediate(live);
    }

    /// <summary>验证 Transform 层级相关扩展的主要行为。</summary>
    [Test]
    public void TransformHierarchyExtensions_WorkAsExpected()
    {
        GameObject root = new GameObject("Root");
        GameObject child = new GameObject("Child");
        child.transform.SetParent(root.transform);
        child.transform.localPosition = Vector3.one;
        child.transform.localRotation = Quaternion.Euler(10f, 20f, 30f);
        child.transform.localScale = Vector3.one * 2f;

        child.transform.CwResetLocal();
        root.CwSetLayerRecursive(7);

        Assert.AreEqual(Vector3.zero, child.transform.localPosition);
        Assert.AreEqual(Quaternion.identity, child.transform.localRotation);
        Assert.AreEqual(Vector3.one, child.transform.localScale);
        Assert.AreSame(child.transform, root.transform.CwFindChildRecursive("Child"));
        Assert.AreEqual("Root/Child", child.CwGetHierarchyPath());
        Assert.AreEqual(7, child.layer);
        Assert.AreEqual(0, root.transform.CwClearChildren());
        Assert.AreEqual(1, root.transform.childCount);
        Object.DestroyImmediate(root);
    }

    /// <summary>验证批量添加组件以及编辑模式下禁止移除组件。</summary>
    [Test]
    public void AddAndRemoveComponentExtensions_WorkAsExpected()
    {
        GameObject target = new GameObject("Target");

        Component[] added = target.CwAddComponents(
            typeof(AudioSource),
            typeof(BoxCollider),
            typeof(BoxCollider),
            typeof(BoxCollider));

        Assert.AreEqual(4, added.Length);
        Assert.IsInstanceOf<AudioSource>(added[0]);
        Assert.IsInstanceOf<BoxCollider>(added[1]);
        Assert.IsInstanceOf<BoxCollider>(added[2]);
        Assert.IsInstanceOf<BoxCollider>(added[3]);
        Assert.AreEqual(3, target.GetComponents<BoxCollider>().Length);

        Assert.AreEqual(0, target.transform.CwRemoveComponent<BoxCollider>(removeAll: false));
        Assert.AreEqual(3, target.GetComponents<BoxCollider>().Length);
        Assert.AreEqual(0, target.CwRemoveComponent<BoxCollider>());
        Assert.AreEqual(3, target.GetComponents<BoxCollider>().Length);

        Object.DestroyImmediate(target);
    }

    /// <summary>验证批量添加组件会在修改对象前拒绝非法类型。</summary>
    [Test]
    public void AddComponents_RejectsInvalidTypesBeforeAddingAnything()
    {
        GameObject target = new GameObject("Target");

        Assert.Throws<System.ArgumentException>(() =>
            target.CwAddComponents(typeof(AudioSource), typeof(string)));
        Assert.Throws<System.ArgumentException>(() =>
            target.CwGetOrAddComponent(typeof(string)));
        Assert.IsNull(target.GetComponent<AudioSource>());

        Object.DestroyImmediate(target);
    }

    /// <summary>验证组件获取、自动添加、必需组件和父子级 Try 查询。</summary>
    [Test]
    public void ComponentLookupExtensions_WorkAsExpected()
    {
        GameObject root = new GameObject("Root");
        GameObject child = new GameObject("Child");
        child.transform.SetParent(root.transform);

        AudioSource first = root.CwGetOrAddComponent<AudioSource>(out bool firstAdded);
        AudioSource second = root.CwGetOrAddComponent<AudioSource>(out bool secondAdded);
        Component boxCollider = root.transform.CwGetOrAddComponent(typeof(BoxCollider));
        SphereCollider sphereCollider = child.AddComponent<SphereCollider>();

        Assert.IsTrue(firstAdded);
        Assert.IsFalse(secondAdded);
        Assert.AreSame(first, second);
        Assert.IsInstanceOf<BoxCollider>(boxCollider);
        Assert.AreSame(first, root.CwGetComponentOrThrow<AudioSource>());
        Assert.Throws<System.InvalidOperationException>(() =>
            root.CwGetComponentOrThrow<Rigidbody>());

        Assert.IsTrue(root.CwTryGetComponentInChildren(
            out SphereCollider foundChildCollider,
            includeInactive: true));
        Assert.AreSame(sphereCollider, foundChildCollider);
        Assert.IsTrue(child.transform.CwTryGetComponentInParent(
            out AudioSource foundParentAudio,
            includeInactive: true));
        Assert.AreSame(first, foundParentAudio);

        Object.DestroyImmediate(root);
    }

    /// <summary>验证本地 Transform 复制、分项重置、单轴缩放和设置父节点。</summary>
    [Test]
    public void TransformConvenienceExtensions_WorkAsExpected()
    {
        GameObject parent = new GameObject("Parent");
        GameObject target = new GameObject("Target");
        GameObject reference = new GameObject("Reference");
        reference.transform.SetParent(parent.transform);

        target.transform.position = new Vector3(10f, 20f, 30f);
        target.transform.rotation = Quaternion.Euler(15f, 25f, 35f);
        target.transform.localScale = Vector3.one * 3f;
        target.transform.CwSetParentAndResetLocal(parent.transform);

        Assert.AreSame(parent.transform, target.transform.parent);
        Assert.AreEqual(Vector3.zero, target.transform.localPosition);
        Assert.AreEqual(Quaternion.identity, target.transform.localRotation);
        Assert.AreEqual(Vector3.one, target.transform.localScale);

        reference.transform.localPosition = new Vector3(1f, 2f, 3f);
        reference.transform.localRotation = Quaternion.Euler(5f, 10f, 15f);
        reference.transform.localScale = new Vector3(2f, 3f, 4f);
        target.transform.CwCopyLocalTransformFrom(reference.transform);

        Assert.AreEqual(reference.transform.localPosition, target.transform.localPosition);
        Assert.AreEqual(reference.transform.localRotation, target.transform.localRotation);
        Assert.AreEqual(reference.transform.localScale, target.transform.localScale);

        target.transform.CwSetLocalScaleX(5f);
        target.transform.CwSetLocalScaleY(6f);
        target.transform.CwSetLocalScaleZ(7f);
        Assert.AreEqual(new Vector3(5f, 6f, 7f), target.transform.localScale);

        target.transform.CwResetLocalPosition();
        target.transform.CwResetLocalRotation();
        target.transform.CwResetLocalScale();
        Assert.AreEqual(Vector3.zero, target.transform.localPosition);
        Assert.AreEqual(Quaternion.identity, target.transform.localRotation);
        Assert.AreEqual(Vector3.one, target.transform.localScale);

        Object.DestroyImmediate(parent);
    }

    /// <summary>验证激活状态切换、直接子物体控制和编辑模式删除保护。</summary>
    [Test]
    public void ActiveStateAndDestroyExtensions_WorkAsExpected()
    {
        GameObject root = new GameObject("Root");
        GameObject firstChild = new GameObject("FirstChild");
        GameObject secondChild = new GameObject("SecondChild");
        GameObject grandchild = new GameObject("Grandchild");
        firstChild.transform.SetParent(root.transform);
        secondChild.transform.SetParent(root.transform);
        grandchild.transform.SetParent(firstChild.transform);

        Assert.AreEqual(2, root.transform.CwSetChildrenActive(false));
        Assert.IsFalse(firstChild.activeSelf);
        Assert.IsFalse(secondChild.activeSelf);
        Assert.IsTrue(grandchild.activeSelf);

        Assert.IsFalse(root.CwToggleActive());
        Assert.IsTrue(root.transform.CwToggleActive());

        GameObject destroyed = new GameObject("Destroyed");
        Assert.IsFalse(destroyed.CwDestroy());
        Assert.IsNotNull(destroyed);

        GameObject nullTarget = null;
        Assert.IsFalse(nullTarget.CwDestroy());

        Object.DestroyImmediate(destroyed);
        Object.DestroyImmediate(root);
    }

    /// <summary>验证拟合 BoxCollider 能覆盖激活、未激活、嵌套和旋转缩放后的子物体。</summary>
    [Test]
    public void AddFittedBoxCollider_FitsCompleteHierarchy()
    {
        GameObject root = new GameObject("Root");
        GameObject first = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject second = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject container = new GameObject("Container");
        GameObject nested = GameObject.CreatePrimitive(PrimitiveType.Cube);

        first.transform.SetParent(root.transform, false);
        first.transform.localPosition = new Vector3(2f, 0f, 0f);
        first.transform.localScale = new Vector3(2f, 2f, 2f);

        second.transform.SetParent(root.transform, false);
        second.transform.localPosition = new Vector3(-2f, 1f, 0f);
        second.transform.localScale = new Vector3(2f, 4f, 2f);
        second.SetActive(false);

        container.transform.SetParent(root.transform, false);
        container.transform.localPosition = new Vector3(0f, 0f, 1f);
        nested.transform.SetParent(container.transform, false);
        nested.transform.localPosition = new Vector3(0f, 0f, 3f);
        nested.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        nested.transform.localScale = new Vector3(2f, 4f, 6f);

        BoxCollider activeOnlyCollider = root.CwAddFittedBoxCollider(includeInactive: false);
        Assert.IsNotNull(activeOnlyCollider);
        AssertVector3Approximately(new Vector3(0f, 0f, 2f), activeOnlyCollider.center);
        AssertVector3Approximately(new Vector3(6f, 4f, 6f), activeOnlyCollider.size);

        BoxCollider completeCollider = root.transform.CwAddFittedBoxCollider(includeInactive: true);
        Assert.AreSame(activeOnlyCollider, completeCollider);
        Assert.AreEqual(1, root.GetComponents<BoxCollider>().Length);
        AssertVector3Approximately(new Vector3(0f, 0.5f, 2f), completeCollider.center);
        AssertVector3Approximately(new Vector3(6f, 5f, 6f), completeCollider.size);

        Object.DestroyImmediate(root);
    }

    /// <summary>验证没有 Renderer、子 Collider 或子节点时不会创建 BoxCollider。</summary>
    [Test]
    public void AddFittedBoxCollider_EmptyHierarchyDoesNotAddComponent()
    {
        GameObject target = new GameObject("Target");

        Assert.IsNull(target.CwAddFittedBoxCollider());
        Assert.IsNull(target.GetComponent<BoxCollider>());

        Object.DestroyImmediate(target);
    }

    /// <summary>验证 Vector、Color 和 LayerMask 值扩展。</summary>
    [Test]
    public void VectorColorAndLayerMaskExtensions_WorkAsExpected()
    {
        Assert.AreEqual(new Vector3(5f, 2f, 3f), new Vector3(1f, 2f, 3f).CwWithX(5f));
        Assert.AreEqual(new Vector2(1f, 5f), new Vector2(1f, 2f).CwAddY(3f));

        Color color = new Color(1f, 0f, 0f, 1f).CwWithAlpha(0.5f);
        Assert.AreEqual(0.5f, color.a, 0.0001f);
        Assert.AreEqual("FF000080", color.CwToHex());
        Assert.IsTrue("00FF00".CwTryParseColor(out Color parsed));
        Assert.AreEqual(Color.green, parsed);

        LayerMask mask = default;
        mask = mask.CwAddLayer(7);
        Assert.IsTrue(mask.CwContains(7));
        mask = mask.CwRemoveLayer(7);
        Assert.IsFalse(mask.CwContains(7));
    }

    /// <summary>验证层级局部包围盒的空值保护和子物体合并结果。</summary>
    [Test]
    public void HierarchyLocalBounds_WorkAsExpected()
    {
        Assert.IsFalse(((Transform)null).CwTryGetHierarchyLocalBounds(out _));

        GameObject root = new GameObject("Root");
        Assert.IsFalse(root.transform.CwTryGetHierarchyLocalBounds(out _));

        GameObject child = new GameObject("Child");
        child.transform.SetParent(root.transform, false);
        child.transform.localPosition = new Vector3(2f, 0f, 0f);
        BoxCollider childCollider = child.AddComponent<BoxCollider>();
        childCollider.size = Vector3.one;

        Assert.IsTrue(root.transform.CwTryGetHierarchyLocalBounds(out Bounds bounds));
        AssertVector3Approximately(new Vector3(2f, 0f, 0f), bounds.center);
        AssertVector3Approximately(Vector3.one, bounds.size);

        Object.DestroyImmediate(root);
    }

    /// <summary>验证点击回调的添加、替换、取消和空值保护。</summary>
    [Test]
    public void ClickAction_AddsReplacesAndDisables()
    {
        Assert.DoesNotThrow(() => ((GameObject)null).CwSetClickAction(() => { }));

        GameObject target = new GameObject("Clickable");
        target.CwSetClickAction(null);
        Assert.AreEqual(0, target.GetComponents<MonoBehaviour>().Length);

        target.CwSetClickAction(() => { });
        target.CwSetClickAction(() => { });
        MonoBehaviour[] behaviours = target.GetComponents<MonoBehaviour>();
        Assert.AreEqual(1, behaviours.Length);
        Assert.IsTrue(behaviours[0].enabled);

        target.CwSetClickAction(null);
        Assert.IsFalse(behaviours[0].enabled);

        Object.DestroyImmediate(target);
    }

    /// <summary>按允许的浮点误差比较两个 Vector3。</summary>
    /// <param name="expected">期望值。</param>
    /// <param name="actual">实际值。</param>
    private static void AssertVector3Approximately(Vector3 expected, Vector3 actual)
    {
        Assert.AreEqual(expected.x, actual.x, 0.0001f);
        Assert.AreEqual(expected.y, actual.y, 0.0001f);
        Assert.AreEqual(expected.z, actual.z, 0.0001f);
    }
}
