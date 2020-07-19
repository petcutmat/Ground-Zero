using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public MasterScript master;
    public GameObject objectToFollow;

    public Vector3 offset; 
    public Vector3 centerPos;
    public float cameraSpeed = 2.0f;

    void Update()
    {
        //establecer centro de mira y personaje objetivo
        objectToFollow = master.players.transform.GetChild(master.whosTurn-1).gameObject;
        float centerRelativePos = objectToFollow.transform.position.x / 2;
        centerPos = new Vector3(centerRelativePos, centerPos.y, centerPos.z); 

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
