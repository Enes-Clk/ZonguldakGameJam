using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TownManager : MonoBehaviour
{
    public enum TownZone { None, Sell, Upgrade, TempUpgrade, Home }

    [Header("Panels")]
    public GameObject sellPanel;
    public GameObject upgradePanel;
    public GameObject tempUpgradePanel;
    public GameObject homePanel;

    [Header("Prompts")]
    public GameObject sellPrompt;
    public GameObject upgradePrompt;
    public GameObject tempUpgradePrompt;
    public GameObject homePrompt;

    [Header("Home UI")]
    public GameObject fadeCanvasObject;
    public CanvasGroup fadeGroup;
    public TextMeshProUGUI dayText;
    public float fadeDuration = 0.6f;

    private TownZone _currentZone = TownZone.None;

    private void Start()
    {
        if (fadeCanvasObject != null)
            fadeCanvasObject.SetActive(false);

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
        }
    }

    public void OnZoneEnter(TownZone zone, Collider2D player)
    {
        _currentZone = zone;
        SetPrompt(sellPrompt,        zone == TownZone.Sell);
        SetPrompt(upgradePrompt,     zone == TownZone.Upgrade);
        SetPrompt(tempUpgradePrompt, zone == TownZone.TempUpgrade);
        SetPrompt(homePrompt,        zone == TownZone.Home);
    }

    public void OnZoneExit(TownZone zone)
    {
        if (_currentZone == zone)
        {
            _currentZone = TownZone.None;
            SetPrompt(sellPrompt, false);
            SetPrompt(upgradePrompt, false);
            SetPrompt(tempUpgradePrompt, false);
            SetPrompt(homePrompt, false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllPanels();
            return;
        }

        if (_currentZone == TownZone.None) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;

        switch (_currentZone)
        {
            case TownZone.Sell:
                OpenPanel(sellPanel);
                SellAllFish();
                break;
            case TownZone.Upgrade:
                OpenPanel(upgradePanel);
                break;
            case TownZone.TempUpgrade:
                OpenPanel(tempUpgradePanel);
                break;
            case TownZone.Home:
                StartCoroutine(EndDayRoutine());
                break;
        }
    }

    private void CloseAllPanels()
    {
        if (sellPanel != null)        sellPanel.SetActive(false);
        if (upgradePanel != null)     upgradePanel.SetActive(false);
        if (tempUpgradePanel != null) tempUpgradePanel.SetActive(false);
        if (homePanel != null)        homePanel.SetActive(false);
    }

    private void SetPrompt(GameObject prompt, bool active)
    {
        if (prompt != null) prompt.SetActive(active);
    }

    private void OpenPanel(GameObject panel)
    {
        if (panel != null) panel.SetActive(true);
    }

    private void SellAllFish()
    {
        if (PersistentManager.Instance == null) return;
        int earnings = PersistentManager.Instance.fishCount * 10;
        PersistentManager.Instance.AddMoney(earnings);
        PersistentManager.Instance.ResetFish();
    }

    private IEnumerator EndDayRoutine()
    {
        if (fadeCanvasObject != null)
            fadeCanvasObject.SetActive(true);

        if (fadeGroup != null)
        {
            fadeGroup.blocksRaycasts = true;
            fadeGroup.interactable = true;
            yield return FadeCanvas(0f, 1f, fadeDuration);
        }

        if (PersistentManager.Instance != null)
            PersistentManager.Instance.currentDay += 1;

        if (dayText != null && PersistentManager.Instance != null)
            dayText.text = "Gün: " + PersistentManager.Instance.currentDay;

        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(timer / duration));
            yield return null;
        }
        fadeGroup.alpha = to;
    }
}