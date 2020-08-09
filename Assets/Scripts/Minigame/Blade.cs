using UnityEngine;

public class Blade : MonoBehaviour
{
    bool isCutting = false;
    public GameObject bladeTrailPrefab;
    Camera cam;
    float cdr = 0.15f;
    public float cdrAlt = 0.15f;
    Vector3 previousPosition;

    void Start(){
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
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
