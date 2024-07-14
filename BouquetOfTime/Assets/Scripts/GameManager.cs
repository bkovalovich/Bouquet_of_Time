using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] FloatVariableSO currentTime;

    private void Awake() {
        
    }
    private void Start() {
        
    }
    private void Update() {
        currentTime.Value -= Time.deltaTime;
    }
}
