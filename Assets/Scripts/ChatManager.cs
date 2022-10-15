using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using nms;


public class ChatManager : MonoBehaviour
{
    [SerializeField] GameObject chatPanel, chatMessages, chatMessagePrefab, chatInput;
    [SerializeField] TMP_InputField chatInputField;

    public void ToggleChatPanel()
    {
        chatInput.SetActive(!chatInput.activeSelf);
        if (chatInput.activeSelf)
            chatInputField.ActivateInputField();


        if (chatInputField.text != "")
        {
            Server.Instance.ClientRequestHandler.ChatMessage.SendToServer(new ChatMessage { Message = chatInputField.text });
        }
    }

    public void CreateChatMessage(ChatMessage data)
    {
        Debug.Log("Message Received");
        GameObject chatMessage = Instantiate(chatMessagePrefab, chatMessages.transform);
        chatMessage.transform.SetParent(chatMessages.transform);
        chatMessage.transform.GetChild(0).GetComponent<TMP_Text>().text = data.Owner;
        chatMessage.transform.GetChild(1).GetComponent<TMP_Text>().text = data.Message;
        chatInputField.text = "";
    }

    public void OnNetworkStart()
    {
        Debug.Log(Server.Instance.ClientRequestHandler.ChatMessage.CustomMessage);
        Server.Instance.ClientRequestHandler.ChatMessage.OnRequestReceived += CreateChatMessage;
    }



}
