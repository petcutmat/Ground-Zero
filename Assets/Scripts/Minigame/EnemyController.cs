using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public GameObject character;
    public GameObject master;
    public GameObject bloodSlice;

    private void Start()
    {
        character =  GameObject.FindGameObjectWithTag("Player");
        master = GameObject.FindGameObjectWithTag("Master");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Blade")){
            GameObject bloodSlicei = Instantiate(bloodSlice, transform.parent);
            if (CompareTag("Boss")) { bloodSlicei.transform.localScale += new Vector3(1, 1, 0) * 150f; }
            bloodSlicei.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            Vector3 pos = transform.GetComponent<RectTransform>().localPosition;
            pos.z = 75; //corregir eje z
            bloodSlicei.GetComponent<RectTransform>().localPosition = pos;

            int points = 20;
            if(master.gameObject.GetComponent<MiniGame>().minigameType == 0)
            {
                points = 5;
            }
            master.GetComponent<MasterScript>().players.transform.GetChild(
                master.GetComponent<MasterScript>().whosTurn-1).GetComponent<Points>().addPoints(points);
            master.GetComponent<MiniGame>().counter += master.GetComponent<MasterScript>().players.transform.GetChild(
                master.GetComponent<MasterScript>().whosTurn - 1).GetComponent<Points>().addPoints(points);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (master.GetComponent<MiniGame>().endMinigame == true) Destroy(gameObject);

        //Calcular posicion entre jugador y enemigo
        Vector3 direction = character.transform.position - transform.position;
        direction.Normalize();

        //Movimiento
        Vector3 velocity = direction * speed;
        GetComponent<Rigidbody2D>().velocity = new Vector3(velocity.x, velocity.y, velocity.z);

        //Rotación hacia jugador
        float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        //Golpear a jugador
        if (Vector3.Distance(transform.position, character.transform.position) < 0.3f){
            character.GetComponent<CharacterController>().Hit();
        }
    }
}
