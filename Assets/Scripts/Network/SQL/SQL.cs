#if UNITY_SERVER || UNITY_EDITOR //SERVER CODE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Threading.Tasks;

public static class SQL
{
    public static async Task<CharacterData?> CharacterDataRequest(string userId)
    {
        Debug.Log("Requesting MYSQL character data for " + userId);
        WWWForm form = new WWWForm();
        form.AddField("user_id", userId);
        UnityWebRequest www = UnityWebRequest.Post($"http://79.163.163.219:7788/cdata.php", form);
        www.SendWebRequest();
        while (!www.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            string error = www.error;
            www.Dispose();
            return null;
        }

        else
        {
            Debug.Log(www.downloadHandler.text);
            CharacterData data = CharacterData.CreateFromJSON(www.downloadHandler.text);
            www.Dispose();
            return data;
        }

        
    }




    static async Task<string> ResponseHandler(UnityWebRequest www)
    {
        www.SendWebRequest();
        while (!www.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            string error = www.error;
            www.Dispose();
            return error;
        }

        else
        {
            string response = www.downloadHandler.text;
            www.Dispose();
            return response;
        }
    }


}

#endif //SERVER CODE