using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public InputActionAsset inputActions; // Input Actions Asset 참조

    private InputAction sceneChangeAction;

    void OnEnable()
    {
        if (inputActions != null)
        {
            var actionMap = inputActions.FindActionMap("SceneChange");
            if (actionMap != null)
            {
                sceneChangeAction = actionMap.FindAction("New action");
                if (sceneChangeAction != null)
                {
                    sceneChangeAction.Enable();
                    sceneChangeAction.performed += OnSceneChange;
                }
            }
        }
    }

    void OnDisable()
    {
        if (sceneChangeAction != null)
        {
            sceneChangeAction.performed -= OnSceneChange;
            sceneChangeAction.Disable();
        }
    }

    private void OnSceneChange(InputAction.CallbackContext context)
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
