using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using nms;


public class ServerRequestHandler : NetworkBehaviour
{
    public RequestHandler<ChatMessage> ChatMessage;
    public RequestHandler<InventorySwap> InventorySwap;
    public RequestHandler<Item> Item;
    public RequestHandler<CharacterData> CharacterData;
    public RequestHandler<PositionUpdate> PositionUpdate;

    void Start()
    {
        ChatMessage = new RequestHandler<ChatMessage>();
        InventorySwap = new RequestHandler<InventorySwap>();
        Item = new RequestHandler<Item>();
        CharacterData = new RequestHandler<CharacterData>();
        PositionUpdate = new RequestHandler<PositionUpdate>();
        
        InventorySwap.OnRequestReceivedWithSenderId += OnInventorySwap;
        ChatMessage.OnRequestReceivedWithSenderId += OnChatMessage;
    }

    public void OnInventorySwap(ulong senderId, InventorySwap inventorySwap)
    {
        int itemSlot1 = inventorySwap.SlotIndex1;
        int itemSlot2 = inventorySwap.SlotIndex2;
        if (itemSlot1 != -1 && itemSlot2 != -1)
        {
            if (SERVER_DATA.PLAYER_DATA[senderId].CharacterData.GetIndexBySlotId(itemSlot1, out int itemIndex1))
            {
                if (SERVER_DATA.PLAYER_DATA[senderId].CharacterData.GetIndexBySlotId(itemSlot2, out int itemIndex2))
                {
                    // If both slots contain items, swap them
                    SERVER_DATA.PLAYER_DATA[senderId].CharacterData.SwapItemSlotsByItemIndex(itemIndex1, itemIndex2);
                    CharacterData.SendToClient(senderId, SERVER_DATA.PLAYER_DATA[senderId].CharacterData);
                }
                else
                {
                    SERVER_DATA.PLAYER_DATA[senderId].CharacterData.MoveItemToSlot(itemIndex1, itemSlot2);
                    CharacterData.SendToClient(senderId, SERVER_DATA.PLAYER_DATA[senderId].CharacterData);
                }
            }
        }
    }

    public void OnChatMessage(ulong senderId, ChatMessage chatMessage)
    {
        Debug.Log(chatMessage.Message + " " + SERVER_DATA.PLAYER_DATA[senderId].CharacterData.CharacterName);
        if (chatMessage.Message[0] == CommandHandler.PREFIX &&
            SERVER_DATA.PLAYER_DATA[senderId].CharacterData.CharacterGameMaster)
            Server.Instance.CommandHandler.Command(senderId, chatMessage.Message);

        else
        {
            chatMessage.Owner = SERVER_DATA.PLAYER_DATA[senderId].CharacterData.CharacterName;
            ChatMessage.SendToAllClient(chatMessage);
        }
    }


}
