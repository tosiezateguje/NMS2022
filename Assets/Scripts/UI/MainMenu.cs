using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;



public class MainMenu : NetworkBehaviour
{
    [SerializeField] TMP_InputField nameInputField, ipInputField, portInputField;
    [SerializeField] UnityTransport unityTransport;

    void OnEnable()
    {
        unityTransport = Server.Singleton.gameObject.GetComponent<UnityTransport>();
    }


    void SetConnectionParameters() => unityTransport.SetConnectionData(ipInputField.text, ushort.Parse(portInputField.text));



    public bool ValidateInput()
    {
        if (nameInputField.text == "")
            return false;
        if (ipInputField.text == "")
            return false;
        if (portInputField.text == "")
            return false;

        return true;
    }


    public void OnClientClick()
    {
        //  if (!ValidateInput())
        // return;

        //  SetConnectionParameters();
        Server.Singleton.StartClient();
    }
}
