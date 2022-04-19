using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public int health;
    public int entranceNumber;
    public int numGems;
    public string [] inventory;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
