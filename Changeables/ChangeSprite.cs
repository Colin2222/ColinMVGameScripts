using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public string savedName;
    public bool mustBeTrue;
    public SpriteRenderer spriteRenderer;
    public Sprite alternateSprite;

    [System.NonSerialized]
    public SceneController sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneController>();

        if((sceneManager.dataManager.GetData(savedName) && mustBeTrue) ||
            (!sceneManager.dataManager.GetData(savedName) && !mustBeTrue)){
            spriteRenderer.sprite = alternateSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
