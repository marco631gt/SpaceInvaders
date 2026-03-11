using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SCORE_MANAGERBALL : MonoBehaviour
{
    public static SCORE_MANAGERBALL Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] TextMeshProUGUI lastScore;
    [SerializeField] string prefix = "Puntos: ";

    int score = 0;
    int bestscore = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        UpdateUI();
    }

    public void AddPoints(int points)
    {
        score = score + points;
        UpdateUI();

        if (score >= 10000)
        {
            UpdateUI2();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    public void UpdateUI2()
    {
        if (lastScore != null)
            lastScore.text = "Última puntuación: " + score.ToString();

        if (score > bestscore)
        {
            bestscore = score;

            if (bestScore != null)
                bestScore.text = "Mejor puntuación: " + bestscore.ToString();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = prefix + score.ToString();
    }
}