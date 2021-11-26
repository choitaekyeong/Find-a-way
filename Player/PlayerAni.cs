using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    // Start is called before the first frame update

    public const int ANI_IDLE = 0;//애니메이터 컨트롤러에 있는 숫자
    public const int ANI_WALK = 1;
    public const int ANI_ATTACK = 2;
    public const int ANI_ATKIDLE = 3;
    public const int ANI_DIE = 4;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void ChangeAni(int aninumber)
    {

        anim.SetInteger("aniname", aninumber);

    }
    // Update is called once per frame
    void Update()
    {
        

    }
}
