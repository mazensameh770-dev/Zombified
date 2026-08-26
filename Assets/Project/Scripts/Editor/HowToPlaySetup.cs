#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[InitializeOnLoad]
public static class HowToPlaySetup
{
    static HowToPlaySetup()
    {
        EditorApplication.delayCall += () =>
        {
            SetupActiveScene();
        };
    }

    [MenuItem("Tools/Setup How To Play UI")]
    public static void SetupActiveScene()
    {
        MainMenuUI mainMenuUI = Object.FindAnyObjectByType<MainMenuUI>();
        if (mainMenuUI == null)
        {
            return;
        }

        Canvas canvas = mainMenuUI.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("[HowToPlaySetup] Canvas not found.");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(canvas.gameObject, "Setup How To Play UI");

        // Load Assets
        TMP_FontAsset creepsterFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Fonts/Creepster/Creepster-Regular SDF.asset");
        Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/HowToPlay_Background.jpg");
        Sprite buttonSprite = null;

        // Find Reset button to copy styling
        Transform resetTransform = mainMenuUI.transform.Find("Reset");
        Button sourceButton = null;
        Image sourceImage = null;
        if (resetTransform != null)
        {
            sourceButton = resetTransform.GetComponentInChildren<Button>(true);
            if (sourceButton != null)
            {
                sourceImage = sourceButton.GetComponent<Image>();
                if (sourceImage != null) buttonSprite = sourceImage.sprite;
            }
        }

        // 1. Create / Update "HowToPlay" Button under MainMenuUI
        Transform howToPlayTransform = mainMenuUI.transform.Find("HowToPlay");
        GameObject howToPlayObj;
        if (howToPlayTransform == null)
        {
            howToPlayObj = new GameObject("HowToPlay", typeof(RectTransform));
            howToPlayObj.transform.SetParent(mainMenuUI.transform, false);
        }
        else
        {
            howToPlayObj = howToPlayTransform.gameObject;
        }

        RectTransform htpRect = howToPlayObj.GetComponent<RectTransform>();
        htpRect.anchorMin = new Vector2(0.5f, 0.5f);
        htpRect.anchorMax = new Vector2(0.5f, 0.5f);
        htpRect.anchoredPosition = new Vector2(785f, 458f); // Mirrored from Reset (-785, 458)
        htpRect.sizeDelta = new Vector2(300f, 135f);
        htpRect.localScale = Vector3.one;

        // UIAnimation on container (mirrored entry from right side)
        UIAnimation htpAnim = howToPlayObj.GetComponent<UIAnimation>();
        if (htpAnim == null) htpAnim = howToPlayObj.AddComponent<UIAnimation>();
        SerializedObject animSO = new SerializedObject(htpAnim);
        animSO.FindProperty("animationType").intValue = 6; // SlidingLeft (slides in from right side)
        animSO.FindProperty("animationTime").floatValue = 1f;
        animSO.FindProperty("delay").floatValue = 0.5f;
        animSO.ApplyModifiedProperties();

        // Button GameObject
        Transform btnTransform = howToPlayObj.transform.Find("HowToPlayButton");
        GameObject btnObj;
        if (btnTransform == null)
        {
            btnObj = new GameObject("HowToPlayButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(Outline), typeof(HoverAnimation), typeof(UIButtonSound));
            btnObj.transform.SetParent(howToPlayObj.transform, false);
        }
        else
        {
            btnObj = btnTransform.gameObject;
        }

        RectTransform btnRect = btnObj.GetComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = Vector2.zero;
        btnRect.sizeDelta = new Vector2(300f, 135f);
        btnRect.localScale = Vector3.one;

        Image btnImage = btnObj.GetComponent<Image>();
        if (buttonSprite != null) btnImage.sprite = buttonSprite;
        btnImage.color = sourceImage != null ? sourceImage.color : new Color(1f, 0f, 0f, 1f);

        Outline outline = btnObj.GetComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(3f, 3f);

        HoverAnimation hover = btnObj.GetComponent<HoverAnimation>();
        SerializedObject hoverSO = new SerializedObject(hover);
        hoverSO.FindProperty("rotate").boolValue = true;
        hoverSO.FindProperty("rotateValue").floatValue = 3f;
        hoverSO.FindProperty("scale").boolValue = true;
        hoverSO.FindProperty("scaleValue").floatValue = 1.1f;
        hoverSO.FindProperty("animationTime").floatValue = 0.3f;
        hoverSO.ApplyModifiedProperties();

        Button howToPlayButton = btnObj.GetComponent<Button>();
        howToPlayButton.targetGraphic = btnImage;

        // Button Text (TMP)
        Transform textTransform = btnObj.transform.Find("Text (TMP)");
        GameObject textObj;
        if (textTransform == null)
        {
            textObj = new GameObject("Text (TMP)", typeof(RectTransform), typeof(TextMeshProUGUI));
            textObj.transform.SetParent(btnObj.transform, false);
        }
        else
        {
            textObj = textTransform.gameObject;
        }

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
        tmp.text = "How to Play";
        if (creepsterFont != null) tmp.font = creepsterFont;
        tmp.fontSize = 32f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        // 2. Create / Update HowToPlayUI Popup under Canvas
        Transform htpUITransform = canvas.transform.Find("HowToPlayUI");
        GameObject htpUIObj;
        if (htpUITransform == null)
        {
            htpUIObj = new GameObject("HowToPlayUI", typeof(RectTransform), typeof(CanvasGroup), typeof(FadeUI), typeof(UIAnimation), typeof(HowToPlayUI));
            htpUIObj.transform.SetParent(canvas.transform, false);
            htpUIObj.transform.SetSiblingIndex(mainMenuUI.transform.GetSiblingIndex() + 1);
        }
        else
        {
            htpUIObj = htpUITransform.gameObject;
        }

        RectTransform htpUIRect = htpUIObj.GetComponent<RectTransform>();
        htpUIRect.anchorMin = Vector2.zero;
        htpUIRect.anchorMax = Vector2.one;
        htpUIRect.anchoredPosition = Vector2.zero;
        htpUIRect.sizeDelta = Vector2.zero;
        htpUIRect.localScale = Vector3.one;

        FadeUI fadeUI = htpUIObj.GetComponent<FadeUI>();
        SerializedObject fadeSO = new SerializedObject(fadeUI);
        fadeSO.FindProperty("fadeDuration").floatValue = 0.15f;
        fadeSO.ApplyModifiedProperties();

        UIAnimation popupAnim = htpUIObj.GetComponent<UIAnimation>();
        SerializedObject popupAnimSO = new SerializedObject(popupAnim);
        popupAnimSO.FindProperty("animationType").intValue = 1; // ScaleUp
        popupAnimSO.FindProperty("animationTime").floatValue = 0.5f;
        popupAnimSO.FindProperty("delay").floatValue = 0f;
        popupAnimSO.ApplyModifiedProperties();

        HowToPlayUI howToPlayUIComponent = htpUIObj.GetComponent<HowToPlayUI>();

        // Background Frame Panel
        Transform panelTransform = htpUIObj.transform.Find("Panel");
        GameObject panelObj;
        if (panelTransform == null)
        {
            panelObj = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            panelObj.transform.SetParent(htpUIObj.transform, false);
        }
        else
        {
            panelObj = panelTransform.gameObject;
        }

        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(1500f, 920f);
        panelRect.localScale = Vector3.one;

        Image panelImage = panelObj.GetComponent<Image>();
        if (bgSprite != null) panelImage.sprite = bgSprite;
        panelImage.color = Color.white;

        // Title TMP
        Transform titleTransform = panelObj.transform.Find("Title");
        GameObject titleObj;
        if (titleTransform == null)
        {
            titleObj = new GameObject("Title", typeof(RectTransform), typeof(TextMeshProUGUI));
            titleObj.transform.SetParent(panelObj.transform, false);
        }
        else
        {
            titleObj = titleTransform.gameObject;
        }

        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = new Vector2(0f, 310f);
        titleRect.sizeDelta = new Vector2(900f, 90f);
        TextMeshProUGUI titleTMP = titleObj.GetComponent<TextMeshProUGUI>();
        titleTMP.text = "HOW TO PLAY";
        if (creepsterFont != null) titleTMP.font = creepsterFont;
        titleTMP.fontSize = 58f;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.color = new Color(1f, 0.88f, 0.2f, 1f); // Gold

        // Helper to create Rows
        TMP_Text r1Title, r1Content, r2Title, r2Content, r3Title, r3Content;
        CreateRow(panelObj.transform, "Row1", new Vector2(0f, 160f), new Vector2(1150f, 140f), creepsterFont, out r1Title, out r1Content);
        CreateRow(panelObj.transform, "Row2", new Vector2(0f, 15f), new Vector2(1150f, 130f), creepsterFont, out r2Title, out r2Content);
        CreateRow(panelObj.transform, "Row3", new Vector2(0f, -125f), new Vector2(1150f, 130f), creepsterFont, out r3Title, out r3Content);

        // Back Button
        Transform backTransform = panelObj.transform.Find("BackButton");
        GameObject backObj;
        if (backTransform == null)
        {
            backObj = new GameObject("BackButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(Outline), typeof(HoverAnimation), typeof(UIButtonSound));
            backObj.transform.SetParent(panelObj.transform, false);
        }
        else
        {
            backObj = backTransform.gameObject;
        }

        RectTransform backRect = backObj.GetComponent<RectTransform>();
        backRect.anchorMin = new Vector2(0.5f, 0.5f);
        backRect.anchorMax = new Vector2(0.5f, 0.5f);
        backRect.anchoredPosition = new Vector2(0f, -305f);
        backRect.sizeDelta = new Vector2(340f, 110f);
        backRect.localScale = Vector3.one;

        Image backImage = backObj.GetComponent<Image>();
        if (buttonSprite != null) backImage.sprite = buttonSprite;
        backImage.color = sourceImage != null ? sourceImage.color : new Color(1f, 0f, 0f, 1f);

        Outline backOutline = backObj.GetComponent<Outline>();
        backOutline.effectColor = Color.black;
        backOutline.effectDistance = new Vector2(3f, 3f);

        HoverAnimation backHover = backObj.GetComponent<HoverAnimation>();
        SerializedObject backHoverSO = new SerializedObject(backHover);
        backHoverSO.FindProperty("rotate").boolValue = true;
        backHoverSO.FindProperty("rotateValue").floatValue = 3f;
        backHoverSO.FindProperty("scale").boolValue = true;
        backHoverSO.FindProperty("scaleValue").floatValue = 1.1f;
        backHoverSO.FindProperty("animationTime").floatValue = 0.3f;
        backHoverSO.ApplyModifiedProperties();

        Button backButton = backObj.GetComponent<Button>();
        backButton.targetGraphic = backImage;

        Transform backTextTransform = backObj.transform.Find("Text (TMP)");
        GameObject backTextObj;
        if (backTextTransform == null)
        {
            backTextObj = new GameObject("Text (TMP)", typeof(RectTransform), typeof(TextMeshProUGUI));
            backTextObj.transform.SetParent(backObj.transform, false);
        }
        else
        {
            backTextObj = backTextTransform.gameObject;
        }

        RectTransform backTextRect = backTextObj.GetComponent<RectTransform>();
        backTextRect.anchorMin = Vector2.zero;
        backTextRect.anchorMax = Vector2.one;
        backTextRect.sizeDelta = Vector2.zero;
        backTextRect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI backTMP = backTextObj.GetComponent<TextMeshProUGUI>();
        backTMP.text = "Back";
        if (creepsterFont != null) backTMP.font = creepsterFont;
        backTMP.fontSize = 40f;
        backTMP.alignment = TextAlignmentOptions.Center;
        backTMP.color = Color.white;

        // Wire references in HowToPlayUI
        SerializedObject htpSO = new SerializedObject(howToPlayUIComponent);
        htpSO.FindProperty("backButton").objectReferenceValue = backButton;
        htpSO.FindProperty("parentUI").objectReferenceValue = mainMenuUI.gameObject;
        htpSO.FindProperty("fadeUI").objectReferenceValue = fadeUI;
        htpSO.FindProperty("row1TitleText").objectReferenceValue = r1Title;
        htpSO.FindProperty("row1ContentText").objectReferenceValue = r1Content;
        htpSO.FindProperty("row2TitleText").objectReferenceValue = r2Title;
        htpSO.FindProperty("row2ContentText").objectReferenceValue = r2Content;
        htpSO.FindProperty("row3TitleText").objectReferenceValue = r3Title;
        htpSO.FindProperty("row3ContentText").objectReferenceValue = r3Content;
        htpSO.ApplyModifiedProperties();

        // Wire references in MainMenuUI
        SerializedObject mmSO = new SerializedObject(mainMenuUI);
        mmSO.FindProperty("howToPlayButton").objectReferenceValue = howToPlayButton;
        mmSO.FindProperty("howToPlayUI").objectReferenceValue = howToPlayUIComponent;
        mmSO.ApplyModifiedProperties();

        // Initially deactivate HowToPlayUI popup
        htpUIObj.SetActive(false);

        howToPlayUIComponent.UpdateContentForPlatform();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("<color=green>[HowToPlaySetup] Successfully created and wired How To Play UI in scene!</color>");
    }

    private static void CreateRow(Transform parent, string name, Vector2 pos, Vector2 size, TMP_FontAsset font, out TMP_Text titleTMP, out TMP_Text contentTMP)
    {
        Transform rowTransform = parent.Find(name);
        GameObject rowObj;
        if (rowTransform == null)
        {
            rowObj = new GameObject(name, typeof(RectTransform));
            rowObj.transform.SetParent(parent, false);
        }
        else
        {
            rowObj = rowTransform.gameObject;
        }

        RectTransform rowRect = rowObj.GetComponent<RectTransform>();
        rowRect.anchorMin = new Vector2(0.5f, 0.5f);
        rowRect.anchorMax = new Vector2(0.5f, 0.5f);
        rowRect.anchoredPosition = pos;
        rowRect.sizeDelta = size;
        rowRect.localScale = Vector3.one;

        // Title
        Transform titleT = rowObj.transform.Find("Title");
        GameObject titleO = titleT == null ? new GameObject("Title", typeof(RectTransform), typeof(TextMeshProUGUI)) : titleT.gameObject;
        titleO.transform.SetParent(rowObj.transform, false);
        RectTransform tRect = titleO.GetComponent<RectTransform>();
        tRect.anchorMin = new Vector2(0f, 1f);
        tRect.anchorMax = new Vector2(1f, 1f);
        tRect.pivot = new Vector2(0.5f, 1f);
        tRect.anchoredPosition = new Vector2(0f, 0f);
        tRect.sizeDelta = new Vector2(0f, 38f);
        TextMeshProUGUI tTMP = titleO.GetComponent<TextMeshProUGUI>();
        if (font != null) tTMP.font = font;
        tTMP.fontSize = 28f;
        tTMP.alignment = TextAlignmentOptions.Left;
        tTMP.color = new Color(1f, 0.84f, 0f, 1f); // Gold
        titleTMP = tTMP;

        // Content
        Transform contentT = rowObj.transform.Find("Content");
        GameObject contentO = contentT == null ? new GameObject("Content", typeof(RectTransform), typeof(TextMeshProUGUI)) : contentT.gameObject;
        contentO.transform.SetParent(rowObj.transform, false);
        RectTransform cRect = contentO.GetComponent<RectTransform>();
        cRect.anchorMin = new Vector2(0f, 0f);
        cRect.anchorMax = new Vector2(1f, 1f);
        cRect.pivot = new Vector2(0.5f, 0.5f);
        cRect.anchoredPosition = new Vector2(0f, -18f);
        cRect.sizeDelta = new Vector2(0f, -38f);
        TextMeshProUGUI cTMP = contentO.GetComponent<TextMeshProUGUI>();
        cTMP.fontSize = 21f;
        cTMP.enableWordWrapping = true;
        cTMP.alignment = TextAlignmentOptions.TopLeft;
        cTMP.color = Color.white;
        contentTMP = cTMP;
    }
}
#endif
