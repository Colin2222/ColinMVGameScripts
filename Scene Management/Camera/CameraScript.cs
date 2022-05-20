using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [System.NonSerialized]
    public Transform followTransform;
    public float verticalOffset;
    public Camera camera;
    //public CinemachineVirtualCamera cCamera;

    void Start()
    {
        ResetFollowTransform();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y + verticalOffset, this.transform.position.z);
    }

    public void ResetFollowTransform(){
        followTransform = GameObject.FindWithTag("PlayerTag").transform;
    }
}
