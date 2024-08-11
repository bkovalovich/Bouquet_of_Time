using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] FloatVariableSO currentTime;
    public TextMeshProUGUI timerUI;

    void OnEnable()
    {
        currentTime.ChangeEvent += UpdateTimer;
    }

    void OnDisable()
    {
        currentTime.ChangeEvent -= UpdateTimer;
    }

    private void UpdateTimer(float time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Round(time % 60);
        timerUI.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
