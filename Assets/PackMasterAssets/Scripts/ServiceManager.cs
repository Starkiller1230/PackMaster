using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ServiceManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _leftSideBaggage;
    [SerializeField] private GridLayoutGroup _rightSideBaggage;
    [SerializeField] private GridLayoutGroup _playerItemPanel;
    [SerializeField] private InventoryCell _emptyCellPrefab;
    [SerializeField] private Transform _draggingParent;
    [SerializeField] private Vector2 _cellSizeVec;
    [SerializeField] private float _cellSpacing;

    private Player _player;
    private Passenger _passenger;
    private Transform[] _dropItemContainers;
    private Item[] _leftBaggageSideItems;
    private Item[] _rightBaggageSideItems;
    private Item[] _playerItems;
    private (int width, int height) _sizeArrayItems;

    public void LoadPassenger(Passenger passenger, Player player)
    {
        _passenger = passenger;
        _player = player;
        _player.Status = PlayerStatus.ServicePassenger;

        // Get size item array.
        _sizeArrayItems = Size.GetSize(_passenger.Baggage.GetBaggageSize);

        // Filling array with items.
        Item[] _baggageItemsList = FillArrayWithRandomItems(_sizeArrayItems.width * _sizeArrayItems.height);

        SmoothItemsListOnPanels(_baggageItemsList);
        Vector2 _rectSize = _leftSideBaggage.GetComponent<RectTransform>().rect.size;

        // Set sell settings.
        Vector2 _cellSizeVec = _rectSize / 4.2f;
        _leftSideBaggage.cellSize = _cellSizeVec;
        _rightSideBaggage.cellSize = _leftSideBaggage.cellSize;
        _playerItemPanel.cellSize = _leftSideBaggage.cellSize;

        // Filling panels with items list.
        FillBaggagePanelList(_leftSideBaggage.transform, _leftBaggageSideItems, false);
        FillBaggagePanelList(_rightSideBaggage.transform, _rightBaggageSideItems, false);
        FillBaggagePanelList(_playerItemPanel.transform, _playerItems, true);

    }

    public void PackBaggage()
    {
        foreach (Transform item in _playerItemPanel.transform)
            if(item.childCount != 0)
                return;

        for (int i = 0; i < _leftSideBaggage.transform.childCount - 1; i++)
            if (_leftSideBaggage.transform.GetChild(i).childCount != 0 &&  _rightSideBaggage.transform.GetChild(i).childCount != 0)
                return;


        _player.Status = PlayerStatus.Idle;
        _passenger.Pay(_player);
        Debug.Log("Pack baggage is sucsess.");
        ClearPanels();
    }

    private void Awake()
    {
        _dropItemContainers = new Transform[] { _leftSideBaggage.transform, _rightSideBaggage.transform, _playerItemPanel.transform };
        _player.OnChangePlayerStatus += ChangePlayerStatus;
    }

    private void ChangePlayerStatus(PlayerStatus playerStatus)
    {
        if(playerStatus != PlayerStatus.ServicePassenger)
            ClearPanels();
    }

    private void FillBaggagePanelList(Transform container, Item[] items, bool canDrugItem)
    {
        foreach (Transform item in container)
            Destroy(item);

        foreach (Item item in items)
        {
            InventoryCell _cell = Instantiate(_emptyCellPrefab, container);
            if (item != null)
                _cell.Item.Init(item, _draggingParent, canDrugItem, _dropItemContainers);
            else
                Destroy(_cell.Item.gameObject);
        }

    }

    private void SmoothItemsListOnPanels(Item[] baggageItems)
    {
        if (_playerItemPanel == null || _leftSideBaggage == null || _rightSideBaggage == null)
            throw new System.Exception("One of baggage panels is null");

        _leftBaggageSideItems = new Item[_sizeArrayItems.width * _sizeArrayItems.height];
        _rightBaggageSideItems = new Item[_sizeArrayItems.width * _sizeArrayItems.height];
        _playerItems = new Item[_sizeArrayItems.width * _sizeArrayItems.height];

        bool _playerItem = true;
        foreach (Item item in baggageItems)
        {
            if (item == null)
                continue;

            int _i = Random.Range(0, 4);
            if (_playerItem == true)
                _i = 2;

            if (_i == 0)
                PasteItemInArray(item, _leftBaggageSideItems, _rightBaggageSideItems);
            else if(_i == 1)
                PasteItemInArray(item, _rightBaggageSideItems, _leftBaggageSideItems);
            else
                PasteItemInArray(item, _playerItems, null);
            _playerItem = false;
        }

        void PasteItemInArray(Item item, Item[] itemList, Item[] checkToMathList)
        {
            bool _isPasted = false;
            int _sizeListItems = _sizeArrayItems.width * _sizeArrayItems.height;

            while (_isPasted == false)
            {
                //Get random item.
                int _i = Random.Range(0, _sizeListItems);

                if (itemList[_i] != null)
                    continue;

                // Check to compare.
                if (checkToMathList != null)
                {
                    int _checkNumberItem = _sizeListItems - _i - 1;
                    if (checkToMathList[_i] == null)
                    {
                        itemList[_i] = item;
                        _isPasted = true;
                    }
                }
                else
                {
                    itemList[_i] = item;
                    _isPasted = true;
                }
            }
        }
    }

    private Item[] FillArrayWithRandomItems(int size)
    {
        Item[] items = new Item[size];

        int _currentIndexItem = _passenger.Baggage.Items.Count - 1;

        if (_passenger.Baggage.Items.Count > size)
            throw new System.Exception("Items more capacity");

        while (_currentIndexItem >= 0)
        {
            int i = Random.Range(0, size);
            if (items[i] == null)
                items[i] = _passenger.Baggage.Items[_currentIndexItem--];
        }

        return items;
    }

    private void ClearPanels()
    {
        for (int i = 0; i < _dropItemContainers.Length; i++)
        {
            for (int j = 0; j < _dropItemContainers[i].childCount; j++)
                Destroy(_dropItemContainers[i].GetChild(j).gameObject);
        }
    }

}
