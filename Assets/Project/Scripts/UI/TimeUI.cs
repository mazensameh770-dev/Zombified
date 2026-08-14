using UnityEngine;
using TMPro;
using System;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    private float timer;
    public event Action<float> OnTimeChanged;

    private void Update()
    {
        timer += Time.deltaTime;
        UpdateTimeUI();

        OnTimeChanged?.Invoke(timer);
    }

    private void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ResetTimer()
    {
        timer = 0f;
        UpdateTimeUI();
    }

    public float GetTime()
    {
        return timer;
    }
}