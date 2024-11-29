using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void ChangeScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "SJ_MainScene")
        {
            SceneManager.LoadScene("SJ_GameScene");
        }
        else if (currentScene == "SJ_GameScene")
        {
            SceneManager.LoadScene("SJ_MainScene");
        }
    }
}