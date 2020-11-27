using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject, IItem
{
    public static List<Item> AllItemList { get; private set; } = new List<Item>();
    public List<Country> GetAllowedCountries => _allowedCountries;
    public List<PurposeTravel> GetAllowedPurposseTravel => _allowedPurposesTravel;
    public Sprite UIIcon => _spriteItem;
    public ItemSize GetSize => _itemSize;
    public bool IsAllCountries => _allCountries;
    public bool IsAllPurposesTravel => _allPurposeTravel;
    public string Name => _name;

    [SerializeField, Tooltip("Расы у которых может быть предмет")]              private List<Country> _allowedCountries;
    [SerializeField, Tooltip("Причины отдыха при которых может быть предмет")]  private List<PurposeTravel> _allowedPurposesTravel;
    [SerializeField] private ItemSize _itemSize;
    [SerializeField] private Sprite _spriteItem;
    [SerializeField] private bool _allCountries;
    [SerializeField] private bool _allPurposeTravel;
    [SerializeField] private string _name;

    private void OnEnable()
    {
        AllItemList.Add(this);
    }
    
    private void OnDisable()
    {
        AllItemList.Remove(this);
    }

}
