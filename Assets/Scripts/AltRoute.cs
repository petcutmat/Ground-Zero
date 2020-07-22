using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltRoute : MonoBehaviour
{ 
    Transform[] childObjects;
    public List<Transform> childList = new List<Transform>();

private void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Fill();

    for (int i = 0; i < childList.Count; i++)
    {
        Vector3 currentPos = childList[i].position;
        if (i > 0)
        {
            Vector3 prevPos = childList[i - 1].position;
            Gizmos.DrawLine(prevPos, currentPos);
        }
    }
}

void Fill()
{
    childList.Clear();
    childObjects = GetComponentsInChildren<Transform>();
    foreach (Transform child in childObjects)
    {
        if (child != transform) childList.Add(child);
    }
}
}
