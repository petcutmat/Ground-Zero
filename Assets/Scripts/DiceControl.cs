using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControl : MonoBehaviour
{
    public bool isRolling;
    private Vector3 initPos;
    public int resultValue;
    public bool buttonPressed;

    void Awake(){
        resultValue = 0;
        initPos = transform.position;
        GetComponent<Rigidbody>().useGravity = false; //dado sin gravedad antes de ser lanzado
        transform.Rotate(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)); //empezar en posición aleatoria
        isRolling = false;
    }
    void Update(){
        if (buttonPressed) {
            buttonPressed = false;
            isRolling = true;
            GetComponent<Rigidbody>().useGravity = true; //dejar caer dados
        }
        if (GetComponent<Rigidbody>().velocity == Vector3.zero && resultValue == 0 && isRolling){ //si cae y no recive un valor
            StartCoroutine(WaitForValue());
        }
        if(!isRolling) transform.Rotate(0, 20 * Time.deltaTime, 40 * Time.deltaTime); //girar infinitamente hasta ser lanzado
    }

    private IEnumerator WaitForValue(){
        yield return new WaitForSeconds(1);
        if (resultValue == 0 && GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            ResetDice();
            GetComponent<Rigidbody>().useGravity = true;
            isRolling = true;
        }
    }

    public void ResetDice(){
        resultValue = 0;
        isRolling = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero; //no acumular velocidad de los dados
        transform.position = initPos;
    }
}
