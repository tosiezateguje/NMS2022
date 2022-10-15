using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class InputManager : NetworkBehaviour
{
    private Inpt inpt;
    private InputAction mouseRightClick, mouseScroll;
    private InputAction chatToggle;
    private CameraManager cameraManager;
    private ClientRequestHandler clientRequestHandler;


    private void Start()
    {
        if (!IsOwner)
            Destroy(this);
        else
            GameManager.Instance.CameraManager.SetTarget(transform);

    }

    private void Awake()
    {
        inpt = new Inpt();
        mouseRightClick = inpt.d.mouseRightClick;
        mouseScroll = inpt.d.mouseScroll;
        cameraManager = GameManager.Instance.CameraManager;
        chatToggle = inpt.d.chatToggle;
    }

    private void OnNetworkStart()
    {
        clientRequestHandler = GameManager.Instance.Server.GetComponent<ClientRequestHandler>();
    }

    private void OnEnable()
    {
        mouseRightClick.Enable();
        mouseScroll.Enable();
        chatToggle.Enable();
        mouseRightClick.performed += RightClick;
        chatToggle.performed += ChatToggle;


    }

    private void OnDisable()
    {
        mouseRightClick.Disable();
        mouseScroll.Disable();
        chatToggle.Disable();
    }


    #region Input Actions

    private void RightClick(InputAction.CallbackContext context)
    {
        OnMouseClick();
    }

   


    public void OnMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (IsOwner)
            {
                Debug.Log("Sending position update");
             //   clientRequestHandler.PositionUpdate.SendToServer(new PositionUpdate { Position = hit.point });
               // GameManager.Instance.ParticleManager.PlayParticle("Move", hit.point);
            }
        }
    }


    private void ChatToggle(InputAction.CallbackContext context)
    {
        GameManager.Instance.ChatManager.ToggleChatPanel();
    }

    #endregion




    private void Update()
    {
        CameraScroll();
    }


    private void CameraScroll()
    {
        cameraManager.ScrollVector += new Vector3(0f, mouseScroll.ReadValue<float>() / 600, -mouseScroll.ReadValue<float>() / 1000);

        if (cameraManager.ScrollVector.y < 3)
            cameraManager.ScrollVector = new Vector3(cameraManager.ScrollVector.x, 3f, -1.8f);
        if (cameraManager.ScrollVector.y > 12)
            cameraManager.ScrollVector = new Vector3(cameraManager.ScrollVector.x, 12, -7.2f);

    }


}
