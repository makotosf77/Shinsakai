using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickAttackMiniGame : MonoBehaviour, IAttckMiniGame
{
    [Header("UI")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text clickCountText;
    [SerializeField] private Slider timerSlider;

    [Header("Settings")]
    [SerializeField] private float duration = 3f;
    [SerializeField] private int maxClicks = 20;

    [Header("Damage")]
    [SerializeField] private int minDamage = 5;
    [SerializeField] private int maxDamage = 50;

    private bool isPlaying;
    private int clickCount;
    private float remainingTime;

    private Action<AttackResults> onFinished;


    private void Awake()
    {
        gamePanel.SetActive(false);
    }


    public void StartGame(Action<AttackResults> callback)
    {
        if (isPlaying)
        {
            return;
        }

        onFinished = callback;

        StartCoroutine(PlayGame());
    }


    private IEnumerator PlayGame()
    {
        isPlaying = true;

        clickCount = 0;
        remainingTime = duration;

        gamePanel.SetActive(true);

        UpdateUI();

        // ボタンを押したクリックを無視するため1フレーム待つ
        yield return null;

        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                AddClick();
            }

            UpdateUI();

            yield return null;
        }

        remainingTime = 0f;

        UpdateUI();

        yield return new WaitForSeconds(0.5f);

        FinishGame();
    }


    private void AddClick()
    {
        if (clickCount >= maxClicks)
        {
            return;
        }

        clickCount++;

        Debug.Log("クリック数: " + clickCount);

        UpdateUI();
    }


    private void UpdateUI()
    {
        Debug.Log(
            $"UI更新 時間:{remainingTime:F1} クリック:{clickCount}"
        );

        if (instructionText != null)
        {
            instructionText.text = "クリック連打！";
        }

        if (timerText != null)
        {
            timerText.text =
                $"残り時間: {remainingTime:F1} 秒";
        }

        if (clickCountText != null)
        {
            clickCountText.text =
                $"クリック: {clickCount} / {maxClicks}";
        }

        if (timerSlider != null)
        {
            timerSlider.value =
                Mathf.Clamp01(remainingTime / duration);
        }
    }


    private void FinishGame()
    {
        float accuracy =
            (float)clickCount / maxClicks;

        int damage =
            Mathf.RoundToInt(
                Mathf.Lerp(
                    minDamage,
                    maxDamage,
                    accuracy
                )
            );

        Debug.Log($"最終ダメージ: {damage}");

        isPlaying = false;

        gamePanel.SetActive(false);

        AttackResults result =
            new AttackResults(
                damage,
                accuracy
            );

        onFinished?.Invoke(result);

        onFinished = null;
    }
}