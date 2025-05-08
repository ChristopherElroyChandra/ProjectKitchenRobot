using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [SerializeField] int _ticksPerSecond;
    public int TicksPerSecond { get { return _ticksPerSecond; } }
    [Min(1)]
    [SerializeField] int _timeMultiplier;
    public int TimeMultiplier { get { return _timeMultiplier; } }
    [SerializeField] List<int> _timeMultipliers;
    public List<int> TimeMultipliers { get { return _timeMultipliers; } }
    [SerializeField] int _timeMultiplierIndex;
    public int TimeMultiplierIndex { get { return _timeMultiplierIndex; } }

    public float TickInterval { get { return 1f / (_ticksPerSecond * _timeMultiplier); } }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        _timeMultiplierIndex = 0;
        _timeMultiplier = _timeMultipliers[_timeMultiplierIndex];
    }

    public bool CanIncreaseTimeMultiplier()
    {
        return _timeMultiplierIndex < _timeMultipliers.Count - 1;
    }

    public bool CanDecreaseTimeMultiplier()
    {
        return _timeMultiplierIndex > 0;
    }

    public void IncreaseTimeMultiplier()
    {
        _timeMultiplierIndex++;
        _timeMultiplier = _timeMultipliers[_timeMultiplierIndex];
    }

    public void DecreaseTimeMultiplier()
    {
        _timeMultiplierIndex--;
        _timeMultiplier = _timeMultipliers[_timeMultiplierIndex];
    }
}
