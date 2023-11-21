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

    private void ChooseSpawnPosition(Transform player)
    {
        print("Setting player spawn point");
        var trans = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)];
        player.position = trans.position;
        player.rotation = trans.rotation;
    }
}
