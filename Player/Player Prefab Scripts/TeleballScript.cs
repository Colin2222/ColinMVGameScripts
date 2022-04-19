using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleballScript : MonoBehaviour
{
    public int maxBounces;
    [System.NonSerialized]
    public int numBounces;

    // Start is called before the first frame update
    void Start()
    {
        numBounces = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        numBounces += 1;
        if(numBounces > maxBounces){
            Destroy(gameObject);
        }
    }
}
