using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public GameObject route;
    public MasterScript master;
    public int routePosition;
    public int steps;
    public float velocity = 3f;
    public GameObject arrow;
    public int arrowResponse = 0;
    public GameObject altWp1;
    public GameObject altWp2;
    public int savedSteps;
    public List<Transform> currentRoute = new List<Transform>();
    private bool disolveObstacleOver = false;
    private bool disolvePlayerOver = false;
    private bool appearPlayerOver = false;
    public Material defaultMat, disolveMat;
    public GameObject forceField;
    public bool canEndTurn = true;
    public bool canStartMinigame = true;
    public int litcounter = 0;

    public AudioClip openDoorSFX;

    private void Start(){ //Creación de ruta por jugador
        ResetRoute();
    }

    public void ResetRoute()
    {
        currentRoute.Clear();
        for (int i = 0; i < route.GetComponent<Route>().childList.Count; i++)
            currentRoute.Add(route.GetComponent<Route>().childList[i]);
    }

    public void Update(){
        if (steps >= 0 && currentRoute.Count == routePosition + 1){
            steps = -1;
            master.GameOver();
        }
        if (arrowResponse == 1) {
            arrowResponse = -1;
            steps = savedSteps;

            StartCoroutine(Move());
            StartCoroutine(RevertArrowResponse());
            foreach (GameObject altarrow in GameObject.FindGameObjectsWithTag("altRouteArrow"))
            {
                Destroy(altarrow);
            }
            Destroy(GameObject.Find("routeArrow"));
           
            canEndTurn = true;
        }
        if(arrowResponse == 2){
            arrowResponse = -1;

            int skipSquares = currentRoute[routePosition].GetComponent<SquareType>().skipSquares;

            int altSquares = altWp1.transform.parent.transform.childCount;

            for (int i = 1; i <= skipSquares; i++) {
                currentRoute.Remove(currentRoute[routePosition + 1]);
            }
            for (int i = 1; i <= altSquares; i++)
            {
                currentRoute.Insert(routePosition + i, altWp1.transform.parent.transform.GetChild(i - 1));

            }

            steps = savedSteps;
            StartCoroutine(Move());
            StartCoroutine(RevertArrowResponse());
            foreach(GameObject altarrow in GameObject.FindGameObjectsWithTag("altRouteArrow")){
                Destroy(altarrow);
            }
            
            Destroy(GameObject.Find("routeArrow")); 
            canEndTurn = true;
        }
        if (arrowResponse == 3)
        {
            arrowResponse = -1;

            int skipSquares = currentRoute[routePosition].GetComponent<SquareType>().skipSquares;

            int altSquares = altWp2.transform.parent.transform.childCount;

            for (int i = 1; i <= skipSquares; i++)
            {
                currentRoute.Remove(currentRoute[routePosition + 1]);
            }
            for (int i = 1; i <= altSquares; i++)
            {
                currentRoute.Insert(routePosition + i, altWp2.transform.parent.transform.GetChild(i - 1));

            }

            steps = savedSteps;
            StartCoroutine(Move());
            StartCoroutine(RevertArrowResponse());
            foreach (GameObject altarrow in GameObject.FindGameObjectsWithTag("altRouteArrow"))
            {
                Destroy(altarrow);
            }

            Destroy(GameObject.Find("routeArrow"));
            canEndTurn = true;
        }
    }

    IEnumerator RevertArrowResponse()
    {
        yield return new WaitForSeconds(2f);
        arrowResponse = 0;
    }

    public void MovePlayer()
    {
        steps = master.dice1.GetComponent<DiceControl>().resultValue + master.dice2.GetComponent<DiceControl>().resultValue;
        StartCoroutine(Move());
    }

    IEnumerator Move(){
        while(steps > 0){
            if (routePosition + 1 == currentRoute.Count){
                master.GameOver();
                break;
            }
            Vector3 nextPos = currentRoute[routePosition + 1].position;
            while (MoveToNext(nextPos)){
                yield return null;
            }
            if (currentRoute[routePosition+1].GetComponent<SquareType>() != null)
            {
                if(currentRoute[routePosition + 1].GetComponent<SquareType>().type == 1 && arrowResponse == 0) //si es tipo altpath
                {
                    Vector3 currentPos = currentRoute[routePosition + 1].transform.position;
                    
                    currentPos = (currentRoute[routePosition + 2].transform.position + currentRoute[routePosition + 1].transform.position) / 2;
                    currentPos.y += 0.2f;
                    GameObject arrowPrefab = Instantiate(arrow, currentPos, Quaternion.identity);
                    arrowPrefab.transform.LookAt(currentRoute[routePosition + 2].transform);
                    Quaternion rot = arrowPrefab.transform.rotation;
                    rot *= Quaternion.Euler(0, 90, 0);
                    arrowPrefab.transform.rotation = rot;
                    arrowPrefab.name = "routeArrow";
                    int counter = 1;

                    foreach (GameObject alt in GameObject.FindGameObjectsWithTag("altStart"))
                    {
                        if (Vector3.Distance(currentRoute[routePosition + 1].transform.position, alt.transform.position) < 3f)
                        {
                            if (counter == 1) altWp1 = alt;
                            if (counter == 2) altWp2 = alt;
                            currentPos = (alt.transform.position + currentRoute[routePosition + 1].transform.position) / 2;
                            currentPos.y += 0.2f;
                            GameObject arrowAltPrefab = Instantiate(arrow, currentPos, Quaternion.identity);
                            arrowAltPrefab.transform.LookAt(alt.transform);
                            Quaternion rotAlt = arrowAltPrefab.transform.rotation;
                            rotAlt *= Quaternion.Euler(0, 90, 0);
                            arrowAltPrefab.transform.rotation = rotAlt;
                            arrowAltPrefab.name = "altRouteArrow" + counter;
                            arrowAltPrefab.tag = "altRouteArrow";
                            counter++;
                        }
                    }
                    savedSteps = steps-1;
                    if(savedSteps == 0){
                        canEndTurn = true;
                    }else{
                        canEndTurn = false;
                    }
                    steps = 1;
                }
                if (currentRoute[routePosition + 1].GetComponent<SquareType>().type == 2){ //si es tipo tp
                    //desaparece
                    transform.GetComponentInChildren<MeshRenderer>().material = disolveMat;
                    transform.GetChild(0).GetComponent<Animator>().enabled = true;
                    transform.GetChild(0).GetComponent<Animator>().Play("P" + master.whosTurn + "Disolve");
                    StartCoroutine(WaitDisolve("player"));
                    forceField.GetComponent<AudioSource>().Play();
                    while (!disolvePlayerOver)
                    {
                        yield return null;
                    }
                    disolvePlayerOver = false;
                    canEndTurn = false;

                    //transporta
                    transform.position = currentRoute[currentRoute[routePosition + 1].GetComponent<SquareType>().tpto].position;
                    routePosition = currentRoute[routePosition + 1].GetComponent<SquareType>().tpto;
                    ResetRoute();

                    //aparece
                    StartCoroutine(WaitAppear());
                    transform.GetChild(0).GetComponent<Animator>().Play("P" + master.whosTurn + "Appear");
                    forceField.GetComponent<AudioSource>().Play();
                    while (!appearPlayerOver)
                    {
                        yield return null;
                    }
                    appearPlayerOver = false;
                    transform.GetChild(0).GetComponent<Animator>().enabled = false;
                    transform.GetComponentInChildren<MeshRenderer>().material = defaultMat;
                    canStartMinigame = false;
                    canEndTurn = true;
                    break;
                }
                
                if (currentRoute[routePosition + 1].GetComponent<SquareType>().type == 3) { //si es tipo puerta
                    if (master.players.transform.GetChild(master.whosTurn - 1).GetComponent<Points>().points >= 
                        currentRoute[routePosition + 1].GetComponent<SquareType>().doorCost){
                        master.players.transform.GetChild(master.whosTurn - 1).GetComponent<Points>().points -= 
                            currentRoute[routePosition + 1].GetComponent<SquareType>().doorCost;
                        GameObject doorInst = currentRoute[routePosition + 1].GetComponent<SquareType>().doori;

                        for (int i = 0; i < doorInst.transform.childCount; i++){
                            doorInst.transform.GetChild(i).GetComponent<Animator>().enabled = true;
                            doorInst.transform.GetChild(i).GetComponent<Animator>().Play("PilarsDisolve");
                        }
                        master.ShowMessage("Obstaculo eliminado!", 3f);
                        GetComponent<AudioSource>().PlayOneShot(openDoorSFX);

                        StartCoroutine(WaitDisolve("obstacle"));
                        while (!disolveObstacleOver){
                            yield return null;
                        }
                        disolveObstacleOver = false;
                        
                        Destroy(doorInst);
                        currentRoute[routePosition + 1].GetComponent<SquareType>().type = -1; //eliminar efecto especial a la casilla
                    }else {
                        //desaparece
                        transform.GetComponentInChildren<MeshRenderer>().material = disolveMat;
                        transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        transform.GetChild(0).GetComponent<Animator>().Play("P" + master.whosTurn + "Disolve");
                        StartCoroutine(WaitDisolve("player"));
                        forceField.GetComponent<AudioSource>().Play();
                        while (!disolvePlayerOver){
                            yield return null;
                        }
                        disolvePlayerOver = false;
                        canEndTurn = false;

                        //transporta
                        transform.position = currentRoute[currentRoute[routePosition + 1].GetComponent<SquareType>().tpto].position;
                        routePosition = currentRoute[routePosition + 1].GetComponent<SquareType>().tpto;
                        ResetRoute();
                        master.ShowMessage("No tienes los puntos necesarios", 3f);

                        //aparece
                        StartCoroutine(WaitAppear());
                        transform.GetChild(0).GetComponent<Animator>().Play("P" + master.whosTurn + "Appear");
                        forceField.GetComponent<AudioSource>().Play();
                        while (!appearPlayerOver)
                        {
                            yield return null;
                        }
                        appearPlayerOver = false;
                        transform.GetChild(0).GetComponent<Animator>().enabled = false;
                        transform.GetComponentInChildren<MeshRenderer>().material = defaultMat;
                        canStartMinigame = false;
                        canEndTurn = true;
                        break;
                    }
                    
                }
            }
            yield return new WaitForSeconds(0.1f);
            steps--;
            routePosition++;
        }
        if (canEndTurn) {
            if (canStartMinigame)
            {
                if(currentRoute.Count != routePosition + 1) master.GetComponent<MasterScript>().StartMinigame();
            }else{
                canStartMinigame = true;
                master.GetComponent<MasterScript>().endTurnButton.GetComponent<Button>().interactable = true;
            }
            canEndTurn = false;
        }
        
    }

    IEnumerator WaitDisolve(string type)
    {
        yield return new WaitForSeconds(1.9f);
        if(type == "player"){
            disolvePlayerOver = true;
        } else {
            disolveObstacleOver = true;
        }
    }
    IEnumerator WaitAppear()
    {
        yield return new WaitForSeconds(1.9f);
        appearPlayerOver = true;
    }

    bool MoveToNext(Vector3 goal)
    {
        if (Vector3.Distance(transform.position, goal) > 0.5f && Vector3.Distance(transform.position, goal) < 1f) {
            float posy = goal.y + 0.2f;
            goal = new Vector3(goal.x, posy, goal.z);
        }
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, velocity * Time.deltaTime));
         
    }

    public IEnumerator TpBack(int stepsBack, int playerNumber)
    {
        
        //desaparece
        transform.GetComponentInChildren<MeshRenderer>().material = disolveMat;
        transform.GetChild(0).GetComponent<Animator>().enabled = true;
        transform.GetChild(0).GetComponent<Animator>().Play("P" + playerNumber + "Disolve");
        StartCoroutine(WaitDisolve("player"));
        forceField.GetComponent<AudioSource>().Play();
        while (!disolvePlayerOver)
        {
            yield return null;
        }
        disolvePlayerOver = false;
        canEndTurn = false;

        //transporta
        if (routePosition - stepsBack > 0)
        {
            transform.position = currentRoute[routePosition-stepsBack].position;
            routePosition -= stepsBack;
        }else {
            transform.position = currentRoute[0].position;
            routePosition = 0;
        }

        //aparece
        StartCoroutine(WaitAppear());
        transform.GetChild(0).GetComponent<Animator>().Play("P" + playerNumber + "Appear");
        forceField.GetComponent<AudioSource>().Play();
        while (!appearPlayerOver)
        {
            yield return null;
        }
        appearPlayerOver = false;
        transform.GetChild(0).GetComponent<Animator>().enabled = false;
        transform.GetComponentInChildren<MeshRenderer>().material = defaultMat;
        canStartMinigame = false;
    }
}
