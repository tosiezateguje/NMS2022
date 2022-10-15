#if UNITY_SERVER || UNITY_EDITOR //SERVER CODE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Threading.Tasks;




public static class AuthService
{
    static async Task<string> RegisterUserRequest(LoginRequest data)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", data.Username);
        form.AddField("password", data.Password);
        form.AddField("email", data.Email);
        UnityWebRequest www = UnityWebRequest.Post($"http://79.163.163.219:{Server.Instance.PhpServerPort}/register.php", form);
        return await ResponseHandler(www);
    }

    public static async Task<string> LoginUserRequest(LoginRequest data)
    {
        Debug.Log("Requesting MYSQL login");
        WWWForm form = new WWWForm();
        form.AddField("username", data.Username);
        form.AddField("password", data.Password);
        UnityWebRequest www = UnityWebRequest.Post($"http://79.163.163.219:{Server.Instance.PhpServerPort}/login.php", form);
        return await ResponseHandler(www);
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

#endif
