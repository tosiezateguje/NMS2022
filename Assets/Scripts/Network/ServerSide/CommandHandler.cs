#if UNITY_EDITOR || UNITY_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class CommandHandler : NetworkBehaviour
{
    public static readonly char PREFIX = '/';

    private EntityList entityList;
    private void Start()
    {
        entityList = Resources.Load<EntityList>("EntityList");
    }

    public void Command(ulong senderId, string command)
    {
        if (command == "") return;
        if (command[0] != PREFIX) return;
        command = command.Remove(0, 1);
        string commandMethod = command.Split(' ')[0];
        string commandArgument = "";
        string commandArgument2 = "";
        if (command.Split(' ').Length > 1)
            commandArgument = command.Split(' ')[1];
        if (command.Split(' ').Length > 2)
            commandArgument2 = command.Split(' ')[2];
        switch (commandMethod)
        {
            case "kick":
                Kick(commandArgument);
                break;
            case "pkick":
                PublicKick(commandArgument);
                break;
            case "playerlist":
                PlayerList(senderId);
                break;
            case "entitylist":
                EntityList(senderId, commandArgument);
                break;
            case "spawn":
                SpawnEntity(senderId, commandArgument);
                break;
            case "set":
                SetDestination(senderId, commandArgument, commandArgument2);
                break;
        }

    }

    void Kick(string characterName)
    {
        if (characterName == "")
        {
            Debug.Log("No character name specified");
            return;
        }
        foreach (var player in SERVER_DATA.PLAYER_DATA)
        {
            if (player.Value.CharacterData.CharacterName == characterName)
            {
                Server.Instance.DisconnectClient(player.Key);
                Debug.Log("Player " + characterName + " has been kicked");
                return;
            }
        }
    }

    void PublicKick(string characterName)
    {
        if (characterName == "")
        {
            Debug.Log("No character name specified");
            return;
        }
        foreach (var player in SERVER_DATA.PLAYER_DATA)
        {
            if (player.Value.CharacterData.CharacterName == characterName)
            {
                Server.Instance.DisconnectClient(player.Key);
                Debug.Log("Player " + characterName + " has been kicked");
                Server.Instance.ServerRequestHandler.ChatMessage.SendToAllClient(new ChatMessage() { Owner = "Server", Message = characterName + " has been kicked" });
                return;
            }
        }
    }

    void PlayerList(ulong senderId)
    {
        string playerList = "Player List: \n";
        foreach (var character in SERVER_DATA.PLAYER_DATA)
        {
            playerList += character.Value.CharacterData.CharacterName + " ";
        }
        Server.Instance.ServerRequestHandler.ChatMessage.SendToClient(senderId, new ChatMessage() { Owner = "Server", Message = playerList });
    }

    void EntityList(ulong senderId, string argument)
    {
        string query = "Entity List: \n";
        if (argument == "-all")
        {
            for (int i = 0; i < entityList.Entities.Count; i++)
            {
                query += i + " " + entityList.Entities[i].name + "\n";
            }
        }

        else
        {
            foreach (var entity in Server.Instance.SpawnManager.SpawnedObjectsList)
            {
                query += entity.name + " " + entity.GetInstanceID() + "\n";
            }
        }

        Server.Instance.ServerRequestHandler.ChatMessage.SendToClient(senderId, new ChatMessage() { Owner = "Server", Message = query });
    }

    void SpawnEntity(ulong senderId, string argument)
    {
        string query = "";
        GameObject entity;

        if (argument == "")
            query += "No entity specified";

        else if (!entityList.GetEntityByIndex(int.Parse(argument), out entity))
            query += "Entity not found";

        else
        {
            var networkObject = Instantiate(entity).GetComponent<NetworkObject>();
            networkObject.Spawn();
            query += $"Entity {entity.name} spawned";
        }

        Server.Instance.ServerRequestHandler.ChatMessage.SendToClient(senderId, new ChatMessage() { Owner = "Server", Message = query });
    }

    void SetDestination(ulong senderId, string objectId, string destination)
    {
        int id = int.Parse(objectId);
        Vector3 dest = new Vector3(float.Parse(destination.Split(',')[0]), float.Parse(destination.Split(',')[1]), float.Parse(destination.Split(',')[2]));
        foreach(var entity in Server.Instance.SpawnManager.SpawnedObjectsList)
        {
            Debug.Log(entity.GetInstanceID());
            if(entity.GetInstanceID() == id)
            {
                entity.gameObject.GetComponent<MoveableEntity>().SetDestination(dest);
                Server.Instance.ServerRequestHandler.ChatMessage.SendToClient(senderId, new ChatMessage() { Owner = "Server", Message = "Destination set for " + entity.name });
            }
        }
    }



}
#endif