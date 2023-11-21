using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform DropLocation;
    [SerializeField] private Ball ball;
    
    private Ball ballObject;

    private void Awake()
    {
        PlayerManager.OnSpawnBall += SpawnBall;
    }

    private void OnDestroy()
    {
        PlayerManager.OnSpawnBall -= SpawnBall;
    }

    public void SpawnBall(Action<Ball> callback)
    {
        if (ballObject == null)
        {
            ballObject = Instantiate(ball);
            ballObject.Init(ResetBallToLocation);
            callback?.Invoke(ballObject);
        }

        ResetBall();
    }

    private void ResetBall()
    {
        ResetBallToLocation(ballObject);
    }

    private void ResetBallToLocation(Ball ballObject)
    {
        ballObject.transform.position = DropLocation.position;
        var rb = ballObject.gameObject.GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;      
    }
}
