using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float speed = 40f;
    public GameObject character;
    public GameObject master;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        master = GameObject.FindGameObjectWithTag("Master");
    }

    void Update(){
        GetComponent<RectTransform>().localPosition = Vector2.MoveTowards(GetComponent<RectTransform>().localPosition, new Vector2(0, 0), Time.deltaTime * speed);
        transform.Rotate(0, 0, 40 * Time.deltaTime);

        if (Vector3.Distance(character.transform.position, transform.position) < 0.3f){
            character.GetComponent<CharacterController>().Slow();
        }

        if (Vector3.Distance(GetComponent<RectTransform>().localPosition, new Vector2(0, 0)) < 500f && GetComponent<RectTransform>().localScale.x > 0){
            transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 0.07f;
        }

        if (Vector3.Distance(GetComponent<RectTransform>().localPosition, new Vector2(0, 0)) < 0.3f || master.GetComponent<MiniGame>().endMinigame){
            Destroy(gameObject);
        }

    }
}
