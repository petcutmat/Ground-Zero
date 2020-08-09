using UnityEngine;

public class SquareType : MonoBehaviour
{
    public int type;
    public int skipSquares;
    public int tpto;
    public Route route;
    public int doorCost;
    public GameObject door;
    public GameObject doori;
    public bool random;

    private void Start() {
        if (random) tpto = GetRandomSquare();
        if (type == 3){
            for (int i = 0; i < transform.parent.childCount; i++) {
                if (transform.parent.GetChild(i) == transform){
                    doori = Instantiate(door, transform.parent.GetChild(i+1).transform.position, Quaternion.identity);

                }
            }
        }
    }
    int GetRandomSquare()
    {
        int[] squares = new int[6];
        squares[0] = 15;
        squares[1] = 17;
        squares[2] = 20;
        squares[3] = 25;
        squares[4] = 30;
        squares[5] = 35;

        return squares[Random.Range(0, squares.Length)];
        ;
    }
}
