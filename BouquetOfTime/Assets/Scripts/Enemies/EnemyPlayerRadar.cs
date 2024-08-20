using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerRadar : MonoBehaviour {

    [SerializeField] Slime slime;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            slime.playerObj = other.gameObject;
            slime.OnPlayerEnterRange();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            slime.OnPlayerLeaveRange();
        }
    }
}
