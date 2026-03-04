using UnityEngine;
using TMPro;

public class SCORE_MANAGERBALL : MonoBehaviour
{
    public static SCORE_MANAGERBALL Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] TextMeshProUGUI lastScore;
    [SerializeField] string prefix = "Puntos: ";

    int score = 0;
    int bestscore = 0;

    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        UpdateUI();
        
    }

    public void AddPoints(int points){
        score = score + points;
        UpdateUI();
    }
    public void ResetScore(){
        score = 0;
        UpdateUI();
    }
    public void UpdateUI2() { 
        lastScore.text = "Última puntuación: " + score.ToString();
        if (score > bestscore) { 
            bestscore = score;
            bestScore.text = "Mejor puntuación: " + bestscore.ToString();
        }
    }
    void UpdateUI(){

        if (scoreText != null)
            scoreText.text = prefix + score.ToString();
            
    }

}