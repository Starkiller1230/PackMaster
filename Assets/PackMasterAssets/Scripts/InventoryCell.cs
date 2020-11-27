using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryCell : MonoBehaviour
{
    public ItemCell Item => _itemCell;
    [SerializeField] private ItemCell _itemCell;
    [SerializeField] private float _offsetY;

}
