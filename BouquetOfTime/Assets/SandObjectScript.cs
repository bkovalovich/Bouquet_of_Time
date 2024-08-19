using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandObjectScript : MonoBehaviour {
    [SerializeField] FloatVariableSO currentTime;
    private float timeToAdd = 5f;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            currentTime.Value += timeToAdd;
            OnCollected();
        }
    }

    private void OnCollected() {
        Destroy(gameObject);
    }
}
