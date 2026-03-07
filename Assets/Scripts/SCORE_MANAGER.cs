using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SCORE_MANAGER : MonoBehaviour
{
    public static SCORE_MANAGER Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] string prefix = "Points: ";

    int score = 0;
    int deathsCounter = 0;
    int level = 1;

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

    public void UpdateHealth(int health)
    {
        if (healthText != null)
            healthText.text = "Health: " + health.ToString();
    }

    public void AddPoints(int points)
    {
        score = score + points;
        deathsCounter = deathsCounter + 1;
        UpdateUI();

        if (deathsCounter == 3)
        {
            nextLevel();
        }
    }

    public void ResetScore()
    {
        score = 0;
        deathsCounter = 0;
        level = 1;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = prefix + score.ToString();
    }

    void nextLevel()
    {
        level = level + 1;
        deathsCounter = 0;

        if (level <= 4)
        {
            SceneManager.LoadScene("SpaceInvaders" + level);
        }
        else
        {
            Debug.Log("¡Felicidades, terminaste todos los niveles!");
        }
    }
}