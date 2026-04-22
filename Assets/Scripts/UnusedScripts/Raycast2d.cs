using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast2D : MonoBehaviour
{
    public Transform objectToPlace;
    public Camera gameCamera;

    void Update()
    {
        Vector2 mousePosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hitInfo.collider != null)
        {
            // Uncomment to instantiate on click
            // if (Input.GetKeyDown(KeyCode.Mouse0))
            // {
            //     Instantiate(objectToPlace, hitInfo.point, Quaternion.identity);
            // }

            objectToPlace.position = new Vector3(hitInfo.point.x, hitInfo.point.y, objectToPlace.position.z);
        }
    }
}