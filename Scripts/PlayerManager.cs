using System;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private XROrigin xRorigin;
    [SerializeField] private ARPlaneManager arPlaneManager;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private Camera cam;

    public Color playerColour;

    public static Action<Action<Ball>> OnSpawnBall;
    public static Action<Transform> OnPlayerSpawn;

    private void Awake()
    {
        xRorigin = GetComponent<XROrigin>();
        arPlaneManager = GetComponent<ARPlaneManager>();

        //if (!IsOwner) return;

        // TODO - fix 2 balls spawning on client side.

        UiManager.OnSpawnBallButtonPressed += SetupBallSpawn;
    }

    private void Start()
    {
        print("On player start");
        OnPlayerSpawn?.Invoke(gameObject.transform);
    }

    private void OnEnable()
    {
        //if (!IsOwner) return;
        SetRandomColour();

    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;

        if (arPlaneManager != null) Destroy(arPlaneManager);
        if (xRorigin != null) Destroy(xRorigin);
        cam.enabled = false;
    }

    private void SetRandomColour()
    {
        playerColour = playerObject.GetComponent<MeshRenderer>().material.color = UnityEngine.Random.ColorHSV();
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

        ball.GetComponent<NetworkObject>().Spawn();
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

        print("Spawn Ball from server");
        SpawnBall();
    }
}
