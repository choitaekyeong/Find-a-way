using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TutorialBoss : Enemy
{



    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        //capsulecoll = GetComponent<CapsuleCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
