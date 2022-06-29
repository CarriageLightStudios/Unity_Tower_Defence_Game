using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float rotationSpeed = 2f;
    float dragSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(2))
        {
            if (x != 0)
            {
                transform.position += -(transform.right * (x * dragSpeed));
            }

            if (y != 0)
            {
                transform.position += -(transform.up * (y * dragSpeed));
            }
        }
        if (Input.GetMouseButton(1))
        {
            transform.eulerAngles += rotationSpeed * new Vector3(-y, x, 0.0f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            transform.position += transform.forward * 1;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            transform.position += -(transform.forward * 1);
        }
    }
}
