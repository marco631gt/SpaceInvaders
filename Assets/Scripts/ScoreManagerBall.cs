using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SCORE_MANAGERBALL : MonoBehaviour
{
    public static SCORE_MANAGERBALL Instance { get; private set; }

    TextMeshProUGUI scoreText;
    TextMeshProUGUI bestScore;
    TextMeshProUGUI lastScore;

    [SerializeField] string prefix = "Score: ";

    int score = 0;
    int bestscore = 0;
    int savedLastScore = 0; // Nueva variable para retener el último puntaje

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // --- CARGAR DATOS AL INICIAR ---
        bestscore = PlayerPrefs.GetInt("BestScore", 0);
        savedLastScore = PlayerPrefs.GetInt("LastScore", 0);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "BallPlay")
        {
            Destroy(gameObject);
            return;
        }

        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        bestScore = GameObject.Find("BestScore")?.GetComponent<TextMeshProUGUI>();
        lastScore = GameObject.Find("LastScore")?.GetComponent<TextMeshProUGUI>();

        UpdateUI();
        UpdateUI2(); // Esto asegura que al cargar la escena, se muestren los datos guardados
    }

    public void AddPoints(int points)
    {
        score += points;
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
        // --- GUARDAR Y MOSTRAR LAST SCORE ---
        // Solo actualizamos el LastScore si el score actual es mayor a 0 (para no sobreescribir con 0 al reiniciar)
        if (score > 0) 
        {
            savedLastScore = score;
            PlayerPrefs.SetInt("LastScore", savedLastScore);
        }

        if (lastScore != null)
            lastScore.text = "LastScore: " + savedLastScore;

        // --- LÓGICA DE BEST SCORE (IGUAL QUE ANTES) ---
        if (score > bestscore)
        {
            bestscore = score;
            PlayerPrefs.SetInt("BestScore", bestscore);

            if (bestScore != null)
                bestScore.text = "BestScore: " + bestscore;
        }
        
        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = prefix + score;
            
        // Refrescamos los textos persistentes para que no aparezcan vacíos al iniciar
        if (lastScore != null) lastScore.text = "LastScore: " + savedLastScore;
        if (bestScore != null) bestScore.text = "BestScore: " + bestscore;
    }
}