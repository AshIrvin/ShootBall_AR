using System;
using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public static Action<Ball> OnGoalScored;
    public static Action<Ball> OnBallSpawned;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        OnBallSpawned?.Invoke(this);
    }

    public void Init(Action<Ball> action)
    {
        OnGoalScored = action;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            OnGoalScored(this);
        }
    }
}
