using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using TMPro;
using Unity.Netcode;
using System;
using System.Threading.Tasks;

public class LoginScreen : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel, characterPanel, loginScreen;
    [SerializeField] private TMP_InputField usernameInput, passwordInput;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] public GameObject ConnectingInfoLabel;

    public async void OnLoginClick()
    {
        #region Initialization
        try
        {
            ConnectingInfoLabel.SetActive(true);
            Server.Singleton.StartClient();
            Server.Instance.OnNetworkStart();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            errorText.text = "Connection Failed";
            return;
        }
        ConnectingInfoLabel.SetActive(false);

        #endregion

        #region Input Validation
        if (usernameInput.text == "" || passwordInput.text == "")
        {
            errorText.text = "Please fill in all fields";
            return;
        }

        if (InputValidator.IsUsernameValid(usernameInput.text) == false)
        {
            errorText.text = "Username is invalid";
            return;
        }

        if (InputValidator.IsPasswordValid(passwordInput.text) == false)
        {
            errorText.text = "Password is invalid";
            return;
        }
        #endregion

        ConnectingInfoLabel.SetActive(true);
        LoginRequest data = new LoginRequest { Username = usernameInput.text, Password = passwordInput.text };
        await Task.Delay(2000);
        ClientAuthorizationHandler.LoginRequest(data);
    }

    public void SetErrorText(string text)
    {
        errorText.text = text;
    }


    // public void ConnectAsServer()
    // {
    //     Server.Instance.StartAsServer();
    // }

    void SetConnectionPanel()
    {
        loginPanel.SetActive(false);
        characterPanel.SetActive(true);
    }




#if UNITY_EDITOR
    public void OnConnectionServer()
    {
        Server.Instance.StartAsServer();
    }
#endif
}
