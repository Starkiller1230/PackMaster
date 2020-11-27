using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform _servicePassengerPanel;
    [SerializeField] private RectTransform _gameHud;
    [SerializeField] private Player _player;

    private void Awake()
    {
        if(_player != null)
            _player.OnChangePlayerStatus += ChangePlayerStatus;
        ChangePlayerStatus(_player.Status);
    }

    private void ChangePlayerStatus(PlayerStatus playerStatus)
    {
        _servicePassengerPanel.gameObject.SetActive(playerStatus == PlayerStatus.ServicePassenger ? true : false);
        _gameHud.gameObject.SetActive(playerStatus == PlayerStatus.ServicePassenger ? false : true);


    }

    private void OnDestroy()
    {
        if (_player != null)
            _player.OnChangePlayerStatus -= ChangePlayerStatus;
    }
}
