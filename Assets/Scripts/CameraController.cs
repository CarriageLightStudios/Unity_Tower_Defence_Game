using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float rotationSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.position += -(transform.right * 0.1f);
            }
            else if (Input.GetAxis("Mouse X") < 0)
            {
                transform.position += transform.right * 0.1f;
            }
        }
        if (Input.GetMouseButton(1))
        {
            transform.eulerAngles += rotationSpeed * new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0.0f);
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
