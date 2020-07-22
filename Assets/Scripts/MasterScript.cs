using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour
{
    public GameObject dice1;
    public GameObject dice2;
    public GameObject numberDisplay;
    public bool hasRolled;
    public int whosTurn;
    public int maxPlayers;
    public GameObject players;
    public GameObject rollButton;
    public GameObject numberPanel;
    public GameObject messagePanel;
    public float timer = 60f;
    public GameObject timerDisplay;
    public GameObject pointsDisplay;
    public GameObject coins;
    public List<GameObject> icoins = new List<GameObject>();

    private void Start(){
        hasRolled = false;
        maxPlayers = 2;
        whosTurn = 1;
        SpawnCoins();
    }

    void SpawnCoins()
    {
        GameObject[] alts = GameObject.FindGameObjectsWithTag("alt");
        foreach (GameObject alt in alts)
        {
            bool hasCoinNear = false;
            foreach(GameObject coin in icoins)
            {
                if (Vector3.Distance(coin.transform.position, alt.transform.position) <= 3)
                {
                    hasCoinNear = true;
                    break;
                }
            }
            if (icoins.Count < 4 && Random.value < .5f && !hasCoinNear) 
            {
                Quaternion rot = coins.transform.rotation;
                rot.y = Random.rotation.y;
                GameObject icoin = Instantiate(coins, alt.transform.position, rot);
                icoins.Add(icoin);
            }
            
        }
        
    }

    void Update(){
        if (players.transform.GetChild(whosTurn-1).GetComponent<Movement>().steps == 0) timer -= Time.deltaTime;
        timerDisplay.GetComponentInChildren<Text>().text = ((int) timer).ToString();
        if (timer <= 0f) EndTurn();

        if (dice1.GetComponent<DiceControl>().resultValue != 0 && 
            dice2.GetComponent<DiceControl>().resultValue != 0 && !hasRolled){ //si ambos dados poseen valor, sumarlos
            MovePlayerByDice();
        }
        pointsDisplay.GetComponentInChildren<Text>().text = (players.transform.GetChild(whosTurn - 1).GetComponent<Points>().points).ToString();
    }

    public void MovePlayerByDice(){
        numberDisplay.GetComponent<Text>().text = (dice1.GetComponent<DiceControl>().resultValue +
            dice2.GetComponent<DiceControl>().resultValue).ToString();

        dice1.GetComponent<DiceControl>().isRolling = false;
        dice2.GetComponent<DiceControl>().isRolling = false;
        hasRolled = true;
        numberPanel.GetComponent<Animator>().enabled = true;
        numberPanel.GetComponent<Animator>().Play("FadeInAndOutNumber");
        StartCoroutine(EndFadeAnim(numberPanel));
        players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().MovePlayer();
    }

    IEnumerator EndFadeAnim(GameObject panel){
        yield return new WaitForSeconds(3f);
        panel.GetComponent<Animator>().enabled = false;
    }

    public void RollDices(){ 
        dice1.GetComponent<DiceControl>().RollDice();
        dice2.GetComponent<DiceControl>().RollDice();
        rollButton.GetComponent<Button>().interactable = false;  
    }

    public void EndTurn(){
        SpawnCoins();
        if (GameObject.Find("altRouteArrow"))
        { //eliminar flechas y seleccionar path normal.
            players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().arrowResponse = 1;
            players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().savedSteps = 0;
        }
        timer = 60f;
        whosTurn++;
        if (whosTurn == maxPlayers+1) whosTurn = 1;
        hasRolled = false;
        dice1.GetComponent<DiceControl>().ResetDice();
        dice2.GetComponent<DiceControl>().ResetDice();
        numberDisplay.GetComponent<Text>().text = "";
        rollButton.GetComponent<Button>().interactable = true;
    }

    public void GameOver(){
        messagePanel.GetComponentInChildren<Text>().text = "¡El jugador " + whosTurn + " se ha hecho con la victoria!";
        messagePanel.GetComponent<Animator>().enabled = true;
        messagePanel.GetComponent<Animator>().Play("FadeInAndOutMessage");
    }

    public void ResetScene(){ //reiniciar escena
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
