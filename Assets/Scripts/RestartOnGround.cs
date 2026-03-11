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

        if (SCORE_MANAGERBALL.Instance != null)
        {
            SCORE_MANAGERBALL.Instance.UpdateUI2();
            SCORE_MANAGERBALL.Instance.ResetScore();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}