using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class InputValidator
{
    private static string[] forbiddenChars = { " ", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "=", "+", "[", "]", "{", "}", ";", ":", "'", "\"", ",", "<", ".", ">", "/", "?", "`", "~", "|" };
    public static bool IsUsernameValid(string username)
    {
        foreach (string c in forbiddenChars)
        {
            if (username.Contains(c))
                return false;
        }

        if (username.Length < 3 || username.Length > 16)
            return false;
       

            return true;
    }

    public static bool IsPasswordValid(string password)
    {
        if (password.Length < 3 || password.Length > 16)
            return false;
     

            return true;
    }

    public static bool IsEmailValid(string email)
    {
        if (email.Length < 3 || email.Length > 32)
            return false;
      

            return true;
    }
}