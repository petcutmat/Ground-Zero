using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Route currentRoute;
    public MasterScript master;
    int routePosition;
    public int steps;
    public float velocity = 3f;

    public void Update(){
        if (steps >= 0 && currentRoute.childList.Count == routePosition + 1){
            steps = -1;
            master.GameOver();
        }
    }

    public void MovePlayer()
    {
        steps = master.dice1.GetComponent<DiceControl>().resultValue + master.dice2.GetComponent<DiceControl>().resultValue;
        StartCoroutine(Move());
    }

    IEnumerator Move(){
        while(steps > 0){
            if (routePosition + 1 == currentRoute.childList.Count){
                master.GameOver();
                break;
            }
            Vector3 nextPos = currentRoute.childList[routePosition + 1].position;
            while (MoveToNext(nextPos)){
                yield return null;
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
