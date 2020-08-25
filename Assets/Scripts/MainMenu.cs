
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int selectedMaxPlayers = 1;
    public AudioClip music1;
    public AudioClip music2;

    void OnEnable()
    {
        var binary = new int[2];
        binary[0] = 0;
        binary[1] = 1;
        int chosenSong = Random.Range(0, binary.Length);
        if (chosenSong == 0) GetComponent<AudioSource>().clip = music1;
        else GetComponent<AudioSource>().clip = music2;
        GetComponent<AudioSource>().Play();
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("maxPlayers", selectedMaxPlayers);
    }

    public void SetMaxPlayers(int num)
    {
        selectedMaxPlayers = num;
    }

    public void LoadBoard()
    {
        SceneManager.LoadScene("BoardScene", LoadSceneMode.Single);
    }

    public void Escape()
    {
        Application.Quit();
    }
}
