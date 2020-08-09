using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public GameObject master;

    bool burn = false;

    void Update()
    {
        if(Vector3.Distance(master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1).transform.position, transform.position) < 1f
            && master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Movement>().litcounter == 0 && !burn)
        {
            master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Health>().healthPoints -= 1;
            master.GetComponent<MasterScript>().UpdateHealthBar();
            burn = true;
            GetComponent<AudioSource>().Play();
            StartCoroutine(WaitBurn());
        }
    }

    IEnumerator WaitBurn()
    {
        yield return new WaitForSeconds(1.5f);
        burn = false;
    }
}
