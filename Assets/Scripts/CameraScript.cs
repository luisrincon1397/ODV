using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Prota;

    void Update()
    {
        if (Prota != null)
        {
            Vector3 position = transform.position;
            position.x = Prota.position.x;
            position.y = Prota.position.y;
            transform.position = position;
        }
    }
}