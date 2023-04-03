using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float speed = 10;
    public float maxDistance = 10;

    private Vector2 startPos;
    private float travelled = 0;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Initalize()
    {
        startPos = transform.position;
        rb2d.velocity = transform.up * speed * Time.fixedDeltaTime;
    }

    private void Update()
    {
        travelled = Vector2.Distance(transform.position, startPos);
        if (travelled >= maxDistance )
        {
            DisableObject();
        }
    }

    private void DisableObject()
    {
        rb2d.velocity = Vector2.zero;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collieded" + collision.name);
        DisableObject();
    }
}
