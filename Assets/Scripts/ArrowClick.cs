using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowClick : MonoBehaviour
{
    public MasterScript master;

    private void Start()
    {
        master = GameObject.Find("Master").GetComponent<MasterScript>();
    }
    void OnMouseDown()
    {
        if(gameObject.name == "altRouteArrow1"){
            master.players.transform.GetChild(master.whosTurn - 1).GetComponent<Movement>().arrowResponse = 2;
        } else if (gameObject.name == "altRouteArrow2"){
                master.players.transform.GetChild(master.whosTurn - 1).GetComponent<Movement>().arrowResponse = 3;
        }else {
            master.players.transform.GetChild(master.whosTurn - 1).GetComponent<Movement>().arrowResponse = 1;
        }
    }
}
