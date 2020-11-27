using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus
{
    Idle = 0,
    SkipPassenger = 1,
    ServicePassenger = 2

}

public class Player : MonoBehaviour
{
    public event Action<PlayerStatus> OnChangePlayerStatus;
    public event Action<int> OnAddMoney;
    public PlayerStatus Status 
    {
        get => _status;
        set
        {
            if(value != _status)
            {
                _status = value;
                OnChangePlayerStatus?.Invoke(value);
            }
        }
    }
    public int Money { get; private set; }

    private PlayerStatus _status;

    public void AddMoney(int value)
    {
        Money += value;
        OnAddMoney?.Invoke(Money);
    }

    private void Start()
    {
        //ServicePassenger();
    }

}
