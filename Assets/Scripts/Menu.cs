using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;

    public void Pause(){
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Resume(){
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void LoadMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
