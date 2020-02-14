using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : MonoBehaviour
{
    [Tooltip("How much it bounces back on attack (multiplied by attack value.")]
    public float Bounciness = 1;
    public float lift = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 moveVector = gameObject.GetComponentInParent<Moveable>().moveVector;
        float damage = collision.gameObject.GetComponentInParent<DealsDamage>().damage;
        moveVector = (Vector2)((transform.position - collision.transform.position).normalized * Bounciness * damage) + moveVector;

        Vector2 diff = (transform.position - collision.transform.position);
        Vector2 bunco = new Vector2(lift, Mathf.Round(diff.y));
        Vector2 normalizedDiff = bunco.normalized;

        collision.GetComponentInParent<Moveable>().SendMessage("AddVector", - (Vector2)(diff * Bounciness * damage));
    }

    

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("OnCollisionStay2D");
        // disable attac
    }
}
