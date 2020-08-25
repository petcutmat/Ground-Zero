using UnityEngine;

public class FireSpirit : MonoBehaviour
{
    public GameObject master;
    Vector3 pos1 = new Vector3(-0.35f, 2.25f, 9);
    Vector3 pos2 = new Vector3(-23f, 2.25f, 9);
    Vector3 pos3 = new Vector3(-23f, 2.25f, 30);
    Vector3 pos4 = new Vector3(-0.35f, 2.25f, 30);
    Vector3 goal = new Vector3(-0.35f, 2.25f, 30);


    void Update()
    {
        if (transform.position == pos1) goal = pos2;
        if (transform.position == pos2) goal = pos3;
        if (transform.position == pos3) goal = pos4;
        if (transform.position == pos4) goal = pos1;
        transform.position = Vector3.MoveTowards(transform.position, goal, 1.5f * Time.deltaTime);
        Transform currentPlayer = master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1);
        if (Vector3.Distance(currentPlayer.transform.position,transform.position) < 3 
            && currentPlayer.transform.childCount <3)
        {
            Transform light = Instantiate(transform.GetChild(0), currentPlayer);
            light.transform.position = currentPlayer.GetChild(0).transform.position;
            light.SetAsLastSibling();
            light.GetComponent<Light>().intensity = 2000;
            currentPlayer.GetComponent<Movement>().litcounter = 3;
        }
    }
}
