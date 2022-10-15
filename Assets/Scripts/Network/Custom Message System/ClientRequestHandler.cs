using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using nms;


public class ClientRequestHandler : NetworkBehaviour
{
    [SerializeField] private ClientAuthorizationHandler clientAuthRequestHandler;
    public RequestHandler<InventorySwap> Inventory;
    public RequestHandler<ChatMessage> ChatMessage;
    public RequestHandler<CharacterData> CharacterData;
    public RequestHandler<PositionUpdate> PositionUpdate;

    void Start()
    {
        clientAuthRequestHandler = gameObject.AddComponent<ClientAuthorizationHandler>();
        Inventory = new RequestHandler<InventorySwap>();
        ChatMessage = new RequestHandler<ChatMessage>();
        CharacterData = new RequestHandler<CharacterData>();
        PositionUpdate = new RequestHandler<PositionUpdate>();
        CharacterData.OnRequestReceived += OnCharacterDataReceived;
        ChatMessage.OnRequestReceived += OnChatMessageReceived;
    }

    public void OnCharacterDataReceived(CharacterData characterData)
    {
        GameManager.Instance.CharacterData = characterData;
        GameManager.Instance.CameraManager.SetPlayerAsTarget();
        GameManager.Instance.Inventory.UpdateInventory();
    }

    public void OnChatMessageReceived(ChatMessage chatMessage) => GameManager.Instance.ChatManager.CreateChatMessage(chatMessage);







}
