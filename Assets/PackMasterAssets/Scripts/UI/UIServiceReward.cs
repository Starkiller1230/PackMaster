using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIServiceReward : MonoBehaviour
{
    [SerializeField] private PassengersQueue _passengersQueue;
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _passengersQueue.OnChangePassenger += UpdateText;
    }

    private void UpdateText(Passenger passenger)
    {
        _text.text = $"+{passenger.ServiceReward}";
    }

    private void OnDestroy()
    {
        _passengersQueue.OnChangePassenger -= UpdateText;
    }

}
