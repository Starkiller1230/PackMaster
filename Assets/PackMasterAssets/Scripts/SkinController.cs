using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    [SerializeField] private Skin[] _skins;

    public Skin GenerateSkin(Country country)
    {
        List<Skin> _selectedSkins = new List<Skin>();
        foreach (Skin skin in _skins)
        {
            if(skin.AllCountries == true)
            {
                _selectedSkins.Add(skin);
            }
            else
            {
                foreach (var item in skin.PurposeCountry)
                    if (item == country)
                        _selectedSkins.Add(skin);
            }
        }

        if (_selectedSkins.Count == 1)
            return _selectedSkins[0];
        else if (_selectedSkins.Count > 1)
            return _selectedSkins[Random.Range(0, _selectedSkins.Count)];
        else
            throw new System.Exception("Not matched skin");

    }

}
