using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public CharacterHitbox characterHitbox;
    public PlayerPhysicsChecker physicsChecker;
    public PlayerHealth playerHealth;
    public Inventory inventory;
    public PlayerMover characterMover;
    public AudioProfile audio;
    public DoorChecker doorChecker;
    public ItemChecker itemChecker;
    public UtilityChecker utilityChecker;
    public InventoryManager inventoryManager;
    public TeleballLauncherScript launcher;

    public float spawnTime;
    private float spawnTimeCounter;
    [System.NonSerialized]
    public bool isSpawning;

    [System.NonSerialized]
    public SceneController sceneManager;

    [System.NonSerialized]
    public Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        sceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneController>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        resetSpawning();
    }

    // Update is called once per frame
    void Update()
    {
        // spawning timing
        if(spawnTimeCounter >= 0)
        {
            spawnTimeCounter -= Time.deltaTime;
            if(spawnTimeCounter <= 0)
            {
                isSpawning = false;
            }
        }
    }

    public Vector3 getPos(){
        return this.gameObject.transform.position;
    }

    public void resetSpawning(){
        isSpawning = true;
        spawnTimeCounter = spawnTime;
    }
}
