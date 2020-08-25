using System.Collections;
using UnityEngine;

public class DiceControl : MonoBehaviour
{
    public bool isRolling;
    private Vector3 initPos;
    public int resultValue;
    public bool rotFast;

    void Awake(){
        initPos = transform.position; //guardar posición inicial del gameobject
        ResetDice();
    }

    void Update(){        
        if(!isRolling && resultValue == 0) transform.Rotate(0, 20 * Time.deltaTime, 40 * Time.deltaTime); //girar infinitamente hasta ser lanzado
        if(rotFast) transform.Rotate(200 * Time.deltaTime, 200 * Time.deltaTime, 200 * Time.deltaTime);
    }

    public void RollDice(){
        isRolling = true;
        StartCoroutine(RotateFast());
        GetComponent<Rigidbody>().useGravity = true; //dejar caer dados
        StartCoroutine(WaitForValue());
    }

    IEnumerator RotateFast()
    {
        rotFast = true;
        yield return new WaitForSeconds(0.5f);
        rotFast = false;
    }

    IEnumerator WaitForValue(){
        yield return new WaitForSeconds(3); //si el dado no ha dado un valor en 3 segundos repetir tirada
        
        if(resultValue == 0){
            ResetDice();
            RollDice(); 
        }
    }

    public void ResetDice(){ //reiniciar posición de los dados
        for (int i = 0; i < transform.childCount; i++){ //reiniciar caras del dado
            transform.GetChild(i).GetComponent<DiceSide>().diceValue = 0;
        }
        transform.rotation = Random.rotation;
        isRolling = false;
        GetComponent<Rigidbody>().useGravity = false;
        transform.position = initPos;
        resultValue = 0;
    }
}
