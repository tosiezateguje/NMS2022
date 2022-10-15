#if UNITY_SERVER || UNITY_EDITOR //- SERVER CODE
using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;


public class AuthorizationHandler : NetworkBehaviour
{
    private CustomMessagingManager manager;

    public void Start()
    {
        manager = NetworkManager.Singleton.CustomMessagingManager;
        manager.RegisterNamedMessageHandler("LoginRequest", OnLoginRequest);
        //      manager.RegisterNamedMessageHandler("LoginResponse", OnLoginResponse);
    }

    public async void OnLoginRequest(ulong senderId, FastBufferReader message)
    {
        message.ReadValueSafe(out LoginRequest loginRequest);
        LoginResponse loginResponse = new LoginResponse();
        if (InputValidator.IsUsernameValid(loginRequest.Username) == false)
        {
            loginResponse.ResponseCode = 406;
            loginResponse.ResponseMessage = "Invalid username";
            SendLoginResponse(senderId, loginResponse);
            return;
        }

        if (InputValidator.IsPasswordValid(loginRequest.Password) == false)
        {
            loginResponse.ResponseCode = 406;
            loginResponse.ResponseMessage = "Invalid password";
            SendLoginResponse(senderId, loginResponse);
            return;
        }

        string sqlResponse = await AuthService.LoginUserRequest(loginRequest);
        string code = sqlResponse.Split('\n')[0];
        string response = sqlResponse.Split('\n')[1];

        if (code == "200")
        {
            CharacterData? characterDataNullable = await SQL.CharacterDataRequest(response);
            if (characterDataNullable != null)
            {
                if (SERVER_DATA.GetPlayerByUserId(response, out CharacterData player))
                {
                    loginResponse.ResponseCode = 409;
                    loginResponse.ResponseMessage = "User already logged in";
                    SendLoginResponse(senderId, loginResponse);
                    return;
                }

                CharacterData characterData = (CharacterData)characterDataNullable;
                Entity playerEntity = SpawningManager.SpawnPlayer();
                characterData.NetworkObjectId = playerEntity.NetworkObject.NetworkObjectId;
                SERVER_DATA.AddPlayerToSession(senderId, new Data { CharacterData = characterData, Entity = playerEntity });

                loginResponse.ResponseCode = 200;
                SendLoginResponse(senderId, loginResponse);
                Server.Instance.ServerRequestHandler.CharacterData.SendToClient(senderId, characterData);
                return;
            }

            if (code == "404")
            {
                loginResponse.ResponseCode = 404;
                loginResponse.ResponseMessage = "Invalid username or password";
                SendLoginResponse(senderId, loginResponse);
                return;
            }
        }
    }


    public void SendLoginResponse(ulong clientId, LoginResponse loginResponse)
    {
        using (var buffer = new FastBufferWriter(64, Allocator.Temp))
        {
            buffer.WriteValueSafe(loginResponse);
            manager.SendNamedMessage("LoginResponse", clientId, buffer);
        }
    }

}
#endif