using UnityEngine;

public class TargetSeeker : AttackRandomTarget
{
    private ISeekStrategy _strategy;
    private bool _isSeeking = false;
    private Vector2 _lastSawPosition;
    private LayerMask _wallsMask;

    public TargetSeeker(IEngageMovment movment, LayerMask walls, ISeekStrategy strategy) : base(movment, walls)
    {
        _strategy = strategy;
        _wallsMask = walls;
    }

    public override float CalculateEffectivness()
    {
        return (LastSawTarget == null ? -1f : 2f) + base.CalculateEffectivness() * 2;
    }

    public override void Execute()
    {
        if (!_isSeeking || LastSawTarget == null)
        {
            _isSeeking = false;
            LastSawTarget = null;
            base.Execute();
            return;
        }
        Debug.LogError("Seeking");
        if (!_strategy.TrySeek(_lastSawPosition))
        {
            Debug.LogError("Cannot seek");
            LastSawTarget = null;
            _isSeeking = false;
        }
    }

    protected override void TargetLost(Vector2 lastPosition, Unit target)
    {
        _isSeeking = true;
        _lastSawPosition = lastPosition;
        _strategy.Init(_owner, target, _attached, _wallsMask);
    }
}
