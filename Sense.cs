﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    public bool bDebug = true;
    public Aspect.aspect aspectName = Aspect.aspect.Enemy;
    public float detectionRate = 1.0f;

    protected float elapsedTime = 0.0f;

    protected virtual void Initialize() { }
    protected virtual void UpdateSense() { }


    void Start()
    {
        //초기화
        elapsedTime = 0.0f;
        Initialize();
    }
    void Update()
    {
        UpdateSense();
    }


}