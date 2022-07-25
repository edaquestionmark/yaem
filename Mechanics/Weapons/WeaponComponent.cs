using UnityEngine;

public abstract class WeaponComponent : MonoBehaviour, IComponent<Weapon>
{
    protected Weapon Owner;
    public ComponentType ComponentType => ComponentType.Singletone;

    public virtual void Init(Weapon owner)
    {
        Owner = owner;
    }
}
