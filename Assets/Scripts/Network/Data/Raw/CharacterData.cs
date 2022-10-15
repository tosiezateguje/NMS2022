using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public struct CharacterData : INetworkSerializable
{
    public ulong NetworkObjectId;
    public string UserId;
    public string CharacterName;
    public int CharacterId;
    public int CharacterLevel;
    public int CharacterExp;
    public bool CharacterPremium;
    public bool CharacterGameMaster;
    public CharacterStats Stats;
    public CharacterLocation Location;
    public Item[] Inventory;
    public Item[] ActiveInventory;


    public static CharacterData CreateFromJSON(string jsonString)
    {
        CharacterData data = JsonUtility.FromJson<CharacterData>(jsonString);
        if (data.Inventory == null)
        {
            data.Inventory = new Item[0];
        }

        if (data.ActiveInventory == null)
        {
            data.ActiveInventory = new Item[0];
        }

        for (int i = 0; i < data.Inventory.Length; i++)
        {
            if (data.Inventory[i].ItemStats == null)
            {
                data.Inventory[i].ItemStats = new CharacterStats[0];
            }
        }
        return data;
    }
    #region Getting items

    public bool GetIndexBySlotId(int slotId, out int index)
    {
        index = -1;
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].SlotId == slotId)
            {
                index = i;
                return true;
            }
        }
        return false;
    }

    public bool GetItemBySlotId(int slotId, out Item item)
    {
        item = new Item();
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].SlotId == slotId)
            {
                item = Inventory[i];
                return true;
            }
        }
        return false;
    }

    public bool GetItemByIndex(int index, out Item item)
    {
        item = new Item();
        if (index < Inventory.Length)
        {
            item = Inventory[index];
            return true;
        }
        return false;
    }

    public bool GetItemIndexBySlotId(int slotId, out int index)
    {
        index = -1;
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].SlotId == slotId)
            {
                index = i;
                return true;
            }
        }
        return false;
    }
    #endregion
    #region Swapping items
    public void SwapItemSlotsBySlotId(int slotIndex1, int slotIndex2)
    {
        if (slotIndex1 == slotIndex2)
            return;

        if (this.GetIndexBySlotId(slotIndex1, out int index1))
        {
            if (this.GetIndexBySlotId(slotIndex2, out int index2))
            {
                Inventory[index1].SlotId = slotIndex2;
                Inventory[index2].SlotId = slotIndex1;
            }
            else
            {
                Inventory[index1].SlotId = slotIndex2;
            }
        }
    }

    public void MoveItemToSlot(int itemIndex, int slotId) => Inventory[itemIndex].SlotId = slotId;

    public void SwapItemSlotsByItemIndex(int itemIndex1, int itemIndex2)
    {
        if (itemIndex1 == itemIndex2)
            return;

        Debug.Log("Swapping item slots: " + itemIndex1 + " and " + itemIndex2);
        int temp = Inventory[itemIndex1].SlotId;
        this.Inventory[itemIndex1].SlotId = this.Inventory[itemIndex2].SlotId;
        this.Inventory[itemIndex2].SlotId = temp;
    }
    #endregion


    #region INetworkSerializable
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref NetworkObjectId);
        serializer.SerializeValue(ref UserId);
        serializer.SerializeValue(ref CharacterName);
        serializer.SerializeValue(ref CharacterId);
        serializer.SerializeValue(ref CharacterLevel);
        serializer.SerializeValue(ref CharacterExp);
        serializer.SerializeValue(ref CharacterPremium);
        serializer.SerializeValue(ref CharacterGameMaster);
        serializer.SerializeValue(ref Stats);
        serializer.SerializeValue(ref Location);
        // serializer.SerializeValue(ref Inventory);
        // serializer.SerializeValue(ref ActiveInventory);



        #region Inventory

        int inventoryLength = 0;
        if (!serializer.IsReader)
        {
            if (Inventory != null || Inventory.Length > 0)
            {
                inventoryLength = Inventory.Length;
            }
        }

        serializer.SerializeValue(ref inventoryLength);

        if (serializer.IsReader)
        {
            if (inventoryLength > 0)
            {
                Inventory = new Item[inventoryLength];
            }
        }

        for (int i = 0; i < inventoryLength; ++i)
        {
            serializer.SerializeValue(ref Inventory[i]);
        }


        #endregion

        #region ActiveInventory

        int activeInventoryLength = 0;
        if (!serializer.IsReader)
        {
            if (ActiveInventory != null || ActiveInventory.Length > 0)
            {
                activeInventoryLength = ActiveInventory.Length;
            }
        }

        serializer.SerializeValue(ref activeInventoryLength);

        if (serializer.IsReader)
        {
            if (activeInventoryLength > 0)
            {
                ActiveInventory = new Item[activeInventoryLength];
            }
        }

        for (int i = 0; i < activeInventoryLength; ++i)
        {
            serializer.SerializeValue(ref ActiveInventory[i]);
        }

        #endregion
    }
    #endregion
}

