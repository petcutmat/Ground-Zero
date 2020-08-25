using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    public GameObject master;
    private bool isDone = false;

    void OnMouseDown()
    {
        if (!isDone)
        {
            isDone = true;
            master.GetComponent<MasterScript>().ee++;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}

