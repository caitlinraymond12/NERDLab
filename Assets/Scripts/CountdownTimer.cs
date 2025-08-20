using UnityEngine;
using TMPro;
using System;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float maxTime = 90f;
    public float remainingTime;
    private bool isRunning;

    public event Action OnTimerFinished;

    public void StartTimer(float time)
    {
        remainingTime = time;
        isRunning = true;
    }

    private void Update()
    {
        if (!isRunning) return;

        remainingTime -= Time.deltaTime;
        int mins = Mathf.FloorToInt(remainingTime / 60);
        int secs = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"{mins:00}:{secs:00}";

        if (remainingTime <= 0f)
        {
            isRunning = false;
            StopTimer();
            timerText.text = "00:00";
            OnTimerFinished?.Invoke();
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}