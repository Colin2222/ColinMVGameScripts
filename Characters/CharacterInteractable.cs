using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractable : MonoBehaviour
{
    public CharacterScript characterScript;

    public void Interact(PlayerMover player)
    {
        characterScript.interactions.Interact(player);
    }
}
