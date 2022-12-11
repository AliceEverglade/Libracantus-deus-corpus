using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;
    [SerializeField] private float groundDamping;
    [SerializeField] private float fallDamping;
    [SerializeField] private float fallDampingThreshold;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movePos = target.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePos,ref velocity, damping);
        if(target.GetComponent<Rigidbody2D>().velocity.y < -fallDampingThreshold)
        {
            damping = fallDamping;
        }
        else
        {
            damping = groundDamping;
        }
    }
}
