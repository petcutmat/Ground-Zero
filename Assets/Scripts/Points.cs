using UnityEngine;

public class Points : MonoBehaviour
{
    public int points;
    public int multiplier =1;

    private void Start(){
        points = 1000;
    }

    public int addPoints(int points)
    {
        points = multiplier * points;
        SumPoints(points);
        return points;
    }
    public void SumPoints(int p)
    {
        points += p;
    }
}
