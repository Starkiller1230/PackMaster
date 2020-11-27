using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Image _itemSprite;
    [SerializeField] private float _offsetY;

    private Transform[] _dropContentItemPanels;
    private Transform _originalParent;
    private Transform _draggingParent;
    private Vector3 _positionOnDragging;
    private bool _canDrugItem;

    public void Init(IItem item, Transform draggingParent, bool canDrugItem, params Transform[] dropContentItemPanels)
    {
        Render(item);
        _dropContentItemPanels = dropContentItemPanels;
        _draggingParent = draggingParent;
        _originalParent = transform.parent;
        _canDrugItem = canDrugItem;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_canDrugItem == false)
            return;

        _originalParent = transform.parent;
        transform.parent = _draggingParent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canDrugItem == false)
            return;

        _positionOnDragging = eventData.position;
        _positionOnDragging.y += _offsetY;
        transform.position = _positionOnDragging;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_canDrugItem == false)
            return;

        PasteItemCellOnInventoryCell(_dropContentItemPanels);
    }

    private void Render(IItem item)
    {
        _itemName.text = item.Name;
        _itemSprite.sprite = item.UIIcon;
    }

    private void PasteItemCellOnInventoryCell(params Transform[] parents)
    {
        Transform _parent = _originalParent;
        float _distanceToParent = Vector3.Distance(_positionOnDragging, _originalParent.parent.position);

        //Find closest cell.
        for (int i = 0; i < parents.Length; i++)
        {
            for (int j = 0; j < parents[i].childCount; j++)
            {
                float _distanceToNewParent = Vector3.Distance(_positionOnDragging, parents[i].GetChild(j).position);
                if (_distanceToNewParent < _distanceToParent)
                {
                    _parent = parents[i].GetChild(j);
                    _distanceToParent = _distanceToNewParent;
                }
            }

        }

        //Checking item in cell.
        if (_parent.childCount == 0)
            SetParent(_parent);
        else
            SetParent(_originalParent);

        void SetParent(Transform parent)
        {
            transform.parent = parent;
            transform.position = parent.GetComponent<RectTransform>().position;
        }
    }


}
