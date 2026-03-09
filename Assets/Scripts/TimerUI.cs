using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    void Start()
    {
        GameTimer timer = Object.FindFirstObjectByType<GameTimer>();

        if (timer != null)
        {
            timer.timerText = GetComponent<Text>();
        }
    }
}