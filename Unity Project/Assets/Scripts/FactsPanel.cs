using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FactsPanel : MonoBehaviour
{
    // ── Inspector Fields ─────────────────────────────────────────────────
    [Header("UI References")]
    [SerializeField] private GameObject factsPanel;      // the panel background
    [SerializeField] private TMP_Text factTitleText;     // shows "Did You Know?"
    [SerializeField] private TMP_Text factBodyText;      // shows the fact text
    [SerializeField] private TMP_Text factCounterText;   // shows "1 / 10"
    [SerializeField] private TMP_Text toggleHintText;    // shows "Press F to open facts"

    [Header("Buttons")]
    [SerializeField] private Button nextButton;          // next fact button
    [SerializeField] private Button prevButton;          // previous fact button
    [SerializeField] private Button closeButton;         // close panel button

    [Header("Settings")]
    [SerializeField] private bool showOnStart    = true;  // show at game start?
    [SerializeField] private float autoShowDelay = 2f;    // delay before showing

    // ── Solar Energy Facts ────────────────────────────────────────────────
    private string[] facts = new string[]
    {
        "Solar panels can last 25 to 30 years before needing replacement.",

        "One solar panel can offset approximately 1 tonne of CO2 emissions every year.",

        "The sun produces enough energy in one hour to power the entire Earth for a full year.",

        "Solar energy is the most abundant energy source on Earth — it is completely free and renewable.",

        "A typical home solar system reduces carbon emissions by 3 to 4 tonnes per year.",

        "Solar panels work even on cloudy days — they just produce less electricity than on sunny days.",

        "The first solar cell was invented in 1954 by Bell Laboratories in the United States.",

        "Countries like Germany, China and the USA generate millions of megawatts using solar energy every year.",

        "Using solar energy instead of fossil fuels helps reduce air pollution and protect human health.",

        "If the entire Sahara Desert was covered in solar panels it could power the whole world 7 times over.",

        "Solar energy produces zero noise pollution — unlike wind turbines or generators.",

        "Installing solar panels can reduce a household electricity bill by up to 70 percent.",

        "Electric vehicles charged using solar energy produce zero carbon emissions from start to finish.",

        "Nepal, India and Bangladesh have used solar energy to bring electricity to remote villages.",

        "Bees and other pollinators thrive in solar farms — making them good for biodiversity too."
    };

    // ── Private ───────────────────────────────────────────────────────────
    private int  currentIndex  = 0;
    private bool isPanelOpen   = false;

    // ── Start ─────────────────────────────────────────────────────────────
    void Start()
    {
        // connect buttons
        if (nextButton  != null) nextButton.onClick.AddListener(ShowNextFact);
        if (prevButton  != null) prevButton.onClick.AddListener(ShowPrevFact);
        if (closeButton != null) closeButton.onClick.AddListener(ClosePanel);

        // hide panel at start
        ClosePanel();

        // show hint text
        if (toggleHintText != null)
            toggleHintText.text = "Press F to open Solar Facts";

        // auto show after delay if enabled
        if (showOnStart)
            Invoke("OpenPanel", autoShowDelay);
    }

    // ── Update — listens for F key ────────────────────────────────────────
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isPanelOpen)
                ClosePanel();
            else
                OpenPanel();
        }
    }

    // ── Open panel ────────────────────────────────────────────────────────
    public void OpenPanel()
    {
        isPanelOpen = true;

        if (factsPanel != null)
            factsPanel.SetActive(true);

        if (toggleHintText != null)
            toggleHintText.text = "Press F to close";

        ShowFact(currentIndex);
    }

    // ── Close panel ───────────────────────────────────────────────────────
    public void ClosePanel()
    {
        isPanelOpen = false;

        if (factsPanel != null)
            factsPanel.SetActive(false);

        if (toggleHintText != null)
            toggleHintText.text = "Press F — Solar Facts";
    }

    // ── Show next fact ────────────────────────────────────────────────────
    public void ShowNextFact()
    {
        currentIndex++;
        if (currentIndex >= facts.Length)
            currentIndex = 0; // loop back to first

        ShowFact(currentIndex);
    }

    // ── Show previous fact ────────────────────────────────────────────────
    public void ShowPrevFact()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = facts.Length - 1; // loop to last

        ShowFact(currentIndex);
    }

    // ── Display fact at index ─────────────────────────────────────────────
    private void ShowFact(int index)
    {
        if (factTitleText != null)
            factTitleText.text = "Did You Know?";

        if (factBodyText != null)
            factBodyText.text = facts[index];

        if (factCounterText != null)
            factCounterText.text = (index + 1) + " / " + facts.Length;

        // dim prev button if on first fact
        if (prevButton != null)
            prevButton.interactable = (index > 0);

        // dim next button if on last fact
        if (nextButton != null)
            nextButton.interactable = (index < facts.Length - 1);
    }
}