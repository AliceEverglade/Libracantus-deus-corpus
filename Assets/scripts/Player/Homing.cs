using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Homing : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject[] targetList;
    [SerializeField] private TimeManagement timeManagement;
    //[SerializeField] private GameObject deathEffect;

    [SerializeField] private float speed = 7;
    [SerializeField] private float rotateSpeed = 200;
    [SerializeField] private float damage = 200;
    [SerializeField] private float lifeTime = 3;
    [SerializeField] private string targetTag;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        targetList = GameObject.FindGameObjectsWithTag(targetTag);
        Debug.Log(targetList);
        if (targetList.Length > 0)
        {
            int listIndex = Random.Range(0, targetList.Length);
            target = targetList[listIndex].transform;
        }
        else
        {
            target = null;
        }
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            Vector2 dir = (Vector2)target.position - rb.position;
            dir.Normalize();
            float rotateAmount = Vector3.Cross(dir, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
        else
        {
            rb.velocity = transform.up * speed;
        }
        if(lifeTime > 0)
        {
            lifeTime -= Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if(targetTag == "Enemy")
            {
                collision.gameObject.GetComponent<DummyHit>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                timeManagement.EndLevel();
            }
            
        }
        
    }
}
