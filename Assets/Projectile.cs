using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Velocity = DefaultValues.MediumProjectileVelocity;

    void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Velocity, 0);
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
        GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.rotation.y, transform.rotation.x) * Velocity;
    }
}
