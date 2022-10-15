#if UNITY_SERVER || UNITY_EDITOR //S SERVER CODE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public static class SERVER_DATA
{

    static Dictionary<ulong, Data> playerData = new Dictionary<ulong, Data>();
    public static Dictionary<ulong, Data> PLAYER_DATA => playerData;



    public static void AddPlayerToSession(ulong senderId, Data data)
    {
        playerData.Add(senderId, data);
    }


    public static bool GetPlayerByUserId(string userId, out CharacterData player)
    {
        player = new CharacterData();
        foreach (KeyValuePair<ulong, Data> dictionaryEntry in SERVER_DATA.PLAYER_DATA)
        {
            if (dictionaryEntry.Value.CharacterData.UserId == userId)
            {
                player = dictionaryEntry.Value.CharacterData;
                return true;
            }
        }
        return false;
    }

}

public struct Data
{
    public CharacterData CharacterData;
    public Entity Entity;
}


#endif //S SERVER END