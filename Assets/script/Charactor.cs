using UnityEngine;

public abstract class Charactor : MonoBehaviour
{
    [SerializeField]protected int maxHP = 100;
    public int CurrentHP { get; protected set; }
    public int MaxHP => maxHP;
    public bool IsDead => CurrentHP <= 0;
    protected virtual void Awake()
    {
        CurrentHP = maxHP;
    }
    public virtual void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        CurrentHP = Mathf.Max(CurrentHP, 0);
        Debug.Log($"{gameObject.name}は{damage}のダメージを受けた。残りHP: {CurrentHP}");
        if (IsDead)
        {
            Die();
        }
    }
    public virtual void Heal(int amount)
    {
        if(IsDead)
        {
            return;
        }
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP,maxHP);
    }
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name}は死亡した。");

    }
}
