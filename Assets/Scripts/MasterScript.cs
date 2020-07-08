using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour
{
    public GameObject dice1;
    public GameObject dice2;
    public GameObject numberDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dice1.GetComponent<DiceControl>().resultValue != 0 && dice2.GetComponent<DiceControl>().resultValue != 0)
        {
            numberDisplay.GetComponent<Text>().text = (dice1.GetComponent<DiceControl>().resultValue + dice2.GetComponent<DiceControl>().resultValue).ToString();
        }
    }

    public void RollDices()
    {
        dice1.GetComponent<DiceControl>().buttonPressed = true;
        dice2.GetComponent<DiceControl>().buttonPressed = true;
    }

    public void ResetScene() //Reiniciar escena
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
