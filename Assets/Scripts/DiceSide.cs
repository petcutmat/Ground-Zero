using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public int sideNumber;
    public int diceValue;
    public GameObject dice;
    public GameObject ground;

    private void Update(){ //si el dado ya no se mueve y la cara del dado toca suelo
       if (diceValue != sideNumber && transform.position.y < 0.1 && dice.GetComponent<Rigidbody>().velocity == Vector3.zero){
            diceValue = sideNumber;
            transform.parent.GetComponent<DiceControl>().resultValue = sideNumber;
       }
    }
}
