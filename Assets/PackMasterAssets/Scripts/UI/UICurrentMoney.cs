using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICurrentMoney : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _player.OnAddMoney += UpdateText;
        UpdateText(_player.Money);
    }

    private void UpdateText(int value)
    {
        _text.text = $"x{value}";
    }

    private void OnDestroy()
    {
        _player.OnAddMoney -= UpdateText;
    }
}
