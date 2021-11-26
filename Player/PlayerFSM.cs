using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;

public class PlayerFSM : MonoBehaviour
{
    // Start is called before the first frame update
    public enum State
    {
        Idle,
        Move,
        Attack,
        AttackIdle,
        Dead

    }
    GameObject CurEnemy;
    public State currentState = State.Idle;// 기본 상태 idle로 처리
    public float attackDelay=2f;
    public float attacktimer = 0f;
    PlayerAni myAni;
    void Start()
    {
        myAni = GetComponent<PlayerAni>();

        ChangeState(State.Idle, PlayerAni.ANI_ATKIDLE);
    }
    void ChangeState(State newState, int aniNumber)
    {
        if (currentState == newState)
        {
            return;
        }
        myAni.ChangeAni(aniNumber);
        currentState = newState;

    }


    public void AttackEnemy(GameObject Enemy)
    {
        if(CurEnemy!=null&&CurEnemy==Enemy)
        {
            return;
        }
        CurEnemy = Enemy;
      

    }
    void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Move:
                break;
            case State.Attack:
                AttackState();
                break;
            case State.AttackIdle:
                break;
            case State.Dead:
                break;
            default:
                break;


        }

    }
    //WALK스테이트
    public void MoveTo()
    {
        CurEnemy = null;
        ChangeState(State.Move, PlayerAni.ANI_WALK);


    }
    //공격스테이트및 애니메이션
    public void AttackState()
    {

        ChangeState(State.Attack, PlayerAni.ANI_ATTACK);


    }
    // Update is called once per frame
    void Update()
    {
            UpdateState();
    }
}
