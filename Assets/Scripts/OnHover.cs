using UnityEngine;

public class OnHover : MonoBehaviour
{
    public bool isOver = true;

    void Update()
    {
        if (!isOver && transform.localScale != new Vector3(1.5f, 1.5f, 1.5f)){
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), Time.deltaTime * 10);
        }
        else if(isOver && transform.localScale != new Vector3(1, 1, 1)) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 10);
        }
    }

    public void MouseOver(){
        isOver = false;
    }
    public void MouseOff() {
        isOver = true;
    }
}
