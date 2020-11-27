using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SkinController))]
public class PassengerSpawner : MonoBehaviour
{
    public int MaxLengthQueue => _maxLengthQueue;
    public List<Passenger> Passengers { get; set; } = new List<Passenger>();
    public int WaitingServicePassengersCount = 0;

    [SerializeField] private Passenger _passengerPrefab;
    [SerializeField, Range(0.1f, 1)] private float _minBaggagiFillRatio;
    [SerializeField, Range(0.5f, 1)] private float _maxBaggagiFillRatio;
    [SerializeField, Min(0)] private int _maxLengthQueue;
    [SerializeField, Min(0)] private int _maxRewardToServise;
    
    private SkinController _skinController;

    public void SpawnPassenger()
    {
        if (Passengers.Count > _maxLengthQueue)
            return;

        // Change random values.
        Country _randomCountry = (Country)Random.Range(0, Enum.GetValues(typeof(Country)).Length);
        PurposeTravel _randonPurposeTravel = (PurposeTravel)Random.Range(0, Enum.GetValues(typeof(PurposeTravel)).Length);
        BaggageSize _randomBagSize = (BaggageSize)Random.Range(1, Enum.GetValues(typeof(BaggageSize)).Length);
        float _randonBaggageFillRation = Random.Range(_minBaggagiFillRatio, _maxBaggagiFillRatio);
        Skin _randomSkin = _skinController.GenerateSkin(_randomCountry);

        // Instantiate and initialize passenger prefab.
        Passenger _newPassenger = Instantiate(_passengerPrefab.gameObject).GetComponent<Passenger>();
        _newPassenger.Init(_randomCountry, _randonPurposeTravel, _randomBagSize, _randomSkin, _maxRewardToServise, _randonBaggageFillRation);
        _newPassenger.SetTCurrentPosition(0f);
        Passengers.Add(_newPassenger);
    }

    private void Awake()
    {
        _skinController = GetComponent<SkinController>();
    }
}
