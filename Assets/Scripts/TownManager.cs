using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TownManager : MonoBehaviour
{
    public enum TownZone { None, Sell, Upgrade, TempUpgrade, Home }

    [Header("Triggers")]
    public Collider2D sellTrigger;
    public Collider2D upgradeTrigger;
    public Collider2D tempUpgradeTrigger;
    public Collider2D homeTrigger;

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
    public CanvasGroup fadeGroup;
    public TextMeshProUGUI dayText;
    public float fadeDuration = 0.6f;

    private TownZone _currentZone = TownZone.None;

    private void Update()
    {
        if (_currentZone == TownZone.None) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_currentZone == TownZone.Sell)
            {
                OpenPanel(sellPanel);
                SellAllFish();
            }
            else if (_currentZone == TownZone.Upgrade)
            {
                OpenPanel(upgradePanel);
            }
            else if (_currentZone == TownZone.TempUpgrade)
            {
                OpenPanel(tempUpgradePanel);
            }
            else if (_currentZone == TownZone.Home)
            {
                StartCoroutine(EndDayRoutine());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (sellTrigger != null && other.IsTouching(sellTrigger))
        {
            _currentZone = TownZone.Sell;
            SetPrompt(sellPrompt, true);
        }
        else if (upgradeTrigger != null && other.IsTouching(upgradeTrigger))
        {
            _currentZone = TownZone.Upgrade;
            SetPrompt(upgradePrompt, true);
        }
        else if (tempUpgradeTrigger != null && other.IsTouching(tempUpgradeTrigger))
        {
            _currentZone = TownZone.TempUpgrade;
            SetPrompt(tempUpgradePrompt, true);
        }
        else if (homeTrigger != null && other.IsTouching(homeTrigger))
        {
            _currentZone = TownZone.Home;
            SetPrompt(homePrompt, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (sellTrigger != null && other.IsTouching(sellTrigger) == false)
        {
            SetPrompt(sellPrompt, false);
        }
        if (upgradeTrigger != null && other.IsTouching(upgradeTrigger) == false)
        {
            SetPrompt(upgradePrompt, false);
        }
        if (tempUpgradeTrigger != null && other.IsTouching(tempUpgradeTrigger) == false)
        {
            SetPrompt(tempUpgradePrompt, false);
        }
        if (homeTrigger != null && other.IsTouching(homeTrigger) == false)
        {
            SetPrompt(homePrompt, false);
        }

        _currentZone = TownZone.None;
    }

    private void SetPrompt(GameObject prompt, bool active)
    {
        if (prompt != null) prompt.SetActive(active);
    }

    private void OpenPanel(GameObject panel)
    {
        if (panel == null) return;
        panel.SetActive(true);
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
        if (fadeGroup != null)
        {
            fadeGroup.gameObject.SetActive(true);
            yield return FadeCanvas(0f, 1f, fadeDuration);
        }

        if (PersistentManager.Instance != null)
        {
            PersistentManager.Instance.currentDay += 1;
        }

        if (dayText != null && PersistentManager.Instance != null)
        {
            dayText.text = "G\u00fcn: " + PersistentManager.Instance.currentDay;
        }

        yield return new WaitForSeconds(0.8f);

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            if (fadeGroup != null) fadeGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
        if (fadeGroup != null) fadeGroup.alpha = to;
    }
}
