using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> SpawnPoints;

    private void Awake()
    {
        PlayerManager.OnPlayerSpawn += ChooseSpawnPosition;
    }

    private void ChooseSpawnPosition(Transform playerTransform)
    {
        int id = (int)playerTransform.GetComponent<NetworkObject>().NetworkObjectId;
        int n = Mathf.Min(SpawnPoints.Count, id);

        var t = SpawnPoints[n-1];
        playerTransform.position = t.position;
        playerTransform.rotation = t.rotation;
    }
}
