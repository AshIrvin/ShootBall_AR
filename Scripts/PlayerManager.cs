using System;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerObject;

    private XROrigin xrOrigin;
    private ARPlaneManager arPlaneManager;
    private Camera cam;

    public Color playerColour;
    public static Action<Action<Ball>> OnSpawnBall;
    public static Action<Transform> OnPlayerSpawn;

    private void Awake()
    {
        xrOrigin = GetComponent<XROrigin>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        cam = xrOrigin.Camera;

        UiManager.OnSpawnBallButtonPressed += SetupBallSpawn;
    }

    private void Start()
    {
        if (!IsOwner) return;

        OnPlayerSpawn?.Invoke(gameObject.transform);
    }

    private void OnEnable()
    {
        // TODO - Set correct colour for each player
        SetRandomColour();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;

        if (arPlaneManager != null) Destroy(arPlaneManager);
        if (xrOrigin != null) Destroy(xrOrigin);
        cam.enabled = false;
    }

    private void SetRandomColour()
    {
        playerColour = UnityEngine.Random.ColorHSV(0, 1, 1, 1);
        playerObject.GetComponent<MeshRenderer>().material.color = playerColour;
    }

    private void SetupBallSpawn()
    {
        if (!IsOwner) return;

        SendServerRpc();
        SpawnBall();
    }

    private void SpawnBall()
    {
        OnSpawnBall?.Invoke(SpawnBallOnNetwork);
    }

    private void SpawnBallOnNetwork(Ball ball)
    {
        if (!IsOwner) return;

        var obj = ball.GetComponent<NetworkObject>();
        obj.Spawn();
    }

    [ServerRpc]
    private void SendServerRpc()
    {
        SendClientRpc();
    }

    [ClientRpc]
    private void SendClientRpc()
    {
        if (IsOwner) return;

        SpawnBall();
    }
}
