using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "ScriptableObjects/Skin")]
public class Skin : ScriptableObject
{
    public Country[] PurposeCountry => _purposeCountry;
    public Material SkinMaterial => _skinMaterial;
    public bool AllCountries => _allCountries;
    public string SkinName => _skinName;

    [SerializeField] private Country[] _purposeCountry;
    [SerializeField] private Material _skinMaterial;
    [SerializeField] private bool _allCountries;
    [SerializeField] private string _skinName;
}
