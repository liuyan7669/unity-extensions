using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public sealed class ExtensionsTests
{
    [Test]
    public void CollectionSafetyAndMutation_WorkAsExpected()
    {
        List<int> values = new List<int> { 1, 2, 3 };
        Assert.IsFalse(values.ExtIsNullOrEmpty());
        Assert.AreEqual(2, values.ExtGetOrDefault(1));
        Assert.AreEqual(99, values.ExtGetOrDefault(10, 99));

        int count = values.Count;
        values.ExtShuffle();
        CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, values);
        _ = values.ExtPopRandom();
        Assert.AreEqual(count - 1, values.Count);
    }

    [Test]
    public void WeightedRandom_IgnoresInvalidWeights()
    {
        string[] values = { "zero", "only", "negative" };
        string result = values.ExtGetWeightedRandom(item => item == "only" ? 10f : item == "negative" ? -1f : 0f);
        Assert.AreEqual("only", result);
    }

    [Test]
    public void RemoveNullObjects_RemovesDestroyedUnityObjects()
    {
        GameObject live = new GameObject("Live");
        GameObject destroyed = new GameObject("Destroyed");
        List<GameObject> values = new List<GameObject> { live, destroyed, null };
        Object.DestroyImmediate(destroyed);

        Assert.AreEqual(2, values.ExtRemoveNullObjects());
        Assert.AreEqual(1, values.Count);
        Assert.AreSame(live, values[0]);
        Object.DestroyImmediate(live);
    }

    [Test]
    public void TransformHierarchyExtensions_WorkAsExpected()
    {
        GameObject root = new GameObject("Root");
        GameObject child = new GameObject("Child");
        child.transform.SetParent(root.transform);
        child.transform.localPosition = Vector3.one;
        child.transform.localRotation = Quaternion.Euler(10f, 20f, 30f);
        child.transform.localScale = Vector3.one * 2f;

        child.transform.ExtResetLocal();
        root.ExtSetLayerRecursive(7);

        Assert.AreEqual(Vector3.zero, child.transform.localPosition);
        Assert.AreEqual(Quaternion.identity, child.transform.localRotation);
        Assert.AreEqual(Vector3.one, child.transform.localScale);
        Assert.AreSame(child.transform, root.transform.ExtFindChildRecursive("Child"));
        Assert.AreEqual("Root/Child", child.ExtGetHierarchyPath());
        Assert.AreEqual(7, child.layer);
        Assert.AreEqual(1, root.transform.ExtClearChildren());
        Assert.AreEqual(0, root.transform.childCount);
        Object.DestroyImmediate(root);
    }

    [Test]
    public void VectorColorAndLayerMaskExtensions_WorkAsExpected()
    {
        Assert.AreEqual(new Vector3(5f, 2f, 3f), new Vector3(1f, 2f, 3f).ExtWithX(5f));
        Assert.AreEqual(new Vector2(1f, 5f), new Vector2(1f, 2f).ExtAddY(3f));

        Color color = new Color(1f, 0f, 0f, 1f).ExtWithAlpha(0.5f);
        Assert.AreEqual(0.5f, color.a, 0.0001f);
        Assert.AreEqual("FF000080", color.ExtToHex());
        Assert.IsTrue("00FF00".ExtTryParseColor(out Color parsed));
        Assert.AreEqual(Color.green, parsed);

        LayerMask mask = default;
        mask = mask.ExtAddLayer(7);
        Assert.IsTrue(mask.ExtContains(7));
        mask = mask.ExtRemoveLayer(7);
        Assert.IsFalse(mask.ExtContains(7));
    }
}
