using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

[System.Serializable]
public struct Item : INetworkSerializable
{
    public int ItemId;
    public string ItemName;
    public int ItemAmount;
    public int SlotId;
    public Image ItemIcon;
    public int ItemRarity;
    public CharacterStats[] ItemStats;
    public bool IsUsable;
    public bool IsEquippable;
    public bool IsLevelDependent;
    public int LevelRequirement;
    public bool IsStackable;
    public int ItemMaxStack;
    public bool IsConsumable;
    public int ItemValue;
    public string ItemDescription;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ItemId);
        serializer.SerializeValue(ref ItemName);
        serializer.SerializeValue(ref ItemAmount);
        serializer.SerializeValue(ref SlotId);
        serializer.SerializeValue(ref ItemRarity);
    
        serializer.SerializeValue(ref IsUsable);
        serializer.SerializeValue(ref IsEquippable);
        serializer.SerializeValue(ref IsLevelDependent);
        serializer.SerializeValue(ref LevelRequirement);
        serializer.SerializeValue(ref IsStackable);
        serializer.SerializeValue(ref ItemMaxStack);
        serializer.SerializeValue(ref IsConsumable);
        serializer.SerializeValue(ref ItemValue);

        serializer.SerializeValue(ref ItemStats);
        serializer.SerializeValue(ref ItemDescription);

        int size = 0;
        if (!serializer.IsReader)
        {
            if (ItemStats != null || ItemStats.Length > 0)
            {
                size = ItemStats.Length;
            }
        }

        serializer.SerializeValue(ref size);

        if (serializer.IsReader)
        {
            if (size > 0)
            {
                ItemStats = new CharacterStats[size];
            }
        }

        for (int i = 0; i < size; ++i)
        {
            serializer.SerializeValue(ref ItemStats[i]);
        }
    }

    public List<string> GetAllItemStats()
    {
        List<string> stats = new List<string>();
        List<string> addictionalList = new List<string>();
        for (int i = 0; i < ItemStats.Length; i++)
        {
            addictionalList = ItemStats[i].GetItemStat();
            foreach(string stat in addictionalList)
                stats.Add(stat);
        }

        return stats;
    }
}
