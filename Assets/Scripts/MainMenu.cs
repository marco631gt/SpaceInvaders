using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlaySpaceInvaders()
    {
        SceneManager.LoadScene("SpaceInvaders");
    }

    public void PlayBallGame()
    {
        SceneManager.LoadScene("BallPlay");
    }
}