using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Blood : MonoBehaviour
{
    public int points = 20;

    void Start() {
        transform.GetChild(0).transform.rotation = Quaternion.identity;
        GetComponent<AudioSource>().Play();
        StartCoroutine(WaitAnim());
        GameObject master = GameObject.Find("Master");
        if (master.GetComponent<MasterScript>().players.transform.GetChild(
            master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<PwUpEffect>().X2PwUpCounter > 0) points *= 2;
        if (master.GetComponent<MiniGame>().minigameType == 0) transform.GetChild(0).GetComponent<Text>().text =
                "+" + (points/4).ToString() + "p";
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = "+" + points + "p";
        }
    }

    void Update(){
        
        transform.GetChild(0).localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.03f;
    }

    IEnumerator WaitAnim(){
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}

