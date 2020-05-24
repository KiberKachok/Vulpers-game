using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAligner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.down);
        transform.LookAt(new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
    }
}
