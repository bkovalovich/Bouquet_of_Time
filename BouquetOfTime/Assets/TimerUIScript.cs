using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TimerUIScript : MonoBehaviour
{
    [SerializeField] FloatVariableSO currentTime;
    private TMP_Text tmptext;

    private void Awake() {
        tmptext = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        currentTime.Subscribe(OnTimeUpdate);
    }
    private void OnDisable() {
        currentTime.Unsubscribe(OnTimeUpdate);
    }

    private void OnTimeUpdate() {
        string timeString = TimeSpan.FromSeconds(currentTime.Value).ToString();
        tmptext.text = timeString;
    }
}
