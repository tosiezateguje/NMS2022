using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DragMouseHolder : MonoBehaviour
{

    [SerializeField] private Canvas parentCanvas;

    [SerializeField] private Image dragHolderImage;
    private RectTransform dragHolderRectTransform;



    void Start()
    {
        // Get the drag holder image and rect transform
        dragHolderRectTransform = gameObject.GetComponent<RectTransform>();
        dragHolderImage = gameObject.GetComponent<Image>();
        gameObject.SetActive(false);

        InventoryCell.OnItemSelection += SelectItem;
        InventoryCell.OnItemDeselection += DeselectItem;
    }

    private void OnDestroy()
    {
        InventoryCell.OnItemSelection -= SelectItem;
        InventoryCell.OnItemDeselection -= DeselectItem;
    }

    public void SelectItem(int index)
    {
        gameObject.SetActive(true);
        dragHolderImage.sprite = Inventory.ITEM_CELLS[index].ItemIcon.sprite;
    }

    public void DeselectItem()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(dragHolderRectTransform, Input.mousePosition, parentCanvas.worldCamera, out Vector3 pos))
            {
                dragHolderRectTransform.position = pos;
            }
        }
    }

}
