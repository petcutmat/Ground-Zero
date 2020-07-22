using UnityEngine;

public class SquareType : MonoBehaviour
{
    public int type;
    public int skipSquares;
    public int tpto;
    public Material blueMat;
    public GameObject tp;
    public GameObject tpIndicator;
    public Route route;
    public int doorCost;
    public GameObject door;
    public GameObject doori;

    private void Start()
    {
        if (type == 1)
        {
            GetComponent<MeshRenderer>().material = blueMat;
        }
        if (type == 2)
        {
            Transform prevSquare = transform.parent.GetChild(transform.parent.childCount - 2);
            GameObject tp1 = Instantiate(tp, transform.position, Quaternion.identity);
            tp1.transform.LookAt(prevSquare);
            Quaternion rot = new Quaternion();
            rot.y = Random.value;
            Vector3 pos = route.childList[tpto].position;
            pos.y += 0.03f;
            GameObject tpi = Instantiate(tpIndicator, pos, rot);
        }
        if (type == 3)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    doori = Instantiate(door, transform.parent.GetChild(i+1).transform.position, Quaternion.identity);

                }
            }
            
        }
    }
}
