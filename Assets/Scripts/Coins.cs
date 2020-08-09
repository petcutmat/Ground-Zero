using UnityEngine;

public class Coins : MonoBehaviour
{
    public MasterScript master;
    public GameObject SpawnVFX;

    void Start(){
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        SpawnVFX = GameObject.Find("Explosion");
    }

    void Update(){
        transform.GetChild(1).transform.Rotate(0, 20 * Time.deltaTime, 0);

        foreach(Transform player in master.players.transform){
            if (Vector3.Distance(player.transform.position, transform.position) < 0.4f){
                GameObject vfx = Instantiate(SpawnVFX, transform.position, Quaternion.identity);
                vfx.GetComponent<AudioSource>().enabled = true;
                vfx.GetComponent<AudioSource>().Play();
                player.GetComponent<Points>().addPoints(100);
                master.StartCoroutine(master.CleanSpawnVFX(vfx,1));
                master.icoins.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }
}

