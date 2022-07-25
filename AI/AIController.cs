using System;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput;
    public event Action<CommandKey> OnCommandInput;

    [SerializeField] private AIStateMachinePreset _preset;
    public Unit AttachedUnit;
    private WeightedStateMachine<AIController> _stateMachhine = new WeightedStateMachine<AIController>();
    [field: SerializeField] public AIVision Vision { get; private set; }
    [field: SerializeField] public float EngagmentRadius { get; private set; }

    private void Start()
    {
        if (!AttachedUnit.TryChangeController(this))
        {
            Debug.LogError("Cannot change controller of the " + AttachedUnit.name);
        }
        _stateMachhine.Init(_preset.GetStates(AttachedUnit), this);
    }

    private void Update()
    {
        _stateMachhine?.Execute();
    }
    public void MoveToPoint(Vector2 point)
    {
        MoveInput?.Invoke((point - AttachedUnit.Position2D).normalized);
    }
    public void LookAtPoint(Vector2 point)
    {
        LookInput?.Invoke(point - AttachedUnit.Position2D);
    }
    public void LookAtDirection(Vector2 point)
    {
        LookInput?.Invoke(point);
    }
    public void MoveToDirection(Vector2 direcion)
    {
        MoveInput?.Invoke(direcion);
    }
    public void WriteCommand(CommandKey command)
    {
        OnCommandInput?.Invoke(command);
    }

    public bool IsEnemyInRange(out List<Unit> enemys)
    {
        if(Vision.ScanResults.TryGetValue(ScannedUnitType.Enemy, out var list))
        {
            enemys = list;
            return true;
        }
        enemys = null;
        return false;
    }
    public bool HandleWalking(Vector2 direction, float distance, out RaycastHit2D result, out bool rightPosition)
    {
        var raycast1 = Physics2D.RaycastAll(AttachedUnit.Position2D + (Vector2)AttachedUnit.transform.right * AttachedUnit.Size, direction, distance);
        var raycast2 = Physics2D.RaycastAll(AttachedUnit.Position2D - (Vector2)AttachedUnit.transform.right * AttachedUnit.Size, direction, distance);

        if((raycast1 == null && raycast2 == null) ||
            (raycast1.Length == 1 && raycast1[0].transform == AttachedUnit) &&
            (raycast2.Length == 1 && raycast2[0].transform == AttachedUnit))
        {
            rightPosition = false;
            result = default;
            return false;
        }

        if(raycast1 != null && raycast1.Length > 1 && raycast1[0].transform == AttachedUnit.transform)
        {
            rightPosition = true;
            result = raycast1[1];
            return true;
        }

        if (raycast2 != null && raycast2.Length > 1 &&  raycast2[0].transform == AttachedUnit.transform)
        {
            rightPosition = false;
            result = raycast2[1];
            return true;
        }

        rightPosition = false;
        result = default;
        return false;
    }

    public float DeltaTime => Time.deltaTime;
}
