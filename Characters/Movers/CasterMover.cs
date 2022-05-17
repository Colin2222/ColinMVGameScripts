using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterMover : MonoBehaviour
{
    public CharacterScript characterScript;

    [System.NonSerialized]
    public bool isHit = false;
    [System.NonSerialized]
    public bool isCasting = false;
    [System.NonSerialized]
    public bool isWinding = false;
    [System.NonSerialized]
    public bool isWaiting = true;
    [System.NonSerialized]
    public bool isCooling = false;
    [System.NonSerialized]
    public bool isAware = false;
    [System.NonSerialized]
    public bool isRolling = false;

    public float projectileSpeed;
    public float projectileWaveHeight;
    public float castTime;
    public float windupTime;
    public float cooldownTime;

    private float castTimer;
    private float windupTimer;
    private float cooldownTimer;

    [System.NonSerialized]
    public GameObject target;
    private float targetDistance = 0.0f;

    Rigidbody2D rigidbody2d;
    Animator animator;

    [System.NonSerialized]
    public int direction = -1;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = characterScript.gameObject.GetComponent<Rigidbody2D>();
        animator = characterScript.gameObject.GetComponent<Animator>();
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

        // update to face the target if not currently hit, lunging, or winding up for lunge
        if(!isHit && !isCasting && !isWinding && target != null){
            if((direction == 1 && transform.root.position.x > target.transform.root.position.x) || (direction == -1 && transform.root.position.x < target.transform.root.position.x)){
                changeDirection();
            }
        }
    }

    void FixedUpdate(){
        if(isCasting && !characterScript.isHurt){
            rigidbody2d.velocity = new Vector2(direction * projectileSpeed, rigidbody2d.velocity.y);
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
