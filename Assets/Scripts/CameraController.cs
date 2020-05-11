using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float verScreenPart = 0.9f;
    float horScreenPart = 0.9f;

    public float speed = 3f;

    Vector3 theory;

    public Transform oriental;

    // Start is called before the first frame update
    void Start()
    {
        theory = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        oriental.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        if (Input.mousePosition.x / Screen.width < 1 && Input.mousePosition.x / Screen.width > 0 && Input.mousePosition.y / Screen.height < 1 && Input.mousePosition.y / Screen.height > 0)
        {
            if (Input.mousePosition.x / Screen.width > horScreenPart)
            {
                theory += speed * oriental.right * Time.deltaTime;
            }

            if (Input.mousePosition.x / Screen.width < 1 - horScreenPart)
            {
                theory -= speed *  oriental.right * Time.deltaTime;
            }

            if (Input.mousePosition.y / Screen.height > verScreenPart)
            {
                theory += speed *  oriental.forward * Time.deltaTime;
            }

            if (Input.mousePosition.y / Screen.height < 1 - verScreenPart)
            {
                theory -= speed * oriental.forward * Time.deltaTime;
            }
        }

        transform.position = Vector3.Lerp(transform.position, theory, 0.3f);
    }
}
