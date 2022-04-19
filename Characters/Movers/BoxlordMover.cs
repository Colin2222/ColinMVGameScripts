using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxlordMover : MonoBehaviour
{
    /*
    STATE INFO
    0: idle
    1: move to target
    2: follow target
    */
    int currentState = 0;

    [System.NonSerialized]
    public GameObject target;
    private float targetDistance = 0.0f;

    [System.NonSerialized]
    public int direction = 1;

    public CharacterScript characterScript;
    Rigidbody2D rigidbody2d;
    Animator animator;
    [System.NonSerialized]
    public SceneController sceneManager;

    public float minFollowDistance;
    public float walkSpeed;
    public float runSpeed;

    public CharacterTimeCue[] cues;
    public int currentCue = 1;
    private int nextCueTime;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneController>();

        rigidbody2d = characterScript.gameObject.GetComponent<Rigidbody2D>();
        animator = characterScript.gameObject.GetComponent<Animator>();

        if(currentCue == cues.Length - 1){
            nextCueTime = cues[0].time;
        } else{
            nextCueTime = cues[currentCue + 1].time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update target if it has changed (detected from the player detector)
        if(characterScript.targetUpdated){
            characterScript.targetUpdated = false;
            target = characterScript.target;
        }

        // update distance between this and target
        if(target != null){
            targetDistance = Vector3.Distance(transform.root.position, target.transform.root.position);
        }

        // update state based on time
        handleTime();

        if(currentState == 0){

        }
        else if(currentState == 1){
            handleState_moveToTarget();
        }
        else if(currentState == 2){
            handleState_followTarget();
        }
    }

    public void updateState(int input){
        currentState = input;
    }

    void handleState_moveToTarget(){

    }

    void handleState_followTarget(){
        if(target != null){
            if((direction == 1 && transform.root.position.x > target.transform.root.position.x) || (direction == -1 && transform.root.position.x < target.transform.root.position.x)){
                changeDirection();
            }
            rigidbody2d.velocity = new Vector2(Mathf.Clamp(Mathf.Pow(targetDistance - minFollowDistance, 2.0f), 0.0f, walkSpeed) * direction, rigidbody2d.velocity.y);
        }
    }

    void handleTime(){
        if(Mathf.FloorToInt(sceneManager.dataManager.currentTime) == nextCueTime){
            currentCue += 1;
            if(currentCue == cues.Length - 1){
                currentState = cues[currentCue].state;
                nextCueTime = cues[0].time;
            } else if (currentCue == cues.Length){
                currentCue = 0;
                currentState = cues[currentCue].state;
                nextCueTime = cues[currentCue + 1].time;
            } else{
                currentState = cues[currentCue].state;
                nextCueTime = cues[currentCue + 1].time;
            }
        }
    }

    public void changeDirection()
    {
        direction = direction * -1;
        Vector3 newScale = characterScript.transform.localScale;
        newScale.x *= -1;
        characterScript.transform.localScale = newScale;
    }


}
