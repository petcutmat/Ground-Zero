using UnityEngine;

public class EasterEgg2 : MonoBehaviour
{
    public GameObject master;
    private bool isDone = false;
    public int number;

    void OnMouseDown()
    {
        if (!isDone)
        {
            if(master.GetComponent<MasterScript>().ee2 == 0) master.GetComponent<MasterScript>().ee2 += number;
            else master.GetComponent<MasterScript>().ee2 += number*10;

            if (master.GetComponent<MasterScript>().ee2 == 22)
            {
                isDone = true;
                master.GetComponent<MasterScript>().ee++;
            }else if (master.GetComponent<MasterScript>().ee2 == 2)
            {

            }else{
                isDone = true;
                master.GetComponent<MasterScript>().ee2 = 0;
                StartCoroutine(master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1).gameObject.GetComponent<Movement>().TpBack(10, master.GetComponent<MasterScript>().whosTurn - 1));
                
            }
            
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}

