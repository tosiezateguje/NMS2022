using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class ClientAuthorizationHandler : NetworkBehaviour
{
    [SerializeField] private LoginScreen loginScreen;
    [SerializeField] private GameObject loginScreenObject;

    void Start()
    {
        loginScreen = FindObjectOfType<LoginScreen>();
        loginScreenObject = loginScreen.gameObject;
        NetworkManager.CustomMessagingManager.RegisterNamedMessageHandler("LoginResponse", LoginResponse);
    }

    void LoginResponse(ulong senderId, FastBufferReader message)
    {
        Debug.Log("Login response received from " + senderId);
        loginScreen.ConnectingInfoLabel.SetActive(false);
        message.ReadValueSafe(out LoginResponse loginResponse);

        if (loginResponse.ResponseCode == 200)
            loginScreenObject.SetActive(false);
        else
            loginScreen.SetErrorText(loginResponse.ResponseMessage);

    }


    public static void LoginRequest(LoginRequest data)
    {
        Debug.Log("Requesting login with username " + data.Username + " and password " + data.Password);
        using (var writer = new FastBufferWriter(1100, Allocator.Temp))
        {
            writer.WriteValueSafe(data);
            Server.Instance.CustomMessagingManager.SendNamedMessage("LoginRequest", NetworkManager.ServerClientId, writer);
        }
    }

    // Update is called once per frame
}
