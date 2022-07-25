using UnityEngine;
using System.Collections.Generic;
using System;

public class AIVision : UnitComponent
{
    [SerializeField] private float _scanRadius;
    [SerializeField, Range(0, 1f)] private float _scanFreqauncy;

    public Dictionary<ScannedUnitType, List<Unit>> ScanResults { get; private set; } = new Dictionary<ScannedUnitType, List<Unit>>();
    public event Action OnScan;
    private Timer _frequancyTimer;
    public bool Enabled { get; set; } = true;
    private void Awake()
    {
        _frequancyTimer = new Timer(_scanFreqauncy);
        _frequancyTimer.OnPeriodReached += Scan;
    }

    private void Update()
    {
        _frequancyTimer.Update(Time.deltaTime);
    }

    private void Scan()
    {
        if (!Enabled)
        {
            return;
        }

        var overlap = Physics2D.OverlapCircleAll(Position2D, _scanRadius);
        if(overlap == null)
        {
            return;
        }

        ClearDicitonary();
        for (int i = 0, length = overlap.Length; i < length; i++)
        {
            if (overlap[i].TryGetComponent<Unit>(out var unit))
            {
                if (unit == Owner)
                {
                    continue;
                }

                int teamNumber = unit.teamNumber;

                if (teamNumber == Owner.teamNumber)
                {
                    AddScanResult(ScannedUnitType.Ally, unit);
                }
                else if(teamNumber == 0)
                {
                    AddScanResult(ScannedUnitType.Neutral, unit);
                }
                else
                {
                    AddScanResult(ScannedUnitType.Enemy, unit);
                }
            }

        }
        OnScan?.Invoke();
    }

    private void ClearDicitonary()
    {
        ScanResults = new Dictionary<ScannedUnitType, List<Unit>>();
    }
    private void AddScanResult(ScannedUnitType type, Unit unit)
    {
        if(ScanResults.TryGetValue(type, out var list))
        {
            list.Add(unit);
        }
        else
        {
            ScanResults.Add(type, new List<Unit>());
            ScanResults[type].Add(unit);
        }
    }
    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Position2D, _scanRadius);
    }
}