using UnityEngine;
using TMPro;

public class BattleResultUI : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField]private TMP_Text resultText;
    private void Awake()
    {
        resultPanel.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ShowWin()
    {
        resultPanel.SetActive(true);
        resultText.text = "WIn!";
    }
    public void ShowGameOver()
    {
        resultPanel.SetActive(true);
        resultText.text = "Game over";
    }
}
