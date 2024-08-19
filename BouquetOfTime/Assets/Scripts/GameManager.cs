using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] FloatVariableSO currentTime;
    public float startTime;

    private void Awake() 
    {
        currentTime.Value = startTime;
    }

    private void Update() 
    {
        currentTime.Value -= Time.deltaTime;
    }
}
