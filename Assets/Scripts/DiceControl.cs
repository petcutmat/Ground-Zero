using System.Collections;
using UnityEngine;

public class DiceControl : MonoBehaviour
{
    public bool isRolling;
    private Vector3 initPos;
    public int resultValue;
    public bool buttonPressed;

    void Awake(){
        initPos = transform.position; //guardar posición inicial del gameobject
        ResetDice();
    }

    void Update(){
        if (GetComponent<Rigidbody>().velocity == Vector3.zero && resultValue == 0 && isRolling){ //si cae y no recive un valor
            StartCoroutine(WaitForValue());
        }
        if(!isRolling) transform.Rotate(0, 20 * Time.deltaTime, 40 * Time.deltaTime); //girar infinitamente hasta ser lanzado
    }

    private IEnumerator WaitForValue(){ //esperar 1 segundo para recoger el valor, reiniciar dados y lanzarlos
        yield return new WaitForSeconds(1);
        if (resultValue == 0 && GetComponent<Rigidbody>().velocity == Vector3.zero){
            ResetDice();
            RollDice();
        }
    }

    public void RollDice(){
        isRolling = true;
        GetComponent<Rigidbody>().useGravity = true; //dejar caer dados
    }

    public void ResetDice(){ //reiniciar posición de los dados
        resultValue = 0;
        isRolling = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero; //no acumular velocidad de los dados
        transform.position = initPos;
    }
}
