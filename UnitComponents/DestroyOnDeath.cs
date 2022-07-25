using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private void Start()
    {
        _unit.Health.OnDeath += Death;
    }

    private void Death(DamageArgs obj)
    {
        Destroy(_unit.gameObject);
    }
}
