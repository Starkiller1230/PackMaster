using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Baggage
{
    public BaggageSize GetBaggageSize => _baggageSize;
    public List<Item> Items { get; private set; }
    public float BaggageFillRatio => _baggageFillRatio;

    [SerializeField] private BaggageSize _baggageSize;
    [SerializeField, Range(0, 1)] private float _baggageFillRatio;

    public Baggage(Passenger passenger, BaggageSize baggageSize, float baggageFillRatio)
    {
        _baggageSize = baggageSize;
        _baggageFillRatio = baggageFillRatio;
        GenerateItems(passenger);
    }

    private void GenerateItems(Passenger passenger)
    {
        int _capasity = Size.GetCapasity(_baggageSize);
        if (_capasity <= 0)
            throw new System.Exception("BaggageSize <= 0");

        List<Item> _posibleItems = new List<Item>();
        Items = new List<Item>();

        //Select posible items.
        foreach (Item item in Item.AllItemList)
        {
            if(item == null)
                continue;

            if(item.IsAllCountries == true && item.IsAllPurposesTravel == true)
            {
                _posibleItems.Add(item);
            }
            else if(item.IsAllCountries == true)
            {
                foreach (PurposeTravel purposeTravel in item.GetAllowedPurposseTravel)
                    if (passenger.GetPurposeTravel == purposeTravel)
                        _posibleItems.Add(item);
            }
            else if(item.IsAllPurposesTravel == true)
            {
                foreach (Country country in item.GetAllowedCountries)
                    if (passenger.GetCountry == country)
                        _posibleItems.Add(item);
            }
            else
            {
                foreach (Country country in item.GetAllowedCountries)
                    foreach (PurposeTravel purposeTravel in item.GetAllowedPurposseTravel)
                        if (passenger.GetCountry == country && passenger.GetPurposeTravel == purposeTravel)
                            _posibleItems.Add(item);
            }

        }

        if (_posibleItems.Count <= 0)
            return;

        //Fill the bag width random posible items.
        int _currentSize = (int)(_baggageFillRatio * _capasity);

        for(int i = 0; i < _capasity; i++)
        {
            Item _tempItem = _posibleItems[Random.Range(0, _posibleItems.Count)];
            int _tempItemCapasity = Size.GetCapasity(_tempItem.GetSize);
            if (_currentSize - _tempItemCapasity >= 0)
            {
                Items.Add(_tempItem);
                _currentSize -= _tempItemCapasity;

            }
        }

    }

}
