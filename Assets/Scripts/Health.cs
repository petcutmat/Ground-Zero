using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthPoints = 3;
    public Material blackMat;
    public Material defaultMat;

    void Start(){
        defaultMat = transform.GetChild(0)
            .GetComponent<MeshRenderer>().material;
    }
}
