using Bouquet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<Rigidbody>().transform.position = new Vector3(0, 0, 0);
            Debug.Log("Kill Plane");
            other.attachedRigidbody.GetComponent<HealthComponent>().AddHealth(-3);
        }
    }
}
