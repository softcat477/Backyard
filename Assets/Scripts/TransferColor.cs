using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Renderer myRenderer = GetComponent<Renderer>(); // Renderer component of the current object
        Renderer otherRenderer = collision.gameObject.GetComponent<Renderer>(); // Renderer component of the collided object

        if (myRenderer != null && otherRenderer != null)
        {
            Material myMaterial = myRenderer.material; // Get the material of the current object
            Material otherMaterial = otherRenderer.material; // Get the material of the collided object

            // Swap materials between the two objects
            myRenderer.material = otherMaterial;
            otherRenderer.material = myMaterial;
        }
        Destroy(gameObject);
    }
}
