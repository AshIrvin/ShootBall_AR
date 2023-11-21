using System;
using Unity.Netcode;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Action<Ball> OnGoalScored;

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
