using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour2D
{
    [SerializeField] private float _delayBeforeDisabling;
    [SerializeField] private GameObject[] _disabledComponentsBeforeDisabling;
    [SerializeField] private float _speed;
    [SerializeField] private TeamColorAdapter _adapter;
    [SerializeField] private float _maxLifeTime = 30;

    private Pool<Projectile> _pool;
    private DamageArgs _args;
    public event Action OnInit;
    private Vector2 _lastPosition;

    private float _elapsedTime;
    private WaitForSeconds _delay;
    private bool _isDisabling = false;

    private void Awake()
    {
        _delay = new WaitForSeconds(_delayBeforeDisabling);
    }
    public void Init(Pool<Projectile> pool, DamageArgs args, Vector2 position)
    {
        _pool = pool;
        _args = args;
        _lastPosition = position;
        gameObject.SetActive(true);
        Position2D = position;
        SetComponentsActiveState(true);
        OnInit?.Invoke();
        _elapsedTime = 0;

        var check = Physics2D.OverlapPoint(position);
        if (check)
        {
            OnHit(check.transform);
        }
        _adapter?.Init(args.Attacker);
    }

    private void SetComponentsActiveState(bool value)
    {
        for(int i = 0, length = _disabledComponentsBeforeDisabling.Length; i < length; i++)
        {
            _disabledComponentsBeforeDisabling[i].SetActive(value);
        }
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _maxLifeTime)
        {
            StartCoroutine(AwaitingDisabling());
        }
        _lastPosition = Position2D;
    }

    private void OnHit(Transform target)
    {
        if (target.TryGetComponent<Unit>(out var unit))
        {
            unit.Health.TakeDamage(_args);
        }
        StartCoroutine(AwaitingDisabling());
    }
    private void LateUpdate()
    {
        if (_isDisabling)
        {
            return;
        }
        Position2D += (Vector2)Cached.up * _speed * Time.deltaTime;

        var result = Physics2D.Raycast(Position2D, (_lastPosition - Position2D), Vector2.Distance(_lastPosition, Position2D));

        if(result)
        {
            OnHit(result.transform);
            Position2D = result.point;
        }
    }

    private IEnumerator AwaitingDisabling()
    {
        if (_isDisabling)
        {
            yield break;
        }
        _isDisabling = true;
        SetComponentsActiveState(false);
        yield return _delay;
        _isDisabling = false;
        gameObject.SetActive(false);
        _pool?.ReturnToPool(this);   
    }
}
