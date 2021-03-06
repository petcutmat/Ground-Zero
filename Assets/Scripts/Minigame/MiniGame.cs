﻿using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    public GameObject zombie;
    private GameObject zombiei;
    public GameObject obs;
    private GameObject obsi;

    public GameObject sword;
    public GameObject blade;
    public GameObject character;

    public GameObject camera;
    public GameObject panel;
    public GameObject counterDisplay;
    public GameObject bounds;

    public int counter;
    public bool allEnemiesSpawned;
    public bool endMinigame;

    public int enemyNumber = 5;
    public float enemySpawnDelay = 1;
    public float obstaclSpawnDelay = 5;
    public float swordSpawnDelay = 5;

    bool diffUp = false;
    bool diffUpEE = false;

    public int minigameType;
    public GameObject bombCounter;


    public void StartMinigame() {
        if (gameObject.GetComponent<MasterScript>().players.transform.GetChild(
            gameObject.GetComponent<MasterScript>().whosTurn - 1).GetComponent<PwUpEffect>().IKPwUpCounter > 0) {
            blade.GetComponent<Blade>().cdrAlt = 1f;
        } else {
            blade.GetComponent<Blade>().cdrAlt = 0.15f;
        }

        if (gameObject.GetComponent<MasterScript>().players.transform.GetChild(gameObject.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Movement>().routePosition > 20) {
            swordSpawnDelay /= 1.5f;
            diffUp = true;
        }
        if (gameObject.GetComponent<MasterScript>().eeCollection.activeSelf){
            diffUpEE = true;
            swordSpawnDelay /= 2f;
        }

        enemyNumber = (int) gameObject.GetComponent<MasterScript>().players.transform.GetChild(gameObject.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Movement>().routePosition/3 +5;
        blade.SetActive(false);
        blade.GetComponent<CircleCollider2D>().radius = 0.15f;
        character.GetComponent<CharacterController>().speed = 1.6f;
        endMinigame = false;
        character.GetComponent<CharacterController>().SpawnCharacter();

        var binary = new int[2];
        binary[0] = 0;
        binary[1] = 1;
        minigameType = Random.Range(0, binary.Length);

        if (minigameType == 1){
            StartCoroutine(SpawnEnemy(enemyNumber, enemySpawnDelay));
            StartCoroutine(SpawnObstacle(obstaclSpawnDelay));
            StartCoroutine(SpawnSword(swordSpawnDelay));
        }else {
            bounds.SetActive(true);
            StartCoroutine(SpawnBurst());
        }
        
        allEnemiesSpawned = false;
    }

    public IEnumerator SpawnBurst()
    {
        blade.SetActive(true);
        blade.GetComponent<CircleCollider2D>().radius = 0.3f;
        character.GetComponent<CharacterController>().speed = 0;
        int counter = 0;
       
        int enemies = 20;
        if (diffUp) enemies = 30;
        for (int i = 0; i < enemies; i++)
        {
            yield return new WaitForSeconds(0.3f);

            zombiei = Instantiate(zombie, panel.transform);

            Vector3[] possiblePossitions = new Vector3[4];
            possiblePossitions[0] = new Vector3(0, 400, -1);
            possiblePossitions[1] = new Vector3(600, 0, -1);
            possiblePossitions[2] = new Vector3(0, -400, -1);
            possiblePossitions[3] = new Vector3(-600, 0, -1);

            zombiei.GetComponent<RectTransform>().localPosition = possiblePossitions[Random.Range(0, possiblePossitions.Length)];

            zombiei.GetComponent<RectTransform>().localScale = new Vector3(0.55f, 0.55f, 1f);
            zombiei.GetComponent<EnemyController>().speed = 0.6f;
            if (diffUp)
            {
                zombiei.GetComponentInChildren<Image>().color = new Color(0.7f, 0.5f, 0.5f);
                if (counter > 5) zombiei.GetComponent<EnemyController>().speed = 0.9f;
                if (counter > 10 && (zombiei.GetComponent<RectTransform>().localPosition == new Vector3(600, 0, -1) || zombiei.GetComponent<RectTransform>().localPosition == new Vector3(-600, 0, -1))) zombiei.GetComponent<EnemyController>().speed = 1.5f;
                if (counter > 20 && (zombiei.GetComponent<RectTransform>().localPosition == new Vector3(600, 0, -1) || zombiei.GetComponent<RectTransform>().localPosition == new Vector3(-600, 0, -1))) zombiei.GetComponent<EnemyController>().speed = 1.9f;
            }
            if (diffUpEE)
            {
                zombiei.GetComponentInChildren<Image>().color = new Color(1f, 0.3f, 0.3f);
                if (counter > 5) zombiei.GetComponent<EnemyController>().speed = 1f;
                if (counter > 10 && (zombiei.GetComponent<RectTransform>().localPosition == new Vector3(600, 0, -1) || zombiei.GetComponent<RectTransform>().localPosition == new Vector3(-600, 0, -1))) zombiei.GetComponent<EnemyController>().speed = 1.9f;
                if (counter > 20 && (zombiei.GetComponent<RectTransform>().localPosition == new Vector3(600, 0, -1) || zombiei.GetComponent<RectTransform>().localPosition == new Vector3(-600, 0, -1))) zombiei.GetComponent<EnemyController>().speed = 2.2f;
            }

            zombiei.tag = "Zombie";
            zombiei.name = "zombie" + i;
            zombiei.transform.SetSiblingIndex(0); //insertarlo encima para capas
            counter++;
        }
        allEnemiesSpawned = true;
        bounds.SetActive(false);
        character.GetComponent<CharacterController>().speed = 1.6f;
    }

    private void Update(){
        if(character.GetComponent<CharacterController>().health == 0 || 
            (allEnemiesSpawned && minigameType == 1 && GameObject.FindGameObjectWithTag("Boss") == null || allEnemiesSpawned && minigameType == 0 && GameObject.FindGameObjectWithTag("Zombie") == null)){
            if (!endMinigame){
                endMinigame = true;
                StartCoroutine(End());
            }
        }
        foreach (GameObject z in GameObject.FindGameObjectsWithTag("Zombie"))
        {
            z.GetComponent<EnemyController>().speed += 0.05f *Time.deltaTime;
        }

        bombCounter.GetComponentInChildren<Text>().text = character.GetComponent<CharacterController>().charges.ToString();

    }
    IEnumerator End(){
        bounds.SetActive(false);
        if (character.GetComponent<CharacterController>().health > 0 && minigameType == 1) {
            Time.timeScale = 0.3f;
            camera.GetComponent<AudioSource>().pitch = 0.3f;
            counterDisplay.SetActive(true);
            counterDisplay.GetComponentInChildren<Text>().text = counter + "p";
            yield return new WaitForSeconds(0.7f);
            GameObject[] blood = GameObject.FindGameObjectsWithTag("Blood");
            for (int i = 0; i < blood.Length; i++)
            {
                Destroy(blood[i]);
            }
            yield return new WaitForSeconds(0.1f);
            Time.timeScale = 1;
            camera.GetComponent<AudioSource>().pitch = 1f;
            yield return new WaitForSeconds(2f);
            counterDisplay.SetActive(false);
            StopAllCoroutines();
        }
        else if (character.GetComponent<CharacterController>().health > 0 && minigameType == 0)
        {
            yield return new WaitForSeconds(0.3f);
            GameObject[] blood = GameObject.FindGameObjectsWithTag("Blood");
            for (int i = 0; i < blood.Length; i++)
            {
                Destroy(blood[i]);
            }
            yield return new WaitForSeconds(0.1f);
            StopAllCoroutines();
        }else{
            GameObject[] blood = GameObject.FindGameObjectsWithTag("Blood");
            for (int i = 0; i < blood.Length; i++)
            {
                Destroy(blood[i]);
            }
            yield return new WaitForSeconds(0.1f);
            StopAllCoroutines();
            gameObject.GetComponent<MasterScript>().players.transform.GetChild(gameObject.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Health>().healthPoints -= 1;
            gameObject.GetComponent<MasterScript>().UpdateHealthBar();
        }
        counter = 0;
        gameObject.GetComponent<MasterScript>().StopMinigame();
    }


    IEnumerator SpawnEnemy(int enemyNumber, float timeBetweenSpawns) {

        for (int i = 0; i < enemyNumber; i++){
            yield return new WaitForSeconds(timeBetweenSpawns);
            
            zombiei = Instantiate(zombie, panel.transform);

            if(Random.Range(0, 2) == 0){
                zombiei.GetComponent<RectTransform>().localPosition = new Vector3(-556, 213, -1);
            } else {
                zombiei.GetComponent<RectTransform>().localPosition = new Vector3(556, -213, -1);
            }

            if (i == enemyNumber - 1){
                zombiei.GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1.3f, 1f);
                zombiei.GetComponent<EnemyController>().speed = 1f;
                zombiei.tag = "Boss";
                zombiei.name = "zombieBoss";
            }else{
                zombiei.GetComponent<RectTransform>().localScale = new Vector3(0.55f, 0.55f, 1f);
                zombiei.GetComponent<EnemyController>().speed = Random.Range(0.4f, 1);
                zombiei.tag = "Zombie";
                zombiei.name = "zombie" + i;
            }

            if (diffUp)
            {
                zombiei.GetComponentInChildren<Image>().color = new Color(0.7f, 0.5f, 0.5f);
                zombiei.GetComponent<EnemyController>().speed = Random.Range(1.1f, 1.2f);
            }
            if (diffUpEE)
            {
                zombiei.GetComponentInChildren<Image>().color = new Color(1f, 0.3f, 0.3f);
                zombiei.GetComponent<EnemyController>().speed = Random.Range(1.3f, 1.6f);
            }

            zombiei.transform.SetSiblingIndex(0); //insertarlo encima para capas
        }
        allEnemiesSpawned = true;
    }

    IEnumerator SpawnObstacle(float timeBetweenSpawns) {
        for (int i = 0; i < 100; i++){
            obsi = Instantiate(obs, panel.transform);
            obsi.transform.localScale = new Vector3(1, 1, 1);

            if (Random.Range(0, 2) == 0){
                obsi.GetComponent<RectTransform>().localPosition = new Vector3(-556, 213, -1);
            }else{
                obsi.GetComponent<RectTransform>().localPosition = new Vector3(556, -213, -1);
            }

            Color clight = obsi.GetComponent<Image>().color;
            Color cdark = new Color(0.85f, 0.724f, 0.724f);
            Color cred = new Color(0.905f, 0.5758f, 0.5758f);
            Color[] colors = new Color[3];
            colors[0] = clight;
            colors[1] = cdark;
            colors[2] = cred;

            obsi.GetComponent<Image>().color = colors[Random.Range(0,colors.Length)];
            obsi.name = "obs" + i;
            obsi.transform.SetSiblingIndex(0); //insertarlo encima para capas
            obsi.GetComponent<ObstacleController>().speed = Random.Range(30f, 40f);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }


    IEnumerator SpawnSword(float sec) {
        yield return new WaitForSeconds(sec);
        GameObject swordi = Instantiate(sword,panel.transform);
        Vector3[] possiblePossitions = new Vector3[3];
        possiblePossitions[0] = new Vector3(400, 144, -1);
        possiblePossitions[1] = new Vector3(0, 0, -1);
        possiblePossitions[2] = new Vector3(-400, -144, -1);

        swordi.GetComponent<RectTransform>().localPosition = possiblePossitions[Random.Range(0,possiblePossitions.Length)];
        
    }


}