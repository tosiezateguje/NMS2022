using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Dictionary<int, Sprite> ItemIcons;
    public static ItemCell[] ITEM_CELLS;

    [SerializeField] private GameObject activeInventoryPanel;

    private void Start()
    {
        // Set the item icons
        LoadItemIcons();

        // Set the item cells
        ITEM_CELLS = new ItemCell[gameObject.transform.childCount + activeInventoryPanel.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform itemCellTransform = gameObject.transform.GetChild(i);
            ITEM_CELLS[i] = new ItemCell()
            {
                ItemRarity = itemCellTransform.GetComponent<Image>(),
                IconObject = itemCellTransform.GetChild(1).gameObject,
                ItemIcon = itemCellTransform.GetChild(1).GetComponent<Image>(),
                ItemAmount = itemCellTransform.GetChild(2).GetComponent<TMP_Text>()
            };
        }

        for(int i=0; i< activeInventoryPanel.transform.childCount; i++)
        {
            Transform itemCellTransform = activeInventoryPanel.transform.GetChild(i);
            ITEM_CELLS[gameObject.transform.childCount+i] = new ItemCell()
            {
                ItemRarity = itemCellTransform.GetComponent<Image>(),
                IconObject = itemCellTransform.GetChild(1).gameObject,
                ItemIcon = itemCellTransform.GetChild(1).GetComponent<Image>(),
                ItemAmount = itemCellTransform.GetChild(2).GetComponent<TMP_Text>()
            };
        }
    }



    void LoadItemIcons()
    {
        ItemIcons = new Dictionary<int, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Items");
        foreach (Sprite sprite in sprites)
            ItemIcons.Add(int.Parse(sprite.name), sprite);

    }


    public void UpdateInventory()
    {
        if (GameManager.Instance.CharacterData.Inventory == null)
        {
            return;
        }

        for (int i = 0; i < ITEM_CELLS.Length; i++)
        {
            ITEM_CELLS[i].ItemAmount.text = "";
            ITEM_CELLS[i].IconObject.SetActive(false);
            ITEM_CELLS[i].ItemRarity.color = ITEM_COLORS.COMMON;
            ITEM_CELLS[i].ItemIcon.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }

        for (int i = 0; i < GameManager.Instance.CharacterData.Inventory.Length; i++)
        {
            int slotId = GameManager.Instance.CharacterData.Inventory[i].SlotId;
            switch (GameManager.Instance.CharacterData.Inventory[i].ItemRarity)
            {
                case 0:
                    ITEM_CELLS[slotId].ItemRarity.color = ITEM_COLORS.COMMON;
                    break;
                case 4:
                    ITEM_CELLS[slotId].ItemRarity.color = ITEM_COLORS.LEGENDARY;
                    break;
            }

            ITEM_CELLS[slotId].IconObject.SetActive(true);
            ITEM_CELLS[slotId].ItemIcon.sprite = ItemIcons[GameManager.Instance.CharacterData.Inventory[i].ItemId];
            ITEM_CELLS[slotId].ItemAmount.text = GameManager.Instance.CharacterData.Inventory[i].ItemAmount.ToString();
        }


    }
}





[System.Serializable]
public struct ItemCell
{
    public Image ItemRarity;
    public GameObject IconObject;
    public Image ItemIcon;
    public TMP_Text ItemAmount;
}