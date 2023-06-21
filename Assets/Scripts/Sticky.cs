using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Sticky : MonoBehaviour
{
    Collider mCollider;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        mCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        rb.isKinematic = true;
    }
}
