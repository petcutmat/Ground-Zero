using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public GameObject master;

    bool burn = false;

    void Update()
    {
        Transform currentPlayer = master.GetComponent<MasterScript>().players.transform.GetChild(master.GetComponent<MasterScript>().whosTurn - 1);
        if (Vector3.Distance(currentPlayer.transform.position, transform.position) < 1f
            && currentPlayer.GetComponent<Movement>().litcounter == 0 && !burn)
        {
            currentPlayer.GetComponent<Health>().healthPoints -= 1;
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
