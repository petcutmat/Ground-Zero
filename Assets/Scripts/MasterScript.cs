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
    public int maxPlayers = 0;
    public GameObject players;
    public GameObject rollButton;
    public GameObject endTurnButton;
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
    public AudioClip drumSFX;

    private void Start(){
        endTurnButton.GetComponent<Button>().interactable = false;
        SpawnCoins();
        SpawnPwUps();
        hasRolled = false;
        for (int i = 0; i < players.transform.childCount; i++){
            if (players.transform.GetChild(i).gameObject.activeSelf) maxPlayers++; 
        }
        whosTurn = 1;
        Screen.SetResolution(1550, 700, true);
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

    void Update(){
        if (players.transform.GetChild(whosTurn-1).GetComponent<Movement>().steps == 0 && timer >= -10f) timer -= Time.deltaTime;
        timerDisplay.GetComponentInChildren<Text>().text = ((int) timer).ToString();
        if (timer <= 0f && timer >= -10f) EndTurn();

        if (dice1.GetComponent<DiceControl>().resultValue != 0 && 
            dice2.GetComponent<DiceControl>().resultValue != 0 && !hasRolled){ //si ambos dados poseen valor, sumarlos
            MovePlayerByDice();
        }
        pointsDisplay.GetComponentInChildren<Text>().text = 
            (players.transform.GetChild(whosTurn - 1).GetComponent<Points>().points).ToString() + "p";

        pointsDisplayMinigame.GetComponentInChildren<Text>().text =
            (players.transform.GetChild(whosTurn - 1).GetComponent<Points>().points).ToString() + "p";

        if(players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter > 0){
            players.transform.GetChild(whosTurn - 1).GetComponent<Points>().multiplier = 2;
        } else {
            players.transform.GetChild(whosTurn - 1).GetComponent<Points>().multiplier = 1;
        }
        if (ee > 1){
            if(gameObject.GetComponent<MiniGame>().blade.GetComponent<Blade>().cdrAlt != 500) gameObject.GetComponent<MiniGame>().blade.GetComponent<Blade>().cdrAlt = 500; //secreto
        }

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
                endTurnButton.GetComponent<Button>().interactable = false;
                rollButton.GetComponent<Button>().interactable = false;
                timer = -115f;
            }
        }
        
    }

    public void EndTurn()
    {
        SpawnCoins();
        if (GameObject.Find("altRouteArrow"))
        { //eliminar flechas y seleccionar path normal.
            players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().arrowResponse = 1;
            players.transform.GetChild(whosTurn - 1).GetComponent<Movement>().savedSteps = 0;
        }
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

        if (players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().IKPwUpCounter > 0) players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().IKPwUpCounter -= 1;
        if (players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter > 0) players.transform.GetChild(whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter -= 1;
        SpawnPwUps();
        DisplayPowerUp();
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


    public void StartMinigame()
    {
        //StartCoroutine(DrumSound());
        PwUpPanel.SetActive(false);
        diceCamera.SetActive(false);
        diceCameraSprite.SetActive(false);
        blackTrancision.SetActive(true);
        blackTrancision.GetComponent<Animator>().enabled = true;
        blackTrancision.GetComponent<Animator>().Play("BT1");
        endTurnButton.GetComponent<Button>().interactable = false;
        StartCoroutine(WaitFadeOutTrancision());
    }

    /*IEnumerator DrumSound()
    {
        GetComponent<AudioSource>().PlayOneShot(drumSFX);
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().PlayOneShot(drumSFX);
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().pitch = 1.2f;
        GetComponent<AudioSource>().PlayOneShot(drumSFX);
        GetComponent<AudioSource>().pitch = 1f;
    }*/

    IEnumerator WaitFadeOutTrancision()
    {
        yield return new WaitForSeconds(2f);
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
        endTurnButton.GetComponent<Button>().interactable = true;
        DisplayPowerUp();
    }


    public void GameOver(){
        ShowMessage("¡El jugador " + whosTurn + " se ha hecho con la victoria!", 500f);
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
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}
