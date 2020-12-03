namespace Entities.MonoBehaviours
{
    public interface IDamageable
    {
        int StartingHealth { get; }
        int CurrentHealth { get; set; }
    }
}