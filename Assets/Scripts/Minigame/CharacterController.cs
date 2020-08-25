using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public float speed = 1.6f;
    public Vector3 startpos;
    public int health; //cambiar a 3 en escena
    private Color c;
    public float cdHit;
    public float cdSlow;
    public GameObject master;
    public int charges = 0;
    public GameObject smokeBomb;

    private void Awake()
    {
        startpos = transform.position;
        c = GetComponent<Image>().color;
        master = GameObject.Find("Master");
        SpawnCharacter();
    }

    private void Update(){
        //inputs
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        //movimiento
        transform.position = transform.position + movement * Time.deltaTime * speed;
        //paredes
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, startpos.x - 2.35f, startpos.x + 2.35f),
            Mathf.Clamp(transform.position.y, startpos.y - 0.9f, startpos.y+0.9f), transform.position.z);
        if ((Input.GetMouseButtonDown(1)) && charges >=1)
        {
            GameObject smkbmb = Instantiate(smokeBomb, transform.parent);
            Vector3 pos = transform.GetComponent<RectTransform>().localPosition;
            pos.z = 75; //corregir eje z
            smkbmb.GetComponent<RectTransform>().localPosition = pos;
            smkbmb.name = "firstSmkBmb";
            smkbmb.GetComponent<AudioSource>().Play();
            GameObject smkbmb2 = Instantiate(smokeBomb, transform);
            smkbmb2.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
            smkbmb2.GetComponent<RectTransform>().localScale= new Vector3(9f, 9f, 0);
            smkbmb2.name = "secondSmkBmb";
            charges -= 1;
            var mousePos = Input.mousePosition;
            mousePos.z = 2f; //corregir distancia z
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    public void SpawnCharacter(){
        transform.position = startpos;
        health = 3;
        StartCoroutine(WaitCD(0));
        speed = 1.6f;
    }

    public void Hit() {
        if(cdHit == 0){
            c.a = 0.5f;
            GetComponent<Image>().color = c;
            health -= 1;
            cdHit = 0.5f;
            Color hitColor = new Color(0.63f, 0.17f, 0.17f);
            transform.parent.GetComponent<Image>().color = hitColor;
            GetComponent<AudioSource>().Play();
            master.GetComponent<MasterScript>().players.transform.GetChild(
               master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Points>().points -= 40;
            master.GetComponent<MiniGame>().counter -= 40;
            StartCoroutine(WaitCD(cdHit));
        }
    }

    IEnumerator WaitCD(float seconds){
        yield return new WaitForSeconds(seconds);
        cdHit = 0;
        c.a = 1f;
        transform.parent.GetComponent<Image>().color = Color.black;
        GetComponent<Image>().color = c;
    }

    public void Slow(){
        if (cdSlow == 0){
            Color c2 = new Color(1, 0.6f, 0.6f);
            GetComponent<Image>().color = c2;
            cdSlow = 1.5f;
            StartCoroutine(SlowCD(cdSlow));
            speed /= 2f;
        }
    }

    IEnumerator SlowCD(float seconds){
        yield return new WaitForSeconds(seconds);
        cdSlow = 0;
        speed *= 2;
        GetComponent<Image>().color = c;
    }
}
