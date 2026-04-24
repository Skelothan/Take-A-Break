using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Camera camera = Camera.main;
        KitchenCamera cameraScript = camera.GetComponent<KitchenCamera>();

        cameraScript.NextScene();
    }
}
