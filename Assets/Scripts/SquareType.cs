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
    public int realSquare;

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
    public int GetRandomSquare()
    {
        int[] squares = new int[5];
        squares[0] = 32;
        squares[1] = 32;
        squares[2] = 32;
        squares[3] = 32;
        squares[4] = 40;

        return squares[Random.Range(0, squares.Length)];
    }
}
