using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    public float Speed;
    public Action<Projectile> returnToPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Action<Projectile> action)
    {
        returnToPool = action;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "Ball":
                break;
            case "Goal":
                returnToPool(this);
                break;
            case "Floor":
                returnToPool(this);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Ball":
                returnToPool(this);
                break;
            case "Goal":
                returnToPool(this);
                break;
            case "Floor":
                returnToPool(this);
                break;
        }
    }

    public void SetForce(Transform trans)
    {
        rb.isKinematic = false;
        rb.AddRelativeForce(trans.forward * Speed, ForceMode.Force);
    }

    private void OnDisable()
    {
        ResetProjectile();
    }

    private void ResetProjectile()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
