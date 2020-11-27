using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerSpawner))]
public class PassengersQueue : MonoBehaviour
{
    public event Action<Passenger> OnChangePassenger;
    public BezierCurve PathWay => _pathWay;
    public float MoveSpeed => _moveSpeed;

    [SerializeField] private ServiceManager _serviceManager;
    [SerializeField] private BezierCurve _pathWay;
    [SerializeField] private Player _player;
    [SerializeField] private float _moveSpeed;
    [SerializeField, Range(0f, 1f)] private float _tValue;
    [SerializeField, Range(0.2f, 1f)] private float _tDiePosition;

    private Passenger _firstPassenger;
    private PassengerSpawner _passengerSpawner;
    private float _passengersDistance;

    public void ServicePassenger()
    {
        if (_firstPassenger == null)
            throw new System.Exception("Current passenger is null.");

        if(_firstPassenger.TPosition.tStopPosition - _firstPassenger.TPosition.tCurrentPosition < 0.01f)
            _serviceManager.LoadPassenger(_firstPassenger, _player);
    }

    public void SkipFirstPassenger()
    {
        _passengerSpawner.Passengers[0].SetTStopPosition(0.9f);
        UpdatePositions();
        for (int i = 1; i < _passengerSpawner.Passengers.Count; i++)
            _passengerSpawner.Passengers[i].SetTStopPosition(_passengerSpawner.Passengers[i - 1].TPosition.tCurrentPosition);

        _passengerSpawner.Passengers[0].GoExit(_player);
        _firstPassenger = _passengerSpawner.Passengers[1];
        OnChangePassenger(_firstPassenger);
        UpdatePositions();
    }

    private void Start()
    {
        _passengerSpawner = GetComponent<PassengerSpawner>();

        if (_passengerSpawner.Passengers.Count < _passengerSpawner.MaxLengthQueue)
        {
            for (int i = _passengerSpawner.Passengers.Count; i < _passengerSpawner.MaxLengthQueue; i++)
                _passengerSpawner.SpawnPassenger();

            _firstPassenger = _passengerSpawner.Passengers[0];
            _passengersDistance = _tValue / _passengerSpawner.MaxLengthQueue;
            UpdatePositions();
        }

    }

    private void UpdatePositions()
    {
        if (_passengerSpawner.MaxLengthQueue != 0)
            _passengersDistance = _tValue / _passengerSpawner.MaxLengthQueue;

        for (int i = _passengerSpawner.Passengers.Count - 1; i >= 0; i--)
        {
            _passengerSpawner.Passengers[i].PositionOnQueue = _passengerSpawner.Passengers.Count - i;

            float _t = _passengerSpawner.Passengers[i].PositionOnQueue * _passengersDistance;

            if (_passengerSpawner.Passengers[i].IsServed == false)
                _passengerSpawner.Passengers[i].SetTStopPosition(_t);
            else
                _passengerSpawner.Passengers[i].SetTStopPosition(1f);
            _passengerSpawner.Passengers[i].transform.position = _pathWay.GetPointAt(_passengerSpawner.Passengers[i].TPosition.tCurrentPosition);
        }

        OnChangePassenger?.Invoke(_firstPassenger);
    }

    private void MovePassengers()
    {
        for (int i = _passengerSpawner.Passengers.Count - 1; i >= 0; i--)
        {
            float _newPositionT = _passengerSpawner.Passengers[i].TPosition.tCurrentPosition + MoveSpeed * Time.deltaTime;

            if (_newPositionT < _passengerSpawner.Passengers[i].TPosition.tStopPosition)
            {
                Vector3 _newPosition = _pathWay.GetPointAt(_passengerSpawner.Passengers[i].SetTCurrentPosition(_newPositionT));
                Quaternion _lookPosition = Quaternion.Lerp(_passengerSpawner.Passengers[i].transform.rotation, Quaternion.LookRotation(_newPosition), Time.deltaTime * 1);
                _passengerSpawner.Passengers[i].transform.rotation = _lookPosition;
                _passengerSpawner.Passengers[i].transform.Rotate(-_passengerSpawner.Passengers[i].transform.rotation.eulerAngles.x, 0, 0);
                _passengerSpawner.Passengers[i].transform.position = _newPosition;
            }

            if (_passengerSpawner.Passengers[i].TPosition.tCurrentPosition >= _tDiePosition)
            {
                Destroy(_passengerSpawner.Passengers[i].gameObject);
                _passengerSpawner.Passengers.RemoveAt(i);
                _passengerSpawner.SpawnPassenger();
                UpdatePositions();
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_pathWay.GetPointAt(_tValue), 0.7f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_pathWay.GetPointAt(_tDiePosition), 0.7f);
    }

    private void FixedUpdate()
    {
        UpdatePositions();

        if (_passengerSpawner.Passengers.Count > 0 && _passengerSpawner.Passengers[0].IsServed == false)
            _firstPassenger = _passengerSpawner.Passengers[0];

        if(_player.Status != PlayerStatus.ServicePassenger)
            MovePassengers();
    }

}
