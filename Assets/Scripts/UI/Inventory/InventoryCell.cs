using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IDragHandler
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject cellIcon;
    [SerializeField] private int index;
    [SerializeField] private bool isActiveInventory;
    public int Index => index;

    public static Action<int> OnMouseOver;
    public static Action OnMouseExit;
    public static Action<int> OnItemSelection;
    public static Action OnItemDeselection;


    void Start()
    {
        cellIcon = gameObject.transform.GetChild(1).gameObject;
        if (isActiveInventory)
        {
            index = 48 + transform.GetSiblingIndex();
        }
        else
        {
            index = transform.GetSiblingIndex();
            inventory = GetComponentInParent<Inventory>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => OnMouseOver?.Invoke(index);
    public void OnPointerExit(PointerEventData eventData) => OnMouseExit?.Invoke();




    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventoryCell dropCell = eventData.pointerDrag.GetComponent<InventoryCell>();
            if (dropCell != null)
            {
                Server.Instance.ClientRequestHandler.Inventory.SendToServer(
                    new InventorySwap
                    {
                        SlotIndex1 = dropCell.Index,
                        SlotIndex2 = index
                    });
            }
        }

        OnItemDeselection?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventoryCell dragCell = eventData.pointerDrag.GetComponent<InventoryCell>();
            if (!dragCell.cellIcon.activeSelf)
                return;

            if (dragCell != null)
            {
                OnItemSelection?.Invoke(index);
            }
        }
    }

}
