using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static Action OnFireButtonPressed;
    public static Action OnSpawnBallButtonPressed;
    public static Action OnResetBallButtonPressed;

    public static Action OnHostButtonPressed;
    public static Action OnServerButtonPressed;
    public static Action OnClientButtonPressed;

    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private Camera menuCamera;
    [SerializeField] private Button spawnButton;

    private void Start()
    {
        EnableGameCanvas(false);

        GameManager.OnBallAssigned += ChangeSpawnButton;
    }

    public void FireProjectileButton()
    {
        OnFireButtonPressed?.Invoke();
    }

    public void SpawnBallButton()
    {
        OnSpawnBallButtonPressed?.Invoke();
    }

    public void HostButton()
    {
        OnHostButtonPressed?.Invoke();

        NetworkManager.Singleton.StartHost();
        DisableNetworkButtons();
        EnableGameCanvas(true);
    }

    public void ServerButton()
    {
        OnServerButtonPressed?.Invoke();

        NetworkManager.Singleton.StartServer();
        DisableNetworkButtons();
        EnableGameCanvas(true);
    }

    public void ClientButton()
    {
        OnClientButtonPressed?.Invoke();

        NetworkManager.Singleton.StartClient();
        DisableNetworkButtons();
        EnableGameCanvas(true);
    }

    private void DisableNetworkButtons()
    {
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        button.transform.parent.gameObject.SetActive(false);
        DisableMenuCamera();
    }

    private void EnableGameCanvas(bool state)
    {
        if (gameCanvas == null) return;

        gameCanvas.SetActive(state);
    }

    private void DisableMenuCamera()
    {
        menuCamera.enabled = false;
    }

    private void ChangeSpawnButton()
    {
        spawnButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reset";
        spawnButton.onClick.AddListener(ResetBallButton);
    }

    private void ResetBallButton()
    {
        OnResetBallButtonPressed?.Invoke();
    }
}
