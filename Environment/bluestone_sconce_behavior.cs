using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class bluestone_sconce_behavior : MonoBehaviour
{
    public Light2D light;

    public float minChangeTime;
    public float maxChangeTime;

    public float minIntensity;
    public float maxIntensity;

    private float currentIntensity;
    private float currentChangeTime;

    private float changeTimer;

    // Start is called before the first frame update
    void Start()
    {
        resetRandoms();
    }

    // Update is called once per frame
    void Update()
    {
        changeTimer += Time.deltaTime;

        if(changeTimer > currentChangeTime){
            resetRandoms();
        }
    }

    private void resetRandoms(){
        changeTimer = 0.0f;
        currentChangeTime = Random.Range(minChangeTime, maxChangeTime);
        currentIntensity = Random.Range(minIntensity, maxIntensity);
        light.intensity = currentIntensity;
    }
}
