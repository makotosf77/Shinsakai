using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Enemy enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hpSlider == null || enemy == null)
        {
            Debug.LogError("EnemyHPBarの参照がない");
            return;
        }
        enemy.OnHPChanged += UpdateHPBar;
        UpdateHPBar(enemy.CurrentHP, enemy.MaxHP);
    }

    private void OnDestroy()
    {
        if(enemy !=null)
        {
            enemy.OnHPChanged -=UpdateHPBar;
        }
    }
    private void UpdateHPBar(int currentHP,int maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }

}
