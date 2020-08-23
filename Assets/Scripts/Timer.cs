using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool m_timerState = true;

    [SerializeField] private TextMeshProUGUI m_time;
    [SerializeField] private float m_timeCounter = 0;

  

    void Update() {
        if (m_timerState)
            runTimer();

    }

    void runTimer() {
        m_timeCounter += Time.deltaTime;

        float minutes = Mathf.FloorToInt(m_timeCounter / 60);
        float seconds = Mathf.FloorToInt(m_timeCounter % 60);

        m_time.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    public void setTimer(bool timerState) {
        if (timerState)
            m_timerState = true;
        else {
            m_timerState = false;
            m_timeCounter = 0;
        }
    }

    public void resetTimer() {
        m_timeCounter = 0;
    }
}
