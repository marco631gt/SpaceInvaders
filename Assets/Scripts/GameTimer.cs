using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 180f;
    public TMP_Text timerText;

    private static GameTimer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;

            // 🔹 Desuscribirse antes de destruir
            SceneManager.sceneLoaded -= OnSceneLoaded;

            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
            return;
        }

        timeRemaining -= Time.deltaTime;
        UpdateTimer();

        if (timerText == null)
        {
            FindTimerText();
        }
    }

    void UpdateTimer()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 🔹 Si estamos en el menú destruir el timer
        if (scene.name == "MainMenu")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
            return;
        }

        timerText = null;
    }

    void FindTimerText()
    {
        TimerUI ui = FindFirstObjectByType<TimerUI>();

        if (ui != null)
        {
            timerText = ui.GetComponent<TMP_Text>();
        }
    }
}