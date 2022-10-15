using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using System.Text.RegularExpressions;

public class ItemHoverPanel : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private bool hoverCooldown = false;

    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Image itemRarityFrame;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text levelRequirement;
    [SerializeField] private TMP_Text[] itemStats;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Material itemRarityMaterial;

    private float time = 0.05f;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SetItemRarityProperties(2);
        InventoryCell.OnMouseOver += ShowPanel;
        InventoryCell.OnMouseExit += HidePanel;
    }

    private void OnDestroy()
    {
        InventoryCell.OnMouseOver -= ShowPanel;
        InventoryCell.OnMouseExit -= HidePanel;
    }



    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = 0.05f;
            hoverCooldown = false;
        }
    }

    public void ShowPanel(int index)
    {
        if (hoverCooldown)
            return;

        if (GameManager.Instance.CharacterData.GetItemBySlotId(index, out Item item))
        {
            ClearItemStatsPanel();
            gameObject.SetActive(true);
            SetItemRarityProperties(item.ItemRarity);
            Vector3 pos = Inventory.ITEM_CELLS[index].ItemRarity.gameObject.transform.position;
            rectTransform.position = pos + new Vector3(-rectTransform.sizeDelta.x / 90, 0, 0);

            itemName.text = item.ItemName;
            if (item.IsLevelDependent)
            {
                levelRequirement.gameObject.SetActive(true);
                levelRequirement.text = "Required level: " + item.LevelRequirement;
            }
            else
                levelRequirement.gameObject.SetActive(false);


            List<string> stats = item.GetAllItemStats();
            for (int i = 0; i < stats.Count; i++)
            {
                itemStats[i].gameObject.SetActive(true);
                itemStats[i].text = stats[i];
            }
            itemDescription.text = item.ItemDescription;


            hoverCooldown = true;
        }
    }

    void SetItemRarityProperties(int rarity)
    {
        switch (rarity)
        {
            case 0:
                itemName.color = ITEM_COLORS.COMMON;
                itemRarityFrame.color = ITEM_COLORS.COMMON;
                itemRarityMaterial.color = ITEM_COLORS.COMMON_BACKGROUND;
                break;
            case 1:
                itemName.color = ITEM_COLORS.UNCOMMON;
                itemRarityFrame.color = ITEM_COLORS.UNCOMMON;
                itemRarityMaterial.color = ITEM_COLORS.UNCOMMON_BACKGROUND;
                break;
            case 2:
                itemName.color = ITEM_COLORS.RARE;
                itemRarityFrame.color = ITEM_COLORS.RARE;
                itemRarityMaterial.color = ITEM_COLORS.RARE_BACKGROUND;
                break;
            case 3:
                itemName.color = ITEM_COLORS.HEROIC;
                itemRarityFrame.color = ITEM_COLORS.HEROIC;
                itemRarityMaterial.color = ITEM_COLORS.HEROIC_BACKGROUND;
                break;
            case 4:
                itemName.color = ITEM_COLORS.MYTHIC;
                itemRarityFrame.color = ITEM_COLORS.MYTHIC;
                itemRarityMaterial.color = ITEM_COLORS.MYTHIC_BACKGROUND;
                break;
            case 5:
                itemName.color = ITEM_COLORS.LEGENDARY;
                itemRarityFrame.color = ITEM_COLORS.LEGENDARY;
                itemRarityMaterial.color = ITEM_COLORS.LEGENDARY_BACKGROUND;
                break;
        }
    }


    void ClearItemStatsPanel()
    {
        itemName.text = "";
        itemDescription.text = "";
        levelRequirement.text = "";
        for (int i = 0; i < itemStats.Length; i++)
        {
            itemStats[i].gameObject.SetActive(false);
        }
    }

    public void HidePanel()
    {
        if (hoverCooldown)
            return;

        gameObject.SetActive(false);
    }


}
