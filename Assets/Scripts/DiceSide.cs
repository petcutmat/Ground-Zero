using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public int sideNumber;
    public int diceValue;
    public GameObject dice;
    public GameObject ground;

    private void Update()
    {
       if (diceValue != sideNumber && transform.position.y < 0.1 && dice.GetComponent<Rigidbody>().velocity == Vector3.zero)
       {
            diceValue = sideNumber;
            transform.parent.GetComponent<DiceControl>().resultValue = sideNumber;
       }
        
    }
}
