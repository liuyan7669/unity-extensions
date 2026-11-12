using NUnit.Framework;
#if COWART_TMP
using TMPro;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Cowart.CwExtensions.Tests
{
    public sealed class UILayoutExtensionsTests
    {
        [Test]
        public void RectTransformSetters_OnlyChangeRequestedValues()
        {
            GameObject target = new GameObject("Rect", typeof(RectTransform));
            RectTransform rect = target.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(1f, 2f);
            rect.sizeDelta = new Vector2(100f, 200f);

            rect.CwSetAnchoredPositionX(5f);
            rect.CwSetAnchoredPositionY(6f);
            rect.CwSetSizeDeltaX(300f);
            rect.CwSetSizeDeltaY(400f);

            Assert.AreEqual(new Vector2(5f, 6f), rect.anchoredPosition);
            Assert.AreEqual(new Vector2(300f, 400f), rect.sizeDelta);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void CanvasGroupAndNavigation_AreSetTogether()
        {
            GameObject target = new GameObject("UI", typeof(RectTransform), typeof(CanvasGroup), typeof(Button));
            CanvasGroup group = target.GetComponent<CanvasGroup>();
            Button button = target.GetComponent<Button>();

            group.CwSetCanvasGroupState(0.25f, false, false);
            button.CwSetNavigationNone();

            Assert.AreEqual(0.25f, group.alpha, 0.0001f);
            Assert.IsFalse(group.interactable);
            Assert.IsFalse(group.blocksRaycasts);
            Assert.AreEqual(Navigation.Mode.None, button.navigation.mode);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPivotKeepPosition_PreservesWorldCorners()
        {
            GameObject parent = new GameObject("Parent", typeof(RectTransform));
            GameObject child = new GameObject("Child", typeof(RectTransform));
            RectTransform rect = child.GetComponent<RectTransform>();
            rect.SetParent(parent.transform, false);
            rect.sizeDelta = new Vector2(200f, 100f);
            rect.anchoredPosition = new Vector2(30f, 40f);
            Vector3[] before = new Vector3[4];
            Vector3[] after = new Vector3[4];
            rect.GetWorldCorners(before);

            rect.CwSetPivotKeepPosition(new Vector2(0f, 1f));
            rect.GetWorldCorners(after);

            for (int i = 0; i < before.Length; i++) Assert.AreEqual(before[i], after[i]);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TextExtensions_SetTextAndVisibility()
        {
            GameObject legacyObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
            Text legacyText = legacyObject.GetComponent<Text>();
            legacyText.CwSetText(string.Empty, true);
            Assert.IsFalse(legacyObject.activeSelf);
            legacyText.CwSetText("Legacy", true);
            Assert.IsTrue(legacyObject.activeSelf);
            Assert.AreEqual("Legacy", legacyText.text);
            Object.DestroyImmediate(legacyObject);

#if COWART_TMP
            GameObject tmpObject = new GameObject("TMP", typeof(RectTransform), typeof(TextMeshProUGUI));
            TMP_Text tmpText = tmpObject.GetComponent<TMP_Text>();
            tmpText.CwSetText(string.Empty, true);
            Assert.IsFalse(tmpObject.activeSelf);
            tmpText.CwSetText("TMP", true);
            Assert.IsTrue(tmpObject.activeSelf);
            Assert.AreEqual("TMP", tmpText.text);
            Object.DestroyImmediate(tmpObject);
#endif
        }

        [Test]
        public void GetRectAndSetAlpha_WorkAsExpected()
        {
            Assert.IsNull(((Component)null).CwGetRect());
            GameObject plain = new GameObject("Plain");
            Assert.IsNull(plain.transform.CwGetRect());

            GameObject target = new GameObject("Image", typeof(RectTransform), typeof(Image));
            Image image = target.GetComponent<Image>();
            Assert.AreSame(target.transform, image.CwGetRect());

            image.color = new Color(0.2f, 0.4f, 0.6f, 1f);
            image.CwSetAlpha(1.5f);
            Assert.AreEqual(1f, image.color.a, 0.0001f);
            image.CwSetAlpha(0.25f);
            Assert.AreEqual(new Color(0.2f, 0.4f, 0.6f, 0.25f), image.color);
            Assert.DoesNotThrow(() => ((Graphic)null).CwSetAlpha(0.5f));

            Object.DestroyImmediate(plain);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void RebuildLayoutTree_AppliesNestedLayoutImmediately()
        {
            GameObject root = new GameObject("Root", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            root.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
            root.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
            root.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            for (int i = 0; i < 3; i++)
            {
                GameObject item = new GameObject("Item" + i, typeof(RectTransform), typeof(LayoutElement));
                item.GetComponent<LayoutElement>().preferredHeight = 40f;
                item.transform.SetParent(root.transform, false);
            }

            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.CwRebuildLayoutTree();
            Assert.AreEqual(120f, rootRect.rect.height, 0.01f);

            Assert.DoesNotThrow(() => ((RectTransform)null).CwRebuildLayoutTree());
            Object.DestroyImmediate(root);
        }
    }
}
