using System;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GoalManager : NetworkBehaviour
{
    private int score = 0;

    public TextMeshPro scoreText;

    private void UpdateScore()
    {
        print("Scored Goal!");
        score++;
        scoreText.text = score.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            UpdateScore();
            //SendServerRpc();
        }
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

        print("Update score from server");
        UpdateScore();
    }
}
