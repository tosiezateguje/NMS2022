using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Threading.Tasks;
using nms;

public class Server : NetworkManager
{



#if UNITY_SERVER || UNITY_EDITOR //S SERVER CODE
    #region PHPServer
    [SerializeField] private string phpServerIP = "79.163.163.219";
    [SerializeField] private string phpServerPort = "7788";
    public string PhpServerIP => phpServerIP;
    public string PhpServerPort => phpServerPort;
    #endregion

    [SerializeField] private CommandHandler commandHandler;
    public CommandHandler CommandHandler => commandHandler;
    public ServerRequestHandler ServerRequestHandler;
    public AuthorizationHandler AuthRequestHandler;
    private void Start()
    { //!REMOVE COMMENT BEFORE BUILD~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
      //StartAsServer();
      //StartHost();
    }
#endif //S SERVER END

    public ClientRequestHandler ClientRequestHandler;

    public void OnNetworkStart()
    {
#if UNITY_SERVER || UNITY_EDITOR //S SERVER CODE
        if (IsServer || IsHost)
        {


            GameObject serverRequestHandlerObject = new GameObject("---SERVER REQUEST HANDLER---");
            serverRequestHandlerObject.AddComponent<NetworkObject>();
            serverRequestHandlerObject.AddComponent<ServerRequestHandler>();
            serverRequestHandlerObject.AddComponent<AuthorizationHandler>();
            ServerRequestHandler = serverRequestHandlerObject.GetComponent<ServerRequestHandler>();
            AuthRequestHandler = serverRequestHandlerObject.GetComponent<AuthorizationHandler>();



            GameObject commandHandlerObject = new GameObject("---COMMAND HANDLER---");
            commandHandlerObject.AddComponent<NetworkObject>();
            commandHandlerObject.AddComponent<CommandHandler>();
            commandHandler = commandHandlerObject.GetComponent<CommandHandler>();
            return;
        }
#endif  //S SERVER END

#if UNITY_EDITOR
        if (IsServer || IsHost)
            return;
#endif

        GameObject clientRequestHandlerObject = new GameObject("---CLIENT REQUEST HANDLER---");
        clientRequestHandlerObject.AddComponent<NetworkObject>();
        clientRequestHandlerObject.AddComponent<ClientRequestHandler>();
        ClientRequestHandler = clientRequestHandlerObject.GetComponent<ClientRequestHandler>();
    }

#if UNITY_SERVER || UNITY_EDITOR //S SERVER CODE

    public void StartAsServer()
    {
        StartServer();
        Debug.Log("Server Started");
        OnNetworkStart();
        OnClientConnectedCallback += OnClientConnected;
    }



    void OnClientConnected(ulong clientId)
    {
        Debug.Log("Client Connected " + clientId);
    }

    public static bool GetObjectById(ulong id, out NetworkObject obj)
    {
        obj = null;
        foreach (NetworkObject networkObject in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList)
        {
            Debug.Log(networkObject.NetworkObjectId);
            if (networkObject.NetworkObjectId == id)
            {
                obj = networkObject;
                return true;
            }
        }
        return false;
    }

#endif //S SERVER END

    public void StartAsClient()
    {
        StartClient();
        if (IsClient)
            Debug.Log("Client Started");
    }

    #region Singleton
    public static Server Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        else
            Instance = this;
    }
    #endregion


}