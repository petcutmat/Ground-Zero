using UnityEngine;

public class Blade : MonoBehaviour
{
    bool isCutting = false;
    public GameObject bladeTrailPrefab;
    Camera cam;
    
    private float cutDelayAlt = 0.3f;
    public float cutDelay = 0.4f;
    public float cdrAlt = 0.15f;
    float cdr = 0.15f;

    Vector3 previousPosition;

    void Start(){
        cam = Camera.main;
        if (PlayerPrefs.GetInt("goldenSword") == 1){
            cdrAlt = 0.35f;
            bladeTrailPrefab.GetComponent<TrailRenderer>().startColor = new Color(1,0.9f,0.10f);
            bladeTrailPrefab.GetComponent<TrailRenderer>().endWidth = 0.03f;
            bladeTrailPrefab.GetComponent<TrailRenderer>().startWidth = 0.03f;
        }
        else
        {
            cdrAlt = 0.15f;
            bladeTrailPrefab.GetComponent<TrailRenderer>().startColor = new Color(1, 1, 1);
            bladeTrailPrefab.GetComponent<TrailRenderer>().endWidth = 0.025f;
            bladeTrailPrefab.GetComponent<TrailRenderer>().startWidth = 0.025f;
        }
    }

    void Update()
    {
        if(cutDelay > 0) cutDelay -= 1f * Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && cutDelay <= 0){
            cutDelay = cutDelayAlt;
            StartCutting();
        } else if (Input.GetMouseButtonUp(0)){
            cdr =cdrAlt;
            StopCutting();
        }

        if (isCutting) {
            cdr -= 1f * Time.deltaTime;
            if (cdr > 0 ){
                UpdateCut();
            }
        }
    }

    void UpdateCut(){
        var mousePos = Input.mousePosition;
        mousePos.z = 1.4f; //corregir distancia z
        transform.position = cam.ScreenToWorldPoint(mousePos);

        float velocity = (mousePos - previousPosition).magnitude * Time.deltaTime;
        if (velocity > .15) {
            GetComponent<CircleCollider2D>().enabled = true;
        } else {
            GetComponent<CircleCollider2D>().enabled = false;
        }
        previousPosition = mousePos;
    }

    void StartCutting(){
        var mousePos = Input.mousePosition;
        mousePos.z = 1.4f; //corregir distancia z
        GetComponent<RectTransform>().localPosition = cam.ScreenToWorldPoint(mousePos);
        GetComponent<AudioSource>().Play();
        isCutting = true;
        Instantiate(bladeTrailPrefab, transform);
    }

    void StopCutting(){
        isCutting = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }

}
