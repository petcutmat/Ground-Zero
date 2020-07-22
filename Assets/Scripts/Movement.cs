using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject route;
    public MasterScript master;
    int routePosition;
    public int steps;
    public float velocity = 3f;
    public GameObject arrow;
    public int arrowResponse = 0;
    public GameObject altWp;
    public int savedSteps;
    List<Transform> currentRoute = new List<Transform>();

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

            ResetRoute();

            StartCoroutine(Move());
            StartCoroutine(RevertArrowResponse());
            Destroy(GameObject.Find("altRouteArrow"));
            Destroy(GameObject.Find("routeArrow"));
        }
        if(arrowResponse == 2){
            arrowResponse = -1;
            
            int skipSquares = currentRoute[routePosition].GetComponent<SquareType>().skipSquares;

            int altSquares = altWp.transform.parent.transform.childCount;

            for (int i = 1; i <= skipSquares; i++) {
                currentRoute.Remove(currentRoute[routePosition + 1]);
            }
            for (int i = 1; i <= altSquares; i++)
            {
                currentRoute.Insert(routePosition + i, altWp.transform.parent.transform.GetChild(i-1));
            }

            steps = savedSteps;
            StartCoroutine(Move());
            StartCoroutine(RevertArrowResponse());
            Destroy(GameObject.Find("altRouteArrow"));
            Destroy(GameObject.Find("routeArrow"));
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

                    foreach (GameObject alt in GameObject.FindGameObjectsWithTag("altStart"))
                    {
                        if (Vector3.Distance(currentRoute[routePosition + 1].transform.position, alt.transform.position) < 3f)
                        {
                            altWp = alt;
                            currentPos = (alt.transform.position + currentRoute[routePosition + 1].transform.position) / 2;
                            currentPos.y += 0.2f;
                            GameObject arrowAltPrefab = Instantiate(arrow, currentPos, Quaternion.identity);
                            arrowAltPrefab.transform.LookAt(alt.transform);
                            Quaternion rotAlt = arrowAltPrefab.transform.rotation;
                            rotAlt *= Quaternion.Euler(0, 90, 0);
                            arrowAltPrefab.transform.rotation = rotAlt;
                            arrowAltPrefab.name = "altRouteArrow";
                        }
                    }
                    savedSteps = steps-1;
                    steps = 1;
                }
                if (currentRoute[routePosition + 1].GetComponent<SquareType>().type == 2){ //si es tipo tp
                    transform.position = currentRoute[currentRoute[routePosition + 1].GetComponent<SquareType>().tpto].position;
                    routePosition = currentRoute[routePosition + 1].GetComponent<SquareType>().tpto;
                    ResetRoute();
                    break;
                }
                if (currentRoute[routePosition + 1].GetComponent<SquareType>().type == 3) { //si es tipo puerta
                    if (master.players.transform.GetChild(master.whosTurn - 1).GetComponent<Points>().points >= currentRoute[routePosition + 1].GetComponent<SquareType>().doorCost)
                    {
                        master.players.transform.GetChild(master.whosTurn - 1).GetComponent<Points>().points -= currentRoute[routePosition + 1].GetComponent<SquareType>().doorCost;
                        Destroy(currentRoute[routePosition + 1].GetComponent<SquareType>().doori);
                    }
                    else
                    {
                        transform.position = currentRoute[currentRoute[routePosition + 1].GetComponent<SquareType>().tpto].position;
                        routePosition = currentRoute[routePosition + 1].GetComponent<SquareType>().tpto;
                        ResetRoute();
                        break;
                    }
                    
                }
            }
            yield return new WaitForSeconds(0.1f);
            steps--;
            routePosition++;
        }
    }

    bool MoveToNext(Vector3 goal)
    {
        if (Vector3.Distance(transform.position, goal) > 0.5f && Vector3.Distance(transform.position, goal) < 1f) {
            float posy = goal.y + 0.2f;
            goal = new Vector3(goal.x, posy, goal.z);
        }
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, velocity * Time.deltaTime));
         
    }
}
