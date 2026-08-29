using System;
using System.Collections;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TargetAttackMiniGame : MonoBehaviour, IAttckMiniGame
{
    [Header("UI")]
    [SerializeField] private GameObject gamePanel;

    [SerializeField]
    private TMP_Text instructionText;

    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private TMP_Text resultText;

    [Header("Target")]
    [SerializeField]
    private RectTransform targetArea;

    [SerializeField]
    private RectTransform targetCenter;

    [Header("Settings")]
    [SerializeField]
    private float duration = 3f;
    [Header("Movement")]
    [SerializeField]
    private float movespeed = 1f;
    [SerializeField]
    private float chargeDirectionInterval = 1f;

    [Header("Damage")]
    [SerializeField]
    private int minDamage = 10;

    [SerializeField]
    private int maxDamage = 80;

    private bool isPlaying;

    private float remainingTime;

    private Vector2 targetDirection;
    private float directiontimer;

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

        remainingTime = duration;

        resultText.gameObject.SetActive(false);

        gamePanel.SetActive(true);

        ResetTarget();


        UpdateUI();

        // Magicボタンを押したクリックを
        // 攻撃判定に使わない
        yield return null;

        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            MoveTarget();

            UpdateUI();

            if (Input.GetMouseButtonDown(0))
            {
                CheckHit();

                yield break;
            }

            yield return null;
        }

        // 時間切れ
        FinishGame(0f);
    }

    private void ResetTarget()
    {
        targetCenter.anchoredPosition = GetRandomPosition();
        ChangeDirection();
        directiontimer = chargeDirectionInterval;
    }
    private void MoveTarget()
    {
        directiontimer -= Time.deltaTime;

        if (directiontimer <= 0f)
        {
            ChangeDirection();
            directiontimer = chargeDirectionInterval;
        }

        Vector2 newPosition =
            targetCenter.anchoredPosition +
            targetDirection *
            movespeed *
            Time.deltaTime;

        // TargetAreaの実際の範囲を取得
        Rect areaRect = targetArea.rect;

        // 的の半分のサイズ
        float targetHalfWidth =
            targetCenter.rect.width / 2f;

        float targetHalfHeight =
            targetCenter.rect.height / 2f;

        // TargetAreaの中に収まるように制限
        float minX =
            areaRect.xMin + targetHalfWidth;

        float maxX =
            areaRect.xMax - targetHalfWidth;

        float minY =
            areaRect.yMin + targetHalfHeight;

        float maxY =
            areaRect.yMax - targetHalfHeight;

        newPosition.x =
            Mathf.Clamp(
                newPosition.x,
                minX,
                maxX
            );

        newPosition.y =
            Mathf.Clamp(
                newPosition.y,
                minY,
                maxY
            );

        targetCenter.anchoredPosition =
            newPosition;
    }
    private void ChangeDirection()
    {
        targetDirection = UnityEngine.Random.insideUnitCircle.normalized;
    }
    private Vector2 GetRandomPosition()
    {
        // TargetAreaの実際の範囲
        Rect areaRect = targetArea.rect;

        // 的の半分のサイズ
        float targetHalfWidth =
            targetCenter.rect.width / 2f;

        float targetHalfHeight =
            targetCenter.rect.height / 2f;

        float minX =
            areaRect.xMin + targetHalfWidth;

        float maxX =
            areaRect.xMax - targetHalfWidth;

        float minY =
            areaRect.yMin + targetHalfHeight;

        float maxY =
            areaRect.yMax - targetHalfHeight;

        float randomX =
            UnityEngine.Random.Range(
                minX,
                maxX
            );

        float randomY =
            UnityEngine.Random.Range(
                minY,
                maxY
            );

        return new Vector2(
            randomX,
            randomY
        );
    }
    private void UpdateUI()
    {
        if (instructionText != null)
        {
            instructionText.text =
                "中心を狙ってクリック！";
        }

        if (timerText != null)
        {
            timerText.text =
                $"残り時間: {remainingTime:F1} 秒";
        }
    }


    private void CheckHit()
    {
        Vector2 mousePosition =
            Input.mousePosition;

        Vector2 localPoint;

        RectTransformUtility
            .ScreenPointToLocalPointInRectangle(
                targetArea,
                mousePosition,
                null,
                out localPoint
            );

        float distance =
            Vector2.Distance(
                localPoint,
                targetCenter.anchoredPosition
            );

        float maxDistance =
            targetArea.rect.width / 2f;

        float accuracy =
            1f -
            Mathf.Clamp01(
                distance / maxDistance
            );

        FinishGame(accuracy);
    }


    private void FinishGame(float accuracy)
    {
        int damage =
            Mathf.RoundToInt(
                Mathf.Lerp(
                    minDamage,
                    maxDamage,
                    accuracy
                )
            );

        string result;

        if (accuracy >= 0.9f)
        {
            result = "PERFECT!";
        }
        else if (accuracy >= 0.7f)
        {
            result = "GREAT!";
        }
        else if (accuracy >= 0.4f)
        {
            result = "GOOD!";
        }
        else
        {
            result = "MISS...";
        }

        Debug.Log(
            $"命中率: {accuracy:F2}"
        );

        Debug.Log(
            $"魔法ダメージ: {damage}"
        );

        StartCoroutine(
            ShowResultAndFinish(
                accuracy,
                damage,
                result
            )
        );
    }


    private IEnumerator ShowResultAndFinish(
        float accuracy,
        int damage,
        string result)
    {
        resultText.gameObject.SetActive(true);

        resultText.text =
            $"{result}\n{damage} DAMAGE!";

        yield return new WaitForSeconds(1f);

        gamePanel.SetActive(false);

        isPlaying = false;

        AttackResults attackResult =
            new AttackResults(
                damage,
                accuracy
            );

        onFinished?.Invoke(attackResult);

        onFinished = null;
    }
}