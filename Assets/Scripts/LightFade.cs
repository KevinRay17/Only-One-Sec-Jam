using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//put this script on a light source in the game scene, when the "Start" button is pressed,
//the game scene opens with light then the scene fades to black

public class LightFade : MonoBehaviour
{
    public static LightFade instance;
    public Light Lt;

    public GameObject SpotLight; //game object (light source) this script is attached to

    void Start()
    {
        instance = this;
        StartCoroutine(Fade());
       // float timeUntilInvoked = 1.0f; //seconds spent in light (seconds before invoke)
        //Invoke("TriggerFunction", timeUntilInvoked); //invoke function after 1 second
    }

    //void TriggerFunction()
    //{
      //  StartCoroutine(Fade()); //coroutine runs independent of update
    //}

    public IEnumerator Fade()
    { 
        yield return new WaitForSeconds(.5f);
        
        float duration = 1.0f; //how long the fade to black is
        float interval = 0.1f; //interval time between iterations of while loop
        Lt.intensity = 1.0f; //scene is lit
        while(Lt.intensity >0)
        {
            Lt.intensity -= 0.05f; //so every .1 second the scene fades to black by a factor of .2 (I THINK)
            yield return new WaitForSeconds(.01f);
            // duration -= interval;
            //yield return new WaitForSeconds(interval); //coroutine will wait for 0.1 seconds
        }
    }
    public IEnumerator FadeIn()
    { 
        yield return new WaitForSeconds(.5f);
        
        float duration = 1.0f; //how long the fade to black is
        float interval = 0.1f; //interval time between iterations of while loop
        Lt.intensity =0f; //scene is lit
        while(Lt.intensity <1)
        {
            Lt.intensity += 0.05f; //so every .1 second the scene fades to black by a factor of .2 (I THINK)
            yield return new WaitForSeconds(.01f);
            // duration -= interval;
            //yield return new WaitForSeconds(interval); //coroutine will wait for 0.1 seconds
        }

        StartCoroutine(Fade());
    }
}