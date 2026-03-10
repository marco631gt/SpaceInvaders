using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RESTART_ON_GROUND : MonoBehaviour
{
    [SerializeField] string groundTag = "Suelo";
    [SerializeField] float restartDelay = 0.5f;

    bool _isRestarting = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isRestarting) return;

        if (collision.gameObject.CompareTag(groundTag))
        {
            StartCoroutine(RestartAfterDelay());
        }
    }

    IEnumerator RestartAfterDelay()
    {
        _isRestarting = true;

        yield return new WaitForSeconds(restartDelay);

        // Mostrar la puntuación final antes de reiniciar
        SCORE_MANAGERBALL.Instance.ShowFinalScore();

        // Reiniciar puntos
        SCORE_MANAGERBALL.Instance.ResetScore();

        // Reiniciar escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}