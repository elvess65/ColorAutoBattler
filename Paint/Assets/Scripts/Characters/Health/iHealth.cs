using Paint.Character.Weapon;

namespace Paint.Character.Health
{
    public interface iHealth
    {
        event System.Action<WeaponTypes, int> OnTakeDamage;
        event System.Action OnDestroy;
        event System.Action<WeaponTypes> OnWrongType;

        bool IsDestroyed { get; }

        void TakeDamage(WeaponTypes type, int damage);
    }
}
