using UnityEngine;

public class IKPwUp : MonoBehaviour
{
    public MasterScript master;
    public GameObject SpawnVFX;
    public AudioClip PwUpSFX;

    void Start(){
        master = GameObject.Find("Master").GetComponent<MasterScript>();
        SpawnVFX = GameObject.Find("Explosion");
    }

    void Update(){
        transform.Rotate(0, 20 * Time.deltaTime, 0);

        foreach (Transform player in master.players.transform) {
            if (Vector3.Distance(player.transform.position, transform.position) < 0.4f) {
                if (player.GetComponent<Health>().healthPoints == 0){
                    Destroy(gameObject);
                }else{
                    GameObject vfx = Instantiate(SpawnVFX, transform.position, Quaternion.identity);
                    vfx.GetComponent<AudioSource>().enabled = true;
                    vfx.GetComponent<AudioSource>().PlayOneShot(PwUpSFX);
                    player.GetComponent<PwUpEffect>().IKPwUpCounter = 2;
                    player.GetComponent<PwUpEffect>().X2PwUpCounter = 0;
                    master.StartCoroutine(master.CleanSpawnVFX(vfx, 3f));
                    master.GetComponent<MasterScript>().DisplayPowerUp();
                    Destroy(gameObject);
                }
            }
        }
    }
}
