using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    void Start()
    {
        GameTimer timer = FindFirstObjectByType<GameTimer>();

        if (timer != null)
        {
            timer.timerText = GetComponent<TMP_Text>();
        }
    }
}