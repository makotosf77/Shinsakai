using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.U2D;

public class HealMiniGame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform cursor;

    [SerializeField] private RectTransform targetArea;

    [Header("Setting")]
    [SerializeField] private float cursorSpeed = 500f;
    [SerializeField] private float perfectRange = 30f;
    [SerializeField] private float goodRange = 80f;

    [Header("Heal Amount")]
    [SerializeField] private int perfectHeal = 30;
    [SerializeField] private int goodHeal = 20;
    [SerializeField] private int normalHeal = 10;
    [SerializeField] private int missHeal = 5;

    private bool isPlaying;

    private float minX;
    private float maxX;
    private int direction = 1;

    private Action<int> onFinished;

    private void Awake()
    {
        panel.SetActive(false);
    }
    private void Update()
    {
        if(!isPlaying)
        {
            return;
        }
        MoveCursor();
        if(Input.GetMouseButton(0))
        {
            FinishGame();
        }
    }
    public void StartGame(Action<int>finishedCallback)
    {
        onFinished= finishedCallback;
        panel.SetActive(true);
        isPlaying = true;
        direction = 1;
        CalculateBounds();
        cursor.anchoredPosition = new Vector2(minX,cursor.anchoredPosition.y);
    }
    private void CalculateBounds()
    {
        float halfWidth = targetArea.rect.width / 2f;
        minX = -halfWidth;
        maxX = halfWidth;
    }
    private void MoveCursor()
    {
        Vector2 position = cursor.anchoredPosition;
        position.x += cursorSpeed * direction * Time.deltaTime;
        if(position.x>=maxX)
        {
            position.x = maxX;
            direction = -1;
        }
        if (position.x <= minX)
        {
            position.x = minX;
            direction = 1;

        }
        cursor.anchoredPosition = position;
    }
    private void FinishGame()
    {
        if(!isPlaying)
        {
            return;
        }
        isPlaying= false;
        panel.SetActive(false);
        float distance = Mathf.Abs(cursor.anchoredPosition.x);
        int healAmount;
        if(distance <= perfectRange)
        {
            healAmount = perfectHeal;

        }
        else if(distance<=goodRange)
        {
            healAmount = goodHeal;
        }
        else
        {
            healAmount = normalHeal;
        }
        onFinished?.Invoke(healAmount);
    }
}
