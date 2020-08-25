using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDropdown : MonoBehaviour
{

    public void HandleInputData(int val)
    {
        if(val == 1)
        {
            SceneManager.LoadScene("BoardScene");
            Time.timeScale = 1;
        }
        if (val == 2)
        {
            Application.Quit();
        }
    }
}
