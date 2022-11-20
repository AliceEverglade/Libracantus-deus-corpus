using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Homing : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private float speed = 5;
    [SerializeField] private float rotateSpeed = 200;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 dir = (Vector2)target.position - rb.position;
        dir.Normalize();
        float rotateAmount = Vector3.Cross(dir, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(deathEffect);
        Destroy(gameObject);
    }
}
