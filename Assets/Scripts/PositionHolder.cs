using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHolder : MonoBehaviour
{
    public int mod = 0;

    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position =  new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z - mod);
    }
}
