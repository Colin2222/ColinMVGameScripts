﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [System.NonSerialized]
    public Transform followTransform;
    public float verticalOffset;

    void Start()
    {
        followTransform = GameObject.FindWithTag("PlayerTag").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y + verticalOffset, this.transform.position.z);
    }
}
