using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    private bool cloud1Back = false;
    private bool cloud2Back = false;
    private bool cloud3Back = false;
    private bool cloud4Back = false;

    void Start(){
        transform.GetChild(1).localScale = new Vector3(0.1f, 0.1f, 0);
        transform.GetChild(2).localScale = new Vector3(0.1f, 0.1f, 0);
        transform.GetChild(3).localScale = new Vector3(0.1f, 0.1f, 0);
        transform.GetChild(4).localScale = new Vector3(0.1f, 0.1f, 0);
        if (transform.name == "secondSmkBmb") transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        if (transform.name == "secondSmkBmb") transform.GetChild(4).GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update(){
        int velocity = 1;
        if (transform.name == "secondSmkBmb") velocity = 3;

        transform.GetChild(0).transform.Rotate(0, 0, -20 * Time.deltaTime);
        if (transform.GetChild(1).localScale.x > 1) cloud1Back = true;
        if (transform.GetChild(1).localScale.x <= 1 && !cloud1Back){
            transform.GetChild(1).localPosition += new Vector3(-1, 1, 0) * Time.deltaTime * 0.5f * velocity;
            transform.GetChild(1).localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.5f;
        } else {
            if (transform.GetChild(1).localScale.x >= 0)
                transform.GetChild(1).localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 0.5f;
        }
        if (transform.GetChild(2).localScale.x > 1) cloud2Back = true;
        if (transform.GetChild(2).localScale.x <= 1 && !cloud2Back){
            transform.GetChild(2).localPosition += new Vector3(1, 1, 0) * Time.deltaTime * 0.8f * velocity;
            transform.GetChild(2).localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.8f;
        }else{
            if(transform.GetChild(2).localScale.x >= 0)
            transform.GetChild(2).localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 0.8f;
        }
        if (transform.GetChild(3).localScale.x > 1) cloud3Back = true;
        if (transform.GetChild(3).localScale.x <= 1 && !cloud3Back){
            transform.GetChild(3).localPosition += new Vector3(1,1, 0) * Time.deltaTime * 0.8f *velocity;
            transform.GetChild(3).localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.8f;
        }else{
            if (transform.GetChild(3).localScale.x >= 0)
                transform.GetChild(3).localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 0.8f;
        }
        if (transform.GetChild(4).localScale.x > 1 ) cloud4Back = true;
        if (transform.GetChild(4).localScale.x <= 1 && !cloud4Back){
            transform.GetChild(4).localPosition += new Vector3(-1, -1, 0) * Time.deltaTime * 0.6f * velocity;
            transform.GetChild(4).localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.6f;
        }else{
            if (transform.GetChild(4).localScale.x >= 0)
                transform.GetChild(4).localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 0.6f;
        }
    }
}
