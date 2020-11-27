using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, RequireComponent(typeof(Baggage))]
public class Passenger : MonoBehaviour
{
    public PurposeTravel GetPurposeTravel => _purposeTravel;
    public Country GetCountry => _country;
    public Baggage Baggage => _baggage;
    public (float tCurrentPosition, float tStopPosition) TPosition => _tPosition;
    public bool IsServed => _isServed;
    public int ServiceReward { get; private set; }
    public int PositionOnQueue;

    [SerializeField] private PurposeTravel _purposeTravel;
    [SerializeField] private Country _country;
    [SerializeField] private Baggage _baggage;
    [SerializeField] private List<Item> _items;
    [SerializeField] private BaggageSize _baggageSize;
    [SerializeField, Range(0f, 1f)] private float _baggageFillRatio;

    private(float tCurrentPosition, float tStopPosition) _tPosition = (0f, 0f);
    private bool _isServed = false;

    public void Init(Country country, PurposeTravel purposeTravel, BaggageSize baggageSize, Skin skin, int maxReward, float baggageFillRatio)
    {
        _country = country;
        _baggageSize = baggageSize;
        _purposeTravel = purposeTravel;
        _baggageFillRatio = baggageFillRatio;
        ServiceReward = (int)(maxReward * _baggageFillRatio);
        _baggage = new Baggage(this, _baggageSize, _baggageFillRatio);
        ApplySkin(skin);
    }

    public void Pay(Player player)
    {
        player.AddMoney(ServiceReward);
        GoExit(player);
    }

    public void GoExit(Player player) 
    { 
        _isServed = true;
        _tPosition.tStopPosition = 1f;
        player.Status = PlayerStatus.Idle;
    }

    public void SetTStopPosition(float tStopValue)
    {
        if (tStopValue >= 0f && tStopValue <= 1f)
            _tPosition.tStopPosition = tStopValue;
    }

    public float SetTCurrentPosition(float tCurrentValue)
    {
        if (tCurrentValue >= 0f && tCurrentValue <= 1f)
            _tPosition.tCurrentPosition = tCurrentValue;
        return _tPosition.tCurrentPosition;
    }

    private void ApplySkin(Skin skin)
    {
        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();

        foreach (MeshRenderer item in meshRenderers)
            item.material = skin.SkinMaterial;

        if(meshRenderer != null)
            meshRenderer.material = skin.SkinMaterial;

    }
}
