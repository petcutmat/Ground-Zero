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
    public int maxPlayers = 1;
    public GameObject players;
    public GameObject rollButton;
    public GameObject endTurnButton;
    public GameObject shopButton;
    public GameObject numberPanel;
    public GameObject messagePanel;
    public float timer = 60f;
    public GameObject timerDisplay;
    public GameObject pointsDisplay;
    public GameObject pointsDisplayMinigame;
    public GameObject coins;
    public List<GameObject> icoins = new List<GameObject>();
    public GameObject SpawnVFX;
    public GameObject healthBar;

    public GameObject boardCamera;
    public GameObject diceCamera;
    public GameObject diceCameraSprite;
    public GameObject minigameCamera;

    public GameObject petalTrancision;
    public GameObject blackTrancision;
    public GameObject blackTrancision2;

    public GameObject PwUpPanel;
    public GameObject x2PwUp;
    public GameObject ikPwUp;

    public int ee = 0;
    public GameObject square_29;
    public GameObject tpWp5;
    public int ee2 = 0;
    public GameObject sun;
    public GameObject eeCollection;

    public GameObject coverCanvas;
    public bool isArrowPostMingame = false;

    void OnEnable()
    {
        maxPlayers = PlayerPrefs.GetInt("maxPlayers");
        if (maxPlayers == 0) maxPlayers = 1;
    }

    private void Start(){
        endTurnButton.GetComponent<Button>().interactable = false;
        SpawnCoins();
        SpawnPwUps();
        hasRolled = false;
        for (int i = 0; i < maxPlayers; i++){
            players.transform.GetChild(i).gameObject.SetActive(true); 
        }
        whosTurn = 1;
        Screen.SetResolution(1550, 700, true);
    }

    private void Update()
    {
        if (boardCamera.activeSelf && ((players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().steps == 0 && timer >= -10f) || GameObject.FindGameObjectWithTag("routeArrow"))) timer -= Time.deltaTime;
        timerDisplay.GetComponentInChildren<Text>().text = ((int)timer).ToString();
        if (timer <= 5 && GameObject.FindGameObjectWithTag("routeArrow")) { //eliminar flechas y seleccionar path normal.
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("altRouteArrow")){
                Destroy(go);
            }
            Destroy(GameObject.FindGameObjectWithTag("routeArrow"));
            players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().arrowResponse = 1;
            players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().savedSteps = 1;

        }

        if (timer <= 0f && timer >= -10f) EndTurn();

        if (dice1.GetComponent<DiceControl>().resultValue != 0 &&
            dice2.GetComponent<DiceControl>().resultValue != 0 && !hasRolled)
        { //si ambos dados poseen valor, sumarlos
            MovePlayerByDice();
        }
        pointsDisplay.GetComponentInChildren<Text>().text =
            (players.transform.GetChild(whosTurn - 1).GetComponent<Points>().points).ToString() + "p";

        pointsDisplayMinigame.GetComponentInChildren<Text>().text =
            (players.transform.GetChild(whosTurn - 1).GetComponent<Points>().points).ToString() + "p";

        if (players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter > 0)
        {
            players.transform.GetChild(whosTurn - 1).GetComponent<Points>().multiplier = 2;
        } else {
            players.transform.GetChild(whosTurn - 1).GetComponent<Points>().multiplier = 1;
        }
        if (ee > 3)
        {
            tpWp5.SetActive(true);
        }
            

        if (ee > 4)
        {
            ee = 0;
            GameObject[] coins = GameObject.FindGameObjectsWithTag("coins");
            foreach(GameObject coin in coins)
            {
                Destroy(coin);
            }
            Transform route = GameObject.Find("Route").transform;
            if (gameObject.GetComponent<MiniGame>().blade.GetComponent<Blade>().cdrAlt != 500) gameObject.GetComponent<MiniGame>().blade.GetComponent<Blade>().cdrAlt = 0.5f;
            for (int i = 0; i < players.transform.childCount; i++)
            {
                if(players.transform.GetChild(i).gameObject.activeSelf) StartCoroutine(players.transform.GetChild(i).gameObject.GetComponent<Movement>().TpBack(50,i+1));
            }
            foreach(Transform square in route)
            {
                if (square.GetComponent<SquareType>() != null && square.GetComponent<SquareType>().type == 1) Destroy(square.GetComponent<SquareType>());
            }
            square_29.GetComponent<SquareType>().type = 2;
            sun.GetComponent<Light>().colorTemperature = 2407;
            eeCollection.SetActive(true);
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("EERemove"))
            {
                Destroy(go);
            }
        }
        if (numberPanel.GetComponent<Animator>().enabled)
        {
            if (players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().steps > 0)
                numberDisplay.GetComponent<Text>().text = (players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().stepsLeft - 1).ToString();
            if (players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().steps == 1) StartCoroutine(EndFadeAnim(numberPanel));
        }

    }

    public void EndTurn()
    {
        SpawnCoins();
        players.transform.GetChild(whosTurn - 1).GetChild(1).gameObject.SetActive(false);
        timer = 60f;
        whosTurn++;
        if (whosTurn == maxPlayers + 1) whosTurn = 1;
        if (players.transform.GetChild(whosTurn - 1).GetComponent<Health>().healthPoints == 0) EndTurn();
        hasRolled = false;
        dice1.GetComponent<DiceControl>().ResetDice();
        dice2.GetComponent<DiceControl>().ResetDice();
        numberDisplay.GetComponent<Text>().text = "";
        rollButton.GetComponent<Button>().interactable = true;
        UpdateHealthBar();
        endTurnButton.GetComponent<Button>().interactable = false;
        players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().canEndTurn = true;
        if (players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().litcounter > 0) players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().litcounter -= 1;
        if (players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().litcounter == 0 && players.transform.GetChild(whosTurn - 1).childCount == 3) Destroy(players.transform.GetChild(whosTurn - 1).GetChild(2).gameObject);

        if (players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().IKPwUpCounter > 0) players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().IKPwUpCounter -= 1;
        if (players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter > 0) players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter -= 1;
        SpawnPwUps();
        DisplayPowerUp();
        players.transform.GetChild(whosTurn - 1).GetChild(1).gameObject.SetActive(true);
    }


    void SpawnCoins() {
        GameObject[] alts = GameObject.FindGameObjectsWithTag("alt");
        Vector3 prevAltpos = new Vector3(0,0,0);
        
        foreach (GameObject alt in alts){
            bool isNear = false;
            foreach (GameObject coin in icoins){
                if (Vector3.Distance(coin.transform.position, alt.transform.position) < 3) {
                    isNear = true;
                    break;
                }
            }

            if (!isNear && Vector3.Distance(prevAltpos, alt.transform.position) > 3 && icoins.Count < 4 && Random.value < .5f) { 
                Vector3 pos = alt.transform.position;
                pos.y += 0.2f;
                prevAltpos = pos;
                GameObject vfx = Instantiate(SpawnVFX, pos, Quaternion.identity);
                StartCoroutine(PerkSpawnAnimation(alt));
                StartCoroutine(CleanSpawnVFX(vfx, 1));
            }
        }
    }

    void SpawnPwUps() {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("pwUp"))
        {
           Destroy(go);
        }
        GameObject[] squares = GameObject.FindGameObjectsWithTag("square");
        foreach (GameObject sq in squares)
        {
            if (Random.value > .7f ){ //30%
                Vector3 pos = sq.transform.position;
                pos.y += 0.3f;

                if (Random.value > .5f){
                    GameObject vfx = Instantiate(SpawnVFX, pos, Quaternion.identity);
                    StartCoroutine(PwUpSpawnAnimation(sq, x2PwUp));
                } else {
                    GameObject vfx = Instantiate(SpawnVFX, pos, Quaternion.identity);
                    StartCoroutine(PwUpSpawnAnimation(sq, ikPwUp));
                }
            }
        }
    }

    IEnumerator PwUpSpawnAnimation(GameObject sq, GameObject pwUp)
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 pos = sq.transform.position;
        pos.y += 0.3f;
        Instantiate(pwUp, pos, Quaternion.identity);
    }

    IEnumerator PerkSpawnAnimation(GameObject alt)
    {
        yield return new WaitForSeconds(0.2f);
        Quaternion rot = coins.transform.rotation;
        rot.y = Random.rotation.y;
        GameObject icoin = Instantiate(coins, alt.transform.position, rot);
        icoins.Add(icoin);
    }

    public IEnumerator CleanSpawnVFX(GameObject vfx, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(vfx);
    }

   

    public void UpdateHealthBar() //llamado por EndTurn() y MinGame > End()
    {
        foreach(Transform hp in healthBar.transform){
            Color c = hp.GetComponent<SpriteRenderer>().color;
            c.a = 1f;
            hp.GetComponent<SpriteRenderer>().color = c;
        }
        for (int i = 0; i < 3 - players.transform.GetChild(whosTurn - 1).GetComponent<Health>().healthPoints; i++) {
            Color c = healthBar.transform.GetChild(i).GetComponent<SpriteRenderer>().color;
            c.a = 0.43f;
            healthBar.transform.GetChild(i).GetComponent<SpriteRenderer>().color = c;
        }
        if(players.transform.GetChild(whosTurn - 1).GetComponent<Health>().healthPoints == 0)
        {
            endTurnButton.GetComponent<Button>().interactable = true;
            int defeatedPlayers = 0;
            for (int i = 0; i < players.transform.childCount; i++)
            {
                if (players.transform.GetChild(i).GetComponent<Health>().healthPoints == 0){
                    defeatedPlayers += 1;
                    MeshRenderer rend = players.transform.GetChild(i).GetChild(0).GetComponent<MeshRenderer>();
                    rend.material = players.transform.GetChild(i).GetComponent<Health>().blackMat;
                } else {
                    MeshRenderer rend = players.transform.GetChild(i).GetChild(0).GetComponent<MeshRenderer>();
                    rend.material = players.transform.GetChild(i).GetComponent<Health>().defaultMat;
                }
            }
            if(defeatedPlayers == maxPlayers)
            {
                ShowMessage("Todos los jugadores han caido", 500f);
                endTurnButton.SetActive(false);
                rollButton.SetActive(false);
                shopButton.SetActive(false);
                timer = -115f;
            }
        }
        
    }

    

    public void DisplayPowerUp()
    {
        PwUpPanel.SetActive(true);
        if (players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().IKPwUpCounter > 0){
            PwUpPanel.transform.GetChild(0).GetComponent<Text>().text = 
                players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().IKPwUpCounter.ToString();
            PwUpPanel.transform.GetChild(1).gameObject.SetActive(true);
            PwUpPanel.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter > 0){
            PwUpPanel.transform.GetChild(0).GetComponent<Text>().text = 
                players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter.ToString();
            PwUpPanel.transform.GetChild(2).gameObject.SetActive(true);
            PwUpPanel.transform.GetChild(1).gameObject.SetActive(false);
        }else{
            PwUpPanel.SetActive(false);
            PwUpPanel.transform.GetChild(1).gameObject.SetActive(false);
            PwUpPanel.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void Escape()
    {
        Application.Quit();
    }
    

    public void MovePlayerByDice(){
        numberDisplay.GetComponent<Text>().text = (dice1.GetComponent<DiceControl>().resultValue +
            dice2.GetComponent<DiceControl>().resultValue).ToString();

        dice1.GetComponent<DiceControl>().isRolling = false;
        dice2.GetComponent<DiceControl>().isRolling = false;
        hasRolled = true;
        numberPanel.GetComponent<Animator>().enabled = true;
        numberPanel.GetComponent<Animator>().Play("FadeInNumber");
        players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().MovePlayer();
    }

    public IEnumerator EndFadeAnim(GameObject panel){
        numberPanel.GetComponent<Animator>().Play("FadeOutNumber");
        yield return new WaitForSeconds(1f);
        panel.GetComponent<Animator>().enabled = false;
    }

    public void RollDices(){ 
        dice1.GetComponent<DiceControl>().RollDice();
        dice2.GetComponent<DiceControl>().RollDice();
        rollButton.GetComponent<Button>().interactable = false;  
    }


    public void StartMinigame()
    {
        if (players.transform.GetChild(whosTurn - 1).GetComponent<Health>().healthPoints > 0)
        {
            PwUpPanel.SetActive(false);
            diceCamera.SetActive(false);
            diceCameraSprite.SetActive(false);
            blackTrancision.SetActive(true);
            blackTrancision.GetComponent<Animator>().enabled = true;
            blackTrancision.GetComponent<Animator>().Play("BT1");
            endTurnButton.GetComponent<Button>().interactable = false;
            coverCanvas.SetActive(true);
            StartCoroutine(WaitFadeOutTrancision());
        }
        
    }

    IEnumerator WaitFadeOutTrancision()
    {
        yield return new WaitForSeconds(1.7f);
        blackTrancision.SetActive(false);
        blackTrancision.GetComponent<Animator>().enabled = false;
        boardCamera.SetActive(false);
        minigameCamera.SetActive(true);
        GetComponent<MiniGame>().StartMinigame();
        blackTrancision.SetActive(false); 
        blackTrancision2.GetComponent<Animator>().enabled = true;
        blackTrancision2.GetComponent<Animator>().Play("BT2");
        petalTrancision.GetComponent<Animator>().enabled = true;
        petalTrancision.GetComponent<Animator>().Play("PT");
        yield return new WaitForSeconds(0.3f);
        coverCanvas.SetActive(false);
        StartCoroutine(WaitFadeInTrancision());
    }

    IEnumerator WaitFadeInTrancision()
    {
        yield return new WaitForSeconds(4f);
        blackTrancision2.GetComponent<Animator>().enabled = false;
        petalTrancision.GetComponent<Animator>().enabled = false;
    }

    public void StopMinigame()
    {
        boardCamera.SetActive(true);
        diceCamera.SetActive(true);
        diceCameraSprite.SetActive(true);
        minigameCamera.SetActive(false);
        diceCamera.GetComponent<Camera>().depth = 1;
        if (!GameObject.FindGameObjectWithTag("routeArrow")) endTurnButton.GetComponent<Button>().interactable = true;
        else isArrowPostMingame = true;
        DisplayPowerUp();
    }


    public void GameOver(){
        endTurnButton.SetActive(false);
        rollButton.SetActive(false);
        shopButton.SetActive(false);
        if (eeCollection.activeSelf)
        {
          ShowMessage("¡Easter Egg completado! ¡Espada de oro desbloqueada!", 500f);
            PlayerPrefs.SetInt("goldenSword", 1);
        } else ShowMessage("¡El jugador " + whosTurn + " se ha hecho con la victoria!", 500f);
    }
    public void ShowMessage(string message, float seconds)
    {
        messagePanel.GetComponentInChildren<Text>().text = message;
        messagePanel.GetComponent<Animator>().enabled = true;
        messagePanel.GetComponent<Animator>().Play("FadeInMessage");
        StartCoroutine(FadeOutMessage(seconds));
    }
    IEnumerator FadeOutMessage(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        messagePanel.GetComponent<Animator>().Play("FadeOutMessage");
        StartCoroutine(EndMessageAnim());
    }
    IEnumerator EndMessageAnim()
    {
        yield return new WaitForSeconds(1.5f);
        messagePanel.GetComponent<Animator>().enabled = false;
    }

    public void ResetScene(){ //reiniciar escena
        Time.timeScale = 1;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}
