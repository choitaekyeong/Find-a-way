using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Chase,
        Attack,
        Move,
        Dead

    };
    public GameManager gameManager;//대화창 게임 매니저
    public IsaManager manager;//RPG,RTS모드 매니저
    public PlayerState playerState;
    //이동
    public float speed= 4.0f;//이동속도
    public float rotateSpeed = 360.0f;//360도 회전
    Vector3 curTargetPos;//마우스 클릭한 좌표
    public Camera cam;
    GameObject curenemy;//적군클릭
    public GameObject[] weapons;//무기 배열
    public bool[] hasWeapons;//가지고있는무기
    public int MaxHp; //최대체력
    public int CurHp;//현재체력
    float hAxis;//상하
    float vAxis;//좌우
    float fireDelay;//공격 딜레이
    float attackDistance=1.5f;//공격 사거리
    float chasedistance;//추적
    int equipWeaponIndex = -1; //초기화
    
    bool rpgModeIsMove;// 이동중
    bool dDown;//구르기
    bool fDown;//공격키
    bool iDown;//상호작용키
    bool sDown1;//장비 1번(검)
    bool sDown2;//장비 2번(원거리무기)
    bool sDown3;//장비 3번(마인드컨트롤 아이템)
    bool isFireReady=true;//공격준비
    bool isSwap;//무기교체 시간차
    bool isDodge;// 구르기
    bool isdamage;//피격후 무적판정
    bool gDown;//ui키
    
    public bool ismove;
    Vector2 moveVec;//캐릭터 좌표
    Vector3 dodgeVec;//회피도중 방향전환이 되지 않도록
    private SkinnedMeshRenderer playerRenderer;
    private Color originalPlayerColor;
    GameObject nearObject;//근처에있는 오브젝트
    GameObject scanObject;//스캔할 오브젝트
    GameObject blood;//피격시 피 이펙트
   
    Rigidbody playerRigid;
    Animator animator;
    //GameObject equipWeapon;
    Weapon equipWeapon;
   void ChangeState(PlayerState newState)
    {
        if(playerState==newState)
        {
            return;

        }
        playerState = newState;

    }
    // Start is called before the first frame update
    void Start()
    {
        blood = transform.Find("PlayerBlood").gameObject;
        playerRigid = GetComponent<Rigidbody>();
        //animator = GetComponentInChildren<Animator>();
        playerRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originalPlayerColor = playerRenderer.material.color;
        animator = GetComponent<Animator>();
        //ChangeState(PlayerState.Idle);

    }
    void Update()
    {
        if(IsaManager.instance.currentGameState == GameState.RPG)
        {
            GetInput();
            Interation();
            Dodge();
            Swap();
            Getout();
            
        }
        if (IsaManager.instance.currentGameState == GameState.RTS)
        {
             MouseMove();
            MouseMoveset();
        }
        //Turn();
        Attack();
        if(Input.GetKeyDown(KeyCode.P))//보스앞
        {
            transform.position = new Vector3(215, 20, 97);
        }
        if (Input.GetKeyDown(KeyCode.O))//보스앞
        {
            transform.position = new Vector3(173, 20, 233);
        }
        if (Input.GetKeyDown(KeyCode.L))//반대쪽 성상
        {
            transform.position = new Vector3(216, 20, 261);
        }

    }
    void PlayerStates()
    {
        switch(playerState)
        {
            case PlayerState.Idle:  
                break;
            case PlayerState.Move:
                break;
            case PlayerState.Attack:
               // transform.LookAt(curTargetPos);
                Attack();
                break;
            case PlayerState.Chase:
                break;
        }
    }
    void FixedUpdate()
    {
        FreezeRotation();
    }
    void FreezeRotation()
    {
        playerRigid.velocity = Vector3.zero;
        playerRigid.angularVelocity = Vector3.zero;

    }
    //캐릭터 입력키받기
     void GetInput()
    {
        //hAxis = Input.GetAxis("Horizontal");//상하
        //vAxis = Input.GetAxis("Vertical");//좌우
       // dDown = Input.GetButtonDown("Jump");//구르기
        fDown = Input.GetButton("Fire1");//공격
        iDown = Input.GetButtonDown("Interation");//상호작용 e키
        sDown1 = Input.GetButtonDown("Swap1");//1번키
        sDown2 = Input.GetButtonDown("Swap2");//2번키
        sDown3 = Input.GetButtonDown("Swap3");//3번키
        gDown = Input.GetButtonDown("Getout");//대화 넘기기
    }
    //이동

    public void MouseMove()
    {
        //GameObject selection = transform.Find("SelectionProjector(Clone)").gameObject;
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("마우스 키입력받음");
            //레이캐스트를 활용한 마우스클릭이동
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Terrain")
                {
                    Debug.Log("땅 클릭했어요");

                    MoveTo(hit.point);
                   
                }
                else if (hit.collider.gameObject.tag == "Enemy")
                {
                    AttackEnemy(hit.collider.gameObject);
                    Debug.Log("적 클릭");

                }
            }
        }   


    }

    //public void Movestate()
    //{
    //    MouseMoveset();
    //    //TurnToDestination();
    //    //MoveToDestination(); 
    //}
    //회전
    public void MoveTo(Vector3 tPos)
    {
        curenemy = null;
        curTargetPos = tPos;
        ismove = true;
        animator.SetBool("IsWalk", ismove);
        animator.SetBool("IsWalk", rpgModeIsMove);
        //ChangeState(PlayerState.Move);
    }
    private void MouseMoveset()
    {
        if(ismove)
        {
            animator.SetBool("IsWalk", ismove);
            var dir = curTargetPos - transform.position;
            animator.transform.forward = dir;
            transform.position += dir.normalized * Time.deltaTime * speed;
            

        }
        if(Vector3.Distance(transform.position,curTargetPos) <=0.3f)
        {
            ismove = false;
            animator.SetBool("IsWalk", ismove);
        }

    }
    void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(curTargetPos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

    }
    void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, curTargetPos, speed * Time.deltaTime);
        if (curenemy = null)
        {
            //클릭위치랑 내위치가 같으면
            if (transform.position == curTargetPos)
            {
                ChangeState(PlayerState.Idle);
           
            }
        }
        else if(Vector3.Distance(transform.position,curTargetPos)<attackDistance)
        {
            Debug.Log("공격 사거리안에 들었음");
            ChangeState(PlayerState.Attack);
        }

    }
    
    //캐릭터회전
    void Turn()
    {
        //transform.LookAt(transform.position + moveVec);
    }
    //공격
    public void AttackEnemy(GameObject enemy)
    {

        if(curenemy !=null&& curenemy==enemy)
        {
            return;
        }
        curenemy = enemy;
        curTargetPos = curenemy.transform.position;
        ChangeState(PlayerState.Move);

    }
    void Attack()
    {
 
        if (equipWeapon == null)//현재장비장착했을경우만
            return;
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
         
        if(fDown&&isFireReady&&!isDodge&& !isSwap&& !rpgModeIsMove)
        {

            Debug.Log("공격중");
            equipWeapon.Use();
            animator.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "IsMeleeAttack" :"IsMindControll");
          
            fireDelay = 0;
        }
       

    }
    //구르기
    void Dodge()
    {
        if (dDown&&!rpgModeIsMove&&!isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            animator.SetTrigger("IsDodge");
            isDodge = true;

            Invoke("DodgeOut", 2.0f);
        }

    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;

    }
    // Update is called once per frame

     void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")//이태그가 웨폰일때
            nearObject = other.gameObject;//저장
        //Debug.Log(nearObject.name);
       


    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")//이태그가 웨폰일때
            nearObject = null;

        if (other.tag == "Obstacle")
            scanObject = null;

    }
  void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Emelee")//적군무기일때
        {
            if (!isdamage)
            {
                Weapon weapon = other.GetComponent<Weapon>();
                CurHp -= weapon.damage;
                if (CurHp <= 0)
                {
                    gameObject.layer = 14;
                    animator.SetTrigger("IsDying");
                    Destroy(gameObject,2);
                }
                StartCoroutine(OnDamaged());
                Debug.Log("Emelee:" + CurHp);
                
            }
        }
        else if (other.tag == "Fire")
        {
            if(!isdamage)
            {
                Fire fire = other.GetComponent<Fire>();
                CurHp -= fire.dmage;
                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);
                StartCoroutine(OnDamaged());

            }

        }
        if (other.tag == "Obstacle")
        {//장애물일때
            scanObject = other.gameObject;
            Debug.Log("스캔오브젝트:" + scanObject.name);
            gameManager.Action(scanObject);
            
        }
        
        


    }
    IEnumerator OnDamaged()
    {
        isdamage = true;
        if(isdamage==true)
        {
            playerRenderer.material.color = Color.red;
            blood.SetActive(true);
        }
        yield return new WaitForSeconds(1.0f);// 무적판정

        isdamage = false;
        if(isdamage==false)
        {
            playerRenderer.material.color = originalPlayerColor;
            blood.SetActive(false);
        }
            


    }
    //장비스왑
    void Swap()
    {
        int weaponIndex = -1;//초기화
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if((sDown1 || sDown2 || sDown3) && !isDodge)
        {
            if(equipWeapon!=null)//아닐때

            equipWeapon.gameObject.SetActive(false);//무기가 있을때
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            
            equipWeapon.gameObject.SetActive(true);

            //무기 스왑 애니메이션
            //animator.SetTrigger("IsSwap");//
            isSwap = true;

            Invoke("SwapOut", 0.5f);
        }
       
        //if ((!hasWeapons[1] || equipWeaponIndex == 1))
        //    return;
        //if ((!hasWeapons[2] || equipWeaponIndex == 2))
        //    return;

    }
    //스왑딜레이
    void SwapOut()
    {
        isSwap = false;
    }
    //상호작용
    void Interation()
    {
        if (iDown && nearObject != null && !isDodge)
        {
            if(nearObject.tag=="Weapon")
            {
                Item item = nearObject.GetComponent<Item>();//아이템이 먼지
                int weaponIndex = item.value;//
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
      
    }
    void Getout()//대화창 넘기기키
    { 
        if(gDown&&scanObject !=null)
        {
            gameManager.Action(scanObject);
        }
    }


    
}
