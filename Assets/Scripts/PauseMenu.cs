using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    KitchenCamera cameraScript;

    public GameObject pauseMenuButtons;

    public GameObject nextButton;

    public GameObject scene1Breakables;
    public GameObject scene2Breakables;
    public GameObject scene3Breakables;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera camera = Camera.main;
        cameraScript = camera.GetComponent<KitchenCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        bool canMoveOn = true;
        // Break[] breakScripts;

        // switch (cameraScript.currentScene) {
        //     case 0:
        //         breakScripts = scene1Breakables.GetComponentsInChildren<Break>(true);
        //         break;
        //     case 1:
        //         breakScripts = scene2Breakables.GetComponentsInChildren<Break>(true);
        //         break;
        //     case 2:
        //         breakScripts = scene3Breakables.GetComponentsInChildren<Break>(true);
        //         break;
        //     default:
        //         return;
        // }

        // for (int i = 0; i < breakScripts.Length; i++)
        // {
        //     canMoveOn = canMoveOn && breakScripts[i].isBroken;
        // }

        nextButton.SetActive(canMoveOn);

    }

    public void ShowPauseMenu()
    {
        pauseMenuButtons.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenuButtons.SetActive(false);
    }

    public void Reset()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void GoToMainMenu()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToNextScene()
    {
        cameraScript.NextScene();
        nextButton.SetActive(false);
    }
}
