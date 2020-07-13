using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour
{
    public GameObject dice1;
    public GameObject dice2;
    public GameObject numberDisplay;

    void Start(){
    }

    void Update(){
        if(dice1.GetComponent<DiceControl>().resultValue != 0 && dice2.GetComponent<DiceControl>().resultValue != 0){ //si ambos dados poseen valor, sumarlos
            numberDisplay.GetComponent<Text>().text = (dice1.GetComponent<DiceControl>().resultValue + dice2.GetComponent<DiceControl>().resultValue).ToString();
        }
    }

    public void RollDices(){ //lanzar dados
        dice1.GetComponent<DiceControl>().RollDice();
        dice2.GetComponent<DiceControl>().RollDice();
    }

    public void ResetScene(){ //reiniciar escena
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
