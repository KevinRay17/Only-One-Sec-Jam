using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tvLight_Flux : MonoBehaviour
{
    public float maxTimer = 0f;
    private float curTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        curTimer = maxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        curTimer -= Time.deltaTime;

        if (curTimer <= 0)
        {
            curTimer = maxTimer;
            this.GetComponent<Light>().intensity = Random.Range(1.55f, 1.75f);
        }
    }
}
