using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    [SerializeField]
    private Slider hpSlider;
    [SerializeField] private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player==null)
        {
            return;
        }
        player.OnHPChanged += UpdateHPBar;
        UpdateHPBar(player.CurrentHP,player.MaxHP);
    }
    private void OnDestroy()
    {
        if(player!=null)
        {
            player.OnHPChanged -=UpdateHPBar;
        }
    }
    private void UpdateHPBar(int currentHP, int maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
