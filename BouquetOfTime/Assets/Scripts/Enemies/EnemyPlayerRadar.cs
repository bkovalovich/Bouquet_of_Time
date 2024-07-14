using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerRadar : MonoBehaviour {

    [SerializeField] Enemy enemy;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            enemy.playerObj = other.gameObject;
            enemy.OnPlayerEnterRange();
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            enemy.OnPlayerLeaveRange();
        }
    }
}
