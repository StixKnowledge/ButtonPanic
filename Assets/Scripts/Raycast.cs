using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public Transform objectToPlace;
    public Camera gameCamera;

    // Update is called once per frame
    void Update()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            objectToPlace.transform.position = new Vector3(hitInfo.point.x, .5f, hitInfo.point.z);
        }

    }
}
