
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwordController : MonoBehaviour
{
    private GameObject master;
    public AudioClip unsheatheSFX;

    private void Start(){
        master = GameObject.FindGameObjectWithTag("Master");
    }

    void Update()
    {
        transform.Rotate(0, 0, -50 * Time.deltaTime);
        if (Vector3.Distance(master.GetComponent<MiniGame>().character.transform.position, transform.position) < 0.2f && GetComponent<Image>().enabled){
            master.GetComponent<MiniGame>().blade.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(unsheatheSFX);
            GetComponent<Image>().enabled = false;
            foreach(GameObject z in GameObject.FindGameObjectsWithTag("Zombie"))
            {
                z.GetComponent<EnemyController>().speed += 0.5f;
            }
            StartCoroutine(WaitAudio());
        }
        if(master.GetComponent<MiniGame>().endMinigame)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitAudio()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
