using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] CameraManager cameraManager;
    [SerializeField] InputManager inputManager;
    [SerializeField] ParticleManager particleManager;
    [SerializeField] GameObject server;
    [SerializeField] ChatManager chatManager;
    [SerializeField] Inventory inventory;
    [SerializeField] EntityList entityList;

    public CameraManager CameraManager => cameraManager;
    public ParticleManager ParticleManager => particleManager;
    public ChatManager ChatManager => chatManager;
    public InputManager InputManager => inputManager;
    public GameObject Server => server;
    public CharacterData CharacterData;
    public Inventory Inventory => inventory;
    public EntityList EntityList => entityList;

    
    

  
}
