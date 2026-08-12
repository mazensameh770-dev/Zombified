using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        UpdateTimeUI();
    }

    private void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}