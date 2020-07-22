using UnityEngine;

public class Coins : MonoBehaviour
{
    public MasterScript master;

    void Start()
    {
        master = GameObject.Find("Master").GetComponent<MasterScript>();
    }

    void Update()
    {
        transform.GetChild(1).transform.Rotate(0, 20 * Time.deltaTime, 0);

        foreach( Transform player in master.players.transform)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 0.4f)
            {
                player.GetComponent<Points>().points += 100;
                master.icoins.Remove(gameObject);
                Destroy(gameObject);
            }
        }
        
    }
}
