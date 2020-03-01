using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Velocity = DefaultValues.MediumProjectileVelocity;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = new Vector2(Velocity, 0);
    }

    public void Update()
    {
        Vector2 v = _rigidbody.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;       
        // TODO: do velocity correctly
        GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.rotation.y * 2 + 1, transform.rotation.x ) * Velocity;
     }
}
