public class AttackResults
{
    public int Damage { get; }
    public float Accuracy { get; }
    public bool IsCritical { get; }
    public AttackResults(int damage, float accuracy, bool isCritical = false)
    {
        Damage = damage;
        Accuracy = accuracy;
        IsCritical = isCritical;
    }

}