using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
    private bool open = false;
    public GameObject rules;
    public void Press()
    {
        if (!open)
        {
            rules.SetActive(true);
            open = true;
        }
        else
        {
            rules.SetActive(false);
            open = false;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
