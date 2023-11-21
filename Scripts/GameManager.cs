using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform DropLocation;
    [SerializeField] private Ball ball;
    
    private Ball ballObject;

    public static Action OnBallAssigned;

    private void Awake()
    {
        PlayerManager.OnSpawnBall += InstantiateBall;
        Ball.OnBallSpawned += AssignBall;
        UiManager.OnResetBallButtonPressed += ResetBall;
    }

    private void OnDestroy()
    {
        PlayerManager.OnSpawnBall -= InstantiateBall;
    }

    public void InstantiateBall(Action<Ball> callback)
    {
        // This null and reset probably isn't needed
        if (ballObject == null)
        {
            ballObject = Instantiate(ball);
            ballObject.Init(ResetBallToLocation);
            callback?.Invoke(ballObject);
        }

        ResetBall();
    }

    private void AssignBall(Ball ball)
    {
        ballObject = ball;
        OnBallAssigned?.Invoke();
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
