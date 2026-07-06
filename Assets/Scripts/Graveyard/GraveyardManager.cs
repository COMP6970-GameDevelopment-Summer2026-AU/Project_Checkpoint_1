// GraveyardManager.cs — game state for Graveyard Keeper: Night Harvest.
// Tracks harvested resources, updates the HUD (counters, objective, night timer,
// interaction prompt), and handles the win / end-of-night state and restart.

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GraveyardManager : MonoBehaviour
{
    public static GraveyardManager Instance { get; private set; }

    [Header("Objective — collect this many of each")]
    public int woodTarget = 8;
    public int stoneTarget = 6;
    public int pumpkinTarget = 5;

    [Header("Night length (seconds)")]
    public float nightDuration = 180f;

    [Header("HUD (assigned by the builder)")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI pumpkinText;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI banishText;

    [Header("Interaction prompt")]
    public GameObject promptRoot;
    public TextMeshProUGUI promptText;

    [Header("End panel")]
    public GameObject endPanel;
    public TextMeshProUGUI endText;

    int wood, stone, pumpkin;
    int banished;
    float timeLeft;
    bool playing = true;

    public bool IsPlaying => playing;
    public float TimeLeft => timeLeft;
    public float NightLength => nightDuration;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void BanishSpirit()
    {
        banished++;
        if (banishText) banishText.text = $"Spirits banished {banished}";
    }

    void Start()
    {
        timeLeft = nightDuration;
        playing = true;
        Time.timeScale = 1f;
        if (endPanel) endPanel.SetActive(false);
        HidePrompt();
        UpdateHUD();
    }

    void Update()
    {
        if (!playing)
        {
            if (GKInput.RestartPressed()) Restart();
            return;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            EndNight(false);   // ran out of time
        }
        UpdateTimer();
    }

    public void AddResource(Harvestable.ResourceType type, int amount)
    {
        switch (type)
        {
            case Harvestable.ResourceType.Wood:    wood    += amount; break;
            case Harvestable.ResourceType.Stone:   stone   += amount; break;
            case Harvestable.ResourceType.Pumpkin: pumpkin += amount; break;
        }
        UpdateHUD();

        if (wood >= woodTarget && stone >= stoneTarget && pumpkin >= pumpkinTarget)
            EndNight(true);   // objective complete
    }

    // ── HUD ───────────────────────────────────────────────────────────────────
    void UpdateHUD()
    {
        if (woodText)    woodText.text    = $"Wood {wood}/{woodTarget}";
        if (stoneText)   stoneText.text   = $"Stone {stone}/{stoneTarget}";
        if (pumpkinText) pumpkinText.text = $"Pumpkins {pumpkin}/{pumpkinTarget}";
        if (objectiveText)
            objectiveText.text = "Restore the grounds before dawn — harvest Wood, Stone & Pumpkins.";
        if (banishText) banishText.text = $"Spirits banished {banished}";
    }

    void UpdateTimer()
    {
        if (!timerText) return;
        int m = Mathf.FloorToInt(timeLeft / 60f);
        int s = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = $"Dawn in {m:0}:{s:00}";
    }

    public void ShowPrompt(string msg)
    {
        if (promptRoot) promptRoot.SetActive(true);
        if (promptText) promptText.text = msg;
    }

    public void HidePrompt()
    {
        if (promptRoot) promptRoot.SetActive(false);
    }

    // ── End of night ───────────────────────────────────────────────────────────
    void EndNight(bool won)
    {
        if (!playing) return;
        playing = false;
        HidePrompt();
        AudioManager.PlayEnd(won);

        if (endPanel) endPanel.SetActive(true);
        if (endText)
        {
            string title = won
                ? "<color=#7CFC7C><size=52><b>GROUNDS RESTORED</b></size></color>\n<size=26>You survived the night!</size>"
                : "<color=#FFB347><size=52><b>DAWN BROKE</b></size></color>\n<size=26>The night is over.</size>";

            endText.text =
                title + "\n\n" +
                $"<color=white>Wood      <b>{wood}/{woodTarget}</b>\n" +
                $"Stone     <b>{stone}/{stoneTarget}</b>\n" +
                $"Pumpkins  <b>{pumpkin}/{pumpkinTarget}</b></color>\n\n" +
                "<color=#8Fe3ff><size=26><b>Press SPACE to play again</b></size></color>";
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
