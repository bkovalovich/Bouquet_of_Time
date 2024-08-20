using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePlop : MonoBehaviour {
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();  
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("plop");
        audioSource.PlayOneShot(audioSource.clip);
    }
}
