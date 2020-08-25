using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public MasterScript master;
    public GameObject objectToFollow;

    public Vector3 offset; 
    public Vector3 centerPos;
    public float cameraSpeed = 2.0f;

    void Update() {

        //establecer centro de mira y personaje objetivo
        objectToFollow = master.players.transform.GetChild(master.whosTurn - 1).gameObject;
        float centerRelativePos = objectToFollow.transform.position.x / 2;
        centerPos = new Vector3(centerRelativePos, centerPos.y, centerPos.z);


        if (master.players.transform.GetChild(master.whosTurn - 1).position.z > 10 || master.players.transform.GetChild(master.whosTurn - 1).position.x < -5){
            offset.x = 3;
            offset.y = 1.5f;
            offset.z = 0;
            centerRelativePos = objectToFollow.transform.position.z;
            centerPos = new Vector3(-25, -7f, 30);
            centerPos = new Vector3(centerPos.x, centerPos.y, centerRelativePos);
        }
        if(master.players.transform.GetChild(master.whosTurn - 1).position.z > 18){
            offset.x = 0;
            offset.y = 1.5f;
            offset.z = -3;
            centerRelativePos = objectToFollow.transform.position.x;
            centerPos = new Vector3(-10, -7, 40);
            centerPos = new Vector3(centerRelativePos, centerPos.y, centerPos.z);
        }
        if (master.players.transform.GetChild(master.whosTurn - 1).position.x < -20)
        {
            offset.x = 3;
            offset.y = 1.9f;
            offset.z = -4;
            centerRelativePos = objectToFollow.transform.position.x;
            centerPos = new Vector3(-45, 2.5f, 29);
            centerPos = new Vector3(centerRelativePos, centerPos.y, centerPos.z);
        }
        if (master.players.transform.GetChild(master.whosTurn - 1).position.z < 10 && master.players.transform.GetChild(master.whosTurn - 1).position.x > -2)
        {
            offset.x = 0;
            offset.y = 1.9f;
            offset.z = -3;
            centerRelativePos = objectToFollow.transform.position.z;
            centerPos = new Vector3(1.5f, -7f, 20);
            centerPos = new Vector3(centerRelativePos, centerPos.y, centerPos.z);
        }


        //rotación de cámara
        Quaternion targetRotation = Quaternion.LookRotation(centerPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraSpeed * Time.deltaTime);

        //posición de cámara
        Vector3 position = transform.position;
        position.x = Mathf.Lerp(transform.position.x, objectToFollow.transform.position.x + offset.x, cameraSpeed * Time.deltaTime);
        position.y = Mathf.Lerp(transform.position.y, objectToFollow.transform.position.y + offset.y, cameraSpeed * Time.deltaTime);
        position.z = Mathf.Lerp(transform.position.z, objectToFollow.transform.position.z + offset.z, cameraSpeed * Time.deltaTime);
        transform.position = position;
    }
       
}
