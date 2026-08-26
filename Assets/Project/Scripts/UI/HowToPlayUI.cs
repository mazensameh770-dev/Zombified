using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HowToPlayUI : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject parentUI;

    [Header("Fade & Animation")]
    [SerializeField] private FadeUI fadeUI;

    [Header("Text Fields")]
    [SerializeField] private TMP_Text row1TitleText;
    [SerializeField] private TMP_Text row1ContentText;
    [SerializeField] private TMP_Text row2TitleText;
    [SerializeField] private TMP_Text row2ContentText;
    [SerializeField] private TMP_Text row3TitleText;
    [SerializeField] private TMP_Text row3ContentText;

    private void Awake()
    {
        if (backButton == null)
            backButton = GetComponentInChildren<Button>(true);

        if (fadeUI == null)
            fadeUI = GetComponent<FadeUI>();

        if (row1ContentText == null || row2ContentText == null || row3ContentText == null)
        {
            TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>(true);
            foreach (TMP_Text t in allTexts)
            {
                if (t.gameObject.name == "Content" && t.transform.parent != null)
                {
                    string parentName = t.transform.parent.name;
                    if (parentName == "Row1") row1ContentText = t;
                    else if (parentName == "Row2") row2ContentText = t;
                    else if (parentName == "Row3") row3ContentText = t;
                }
                else if (t.gameObject.name == "Title" && t.transform.parent != null)
                {
                    string parentName = t.transform.parent.name;
                    if (parentName == "Row1") row1TitleText = t;
                    else if (parentName == "Row2") row2TitleText = t;
                    else if (parentName == "Row3") row3TitleText = t;
                }
            }
        }
    }

    private void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBack);
        }

        UpdateContentForPlatform();
    }

    private void OnEnable()
    {
        UpdateContentForPlatform();
    }

    public void Open(GameObject caller)
    {
        parentUI = caller;
        gameObject.SetActive(true);
    }

    private void GoBack()
    {
        if (fadeUI != null) fadeUI.Hide();
        else gameObject.SetActive(false);

        if (parentUI != null) parentUI.SetActive(true);
    }

    public void UpdateContentForPlatform()
    {
        bool isMobile = Application.isMobilePlatform;

        // --- ROW 1: Objective & Platform Controls ---
        if (row1TitleText != null)
            row1TitleText.text = "1. OBJECTIVE & CONTROLS";

        if (row1ContentText != null)
        {
            if (isMobile)
            {
                row1ContentText.text = "Eliminate all soldiers using traps before they escape!\n" +
                    "<color=#00E5FF>Controls (Touch):</color> Tap a card to select. <b>Touch & Drag</b> across the grid to preview range and validity in real-time. <b>Release</b> finger to place. <b>Tap placed trap</b> to remove.";
            }
            else
            {
                row1ContentText.text = "Eliminate all soldiers using traps before they escape!\n" +
                    "<color=#00E5FF>Controls (PC):</color> <b>Hover mouse</b> over tiles to preview range & validity. <b>Left-Click</b> to place trap, <b>Right-Click</b> to remove a placed trap. Press <b>ESC</b> to pause.";
            }
        }

        // --- ROW 2: Soldiers & Zombify Mechanic ---
        if (row2TitleText != null)
            row2TitleText.text = "2. SOLDIERS & ZOMBIFY";

        if (row2ContentText != null)
        {
            row2ContentText.text = "Soldiers attack approaching zombies with a range of <color=#FF5252><b>3 tiles</b></color>.\n" +
                "When you cast <color=#76FF03><b>Zombify</b></color>, a soldier transforms into a powerful Zombie with <color=#76FF03><b>4 HP</b></color> and an attack range of <color=#76FF03><b>4 tiles</b></color>!";
        }

        // --- ROW 3: Landmines & Explosive Barrels ---
        if (row3TitleText != null)
            row3TitleText.text = "3. LANDMINES & BARRELS";

        if (row3ContentText != null)
        {
            row3ContentText.text = "Landmines detonate when stepped on by a soldier.\n" +
                "<color=#FFB300><b>Chain Detonation:</b></color> Barrels <i>cannot</i> be placed directly in the soldier's path. Place barrels within a Landmine's blast radius so the mine's explosion ignites the barrel!";
        }
    }

    private void OnDestroy()
    {
        if (backButton != null)
            backButton.onClick.RemoveListener(GoBack);
    }
}
