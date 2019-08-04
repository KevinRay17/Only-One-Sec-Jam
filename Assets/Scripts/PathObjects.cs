using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PathObjects : MonoBehaviour
{
    public static PathObjects instance;

    public bool boom = false;

    //public ParticleSystem shatter;

    public float intensity = 1;
    public bool gavePoints = false;
    
    
   

    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (boom)
        {
            StartCoroutine(BigBoom());
        }
    }

    IEnumerator BigBoom()
    {
        yield return new WaitForSeconds(1);
        //shatter.Emit(1);
        gameObject.GetComponent<Collider>().isTrigger = true;
        gameObject.tag = "OffPath";
        while (intensity > .2f)
        {
            intensity -= .02f;
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0,0, intensity, 1));
            if (Tilemap.instance.restarting)
            {
                break;
            }
            yield return 0;
        }
        boom = false;
    }
}
