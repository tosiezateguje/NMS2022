using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class ConnectingInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text connectingInfoText;
    private bool cooldown = false;

    void FixedUpdate()
    {
        if(cooldown)
            return;

        Void(100);
    }

    async void Void(int time)
    {
        cooldown = true;
        connectingInfoText.text = "Connecting.";
        await Task.Delay(time);
        connectingInfoText.text = "Connecting..";
        await Task.Delay(time);
        connectingInfoText.text = "Connecting...";
        await Task.Delay(time);
        cooldown = false;
    }

}
