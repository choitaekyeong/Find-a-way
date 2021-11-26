using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class Enemy : MonoBehaviour
{
    public enum Type { O,B,W,M,G}
    public Type enemyType;
    

    public enum EnemyState
    {
        idle,
        move,
        chase,
        Attack,
        dead
    };
    public Camera cam;
    // Start is called before the first frame update
    public EnemyState currentstate = EnemyState.idle;
    public int maxHealth;//체력
    public int curHealth;//현재 체력
    public bool isAttack;//공격
    public bool isDead = false;//죽은지살았는지 판단
    public float moveSpeed = 2.0f;
    public Transform target;//타겟 위치
    public BoxCollider meleeArea; //무기 콜라이더
    public float chaseDis;//인식 범위
    public float attackDis = 1.5f;//공격범위
    public float foundDis;//인식범위    
    public GameObject Potal;
    public float rotationAnglePerSecond = 360f;//초당 회전각도
    public GameObject fire;// 원거리몬스터를위한 만든 fire
    public GameObject p_Fire;//플레이어 파이어
    public Rigidbody rigid;
    //CapsuleCollider capsulecoll;
    public Material mat;//색상
    private Color originalcolor;
    public NavMeshAgent nav;//내비메쉬 적찾기
    public Animator animator;//애니메이션
    //public Weapon Eweapon;//적군 무기
    public List<GameObject> targets = new List<GameObject>();//타겟게임오브젝트 넣을것들
    public SelectionManager selection;
    public GameObject targeting;//타켓
    public GameObject weapon;//무기
    public GameObject ProtectItem;//쉴드
    public GameObject fireWayPoint;//파이어볼위치
    public bool isMove;
    public Vector3 destination;
    AudioSource enemySound;//적군소리
    void Start()
    {
        selection = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        rigid = GetComponent<Rigidbody>();
        //capsulecoll = GetComponent<CapsuleCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        //originalcolor = mat.color;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemySound = GetComponent<AudioSource>();
        //meleeArea.enabled = false;
        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }
    void Update()
    {
        if (this.gameObject.tag == "Player" && IsaManager.instance.currentGameState == GameState.RTS &&enemyType !=Type.M)
        {
            MouseMove();
            SetDestnation();
            StartCoroutine(Check());


        }
        else if (enemyType != Type.M)
        SetDestnation();
        StartCoroutine(Check());
        StartCoroutine(StateAction());
    }
    public void FreezeVelocity()
    {
        if (!isDead)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    private void MouseMove()
    {
        GameObject selection = transform.Find("SelectionProjector(Clone)").gameObject;
        if (Input.GetMouseButtonDown(1)&&selection !=null)
        {
            Debug.Log("마우스 키입력받음");
            //레이캐스트를 활용한 마우스클릭이동
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Terrain")
                {
                    Debug.Log("땅 클릭했어요");

                    Moving(hit.point);

                    //animator.SetBool("Iswalk", true);
                }
              
            }
        }


    }
    void Moving(Vector3 dest)
    {
        currentstate = EnemyState.move;
        destination = dest;
       // Debug.Log(destination);
        isMove = true;
        animator.SetBool("Isrun", isMove);
    }
     void SetDestnation()
    {
        if(isMove)
        {
            animator.SetBool("Isrun", isMove);
            var dir = destination - transform.position;
            animator.transform.forward = dir;
            transform.position += dir.normalized * Time.deltaTime * moveSpeed;
           

        }
        if (Vector3.Distance(destination, transform.position) <= 0.3f)
        {
            isMove = false;
            animator.SetBool("Isrun", isMove);
        }

    }
  public void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(targeting.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationAnglePerSecond);
    }
   void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

    }
    void Ending()
    {
        SceneManager.LoadScene("Ending");
    }
   
    void FixedUpdate()
    {
            FreezeVelocity();
    }
    //OK 적용잘되고있어.
    void OnTriggerEnter(Collider other)
    {
        if(this.gameObject.tag=="Enemy"||this.gameObject.tag=="Boss")
        {//근접무기에 공격받을시..
            if (other.tag == "Melee")
            {
                Weapon weapon = other.GetComponent<Weapon>();
                curHealth -= weapon.damage;
                Debug.Log("현재 체력:" + curHealth);
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
                Debug.Log("Melee:" + curHealth);
            }
             if (other.tag == "Mind"&&enemyType != Type.M&&mat.color==Color.yellow&&enemyType != Type.B)
            {
                if (this.gameObject.tag == "Enemy")
                {
                    weapon.tag = "Melee";//웨폰태그 변경
                    this.gameObject.tag = "Player";//캐릭터 태그도 변경
                    curHealth += 1000;
                    fire = p_Fire;
                    targeting = null;
                   
                }
                //Transform tempobj = transform.Find("BigOrkWeapon");
                //tempobj.gameObject.tag = "Melee";
                mat.color = Color.green;
                Debug.Log("마인드 컨트롤 당했습니다");

            }
            if(other.tag =="Pfire")
            {
                Fire fire = other.GetComponent<Fire>();
                curHealth -= fire.dmage;
                Debug.Log("현재 체력:" + curHealth);
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
                Debug.Log("Melee:" + curHealth);
            }
            
        }
        if(this.gameObject.tag =="Player")
        {
            //캐릭터가 플이어태그이고 적군무기에 맞았을경우
            if (other.tag == "Emelee")
            {
                Weapon weapon = other.GetComponent<Weapon>();
                curHealth -= weapon.damage;
                Debug.Log("현재 체력:" + curHealth);
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
                Debug.Log("Melee:" + curHealth);
            }
            if(other.tag == "Fire")
            {
                Fire fire = other.GetComponent<Fire>();
                curHealth -= fire.dmage;
                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject,2.3f);
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
            }
        }

    }
    //상태 체크

    IEnumerator Check()
    {
      if(!isDead &&enemyType !=Type.M)
       {
           
            yield return new WaitForSeconds(0.2f);
            targets.Clear();//리스트값  초기화
            Collider[] colls = Physics.OverlapSphere(transform.position, 7.0f);//인식 범위 안에 있는 오브젝트들 인식
            for (int i = 0; i < colls.Length; i++)
            {
                if (this.gameObject.tag == "Enemy")//적군상태일때
                {
                    if (colls[i].gameObject.tag == "Player" || colls[i].gameObject.tag == "PlayerUnit")
                    {
                        target = colls[i].gameObject.transform;
                        targets.Add(target.transform.gameObject);
                    }
                }
                else if(this.gameObject.tag=="Player")
                {

                    if (colls[i].gameObject.tag == "Enemy")
                    {
                      //  Debug.Log("적발견");
                        target = colls[i].gameObject.transform;
                        targets.Add(target.transform.gameObject);
                    }

                }
                
            }

            if (targets.Count != 0)
            {
                //Debug.Log("인식범위 갯수" + targets.Count);
                targeting = targets[0];
               // Debug.Log("타겟" + targeting);
                foundDis = Vector3.Distance(targets[0].transform.position, transform.position);
              //  Debug.Log("발견거리" + foundDis);
                foreach (GameObject found in targets)
                {
                    float distanceTotarget = Vector3.Distance(found.transform.position,transform.position );
                   // Debug.Log("distanceTotarget:" + distanceTotarget);
                    if(distanceTotarget < foundDis)
                    {
                        targeting = found;
                        foundDis = distanceTotarget;
                       // Debug.Log("distanceTotarget" + distanceTotarget);
                    }
                   
                }

            }
          
           // Debug.Log(targets.Count);
           // Debug.Log(targeting.name);
            nav.stoppingDistance = attackDis;
            //Debug.Log(foundDis);
            if (foundDis <= attackDis && !isAttack&&targets.Count !=0) //
            {

                // Debug.Log("발견거리" + foundDis);
                // Debug.Log("공격범위 들어옴");
                //Debug.Log("공격" + foundDis);

                nav.stoppingDistance = attackDis;
                //Debug.Log("네비금지" + nav.stoppingDistance);
                // StartCoroutine(Attack());
                currentstate = EnemyState.Attack;

            }


            else if (foundDis <= chaseDis)
            {
                currentstate = EnemyState.chase;
            }
            else
                currentstate = EnemyState.idle;
            if (targets.Count == 0 && isMove == false)
            {
                currentstate = EnemyState.idle;
            }
            
        }

    }
 
   IEnumerator StateAction()
    {
       if(!isDead &&enemyType != Type.M)
        {
            switch (currentstate)
            {
                case EnemyState.idle:
                  
                    nav.isStopped = false;
                    this.nav.velocity = Vector3.zero;
                    //IdleState();
                   // animator.SetBool("Isrun", false);
                    break;
                case EnemyState.move:
                    animator.SetBool("Isrun", true);

                    break;
                case EnemyState.chase:
                    nav.destination = targeting.transform.position;
                    // ChaseState();
                    TurnToDestination();
                    animator.SetBool("Isrun", true);
                    break;
                case EnemyState.Attack:
                    StartCoroutine(Attack());
                    break;
                case EnemyState.dead:

                    break;

            }
            yield return null;
        }

    }
    
    //어택
   IEnumerator Attack()
    {

        switch (enemyType)
        {
            case Type.O: // 오크 공격
                nav.enabled = false;
                isAttack = true;
                animator.SetBool("IsAttack", true);
                enemySound.Play();
                yield return new WaitForSeconds(0.3f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(0.5f);
                meleeArea.enabled = false;
                yield return new WaitForSeconds(1.4f);

                break;
            case Type.W: // 위치 공격
                Debug.Log("파이어볼!");
                nav.enabled = false;
                isAttack = true;
                animator.SetBool("IsAttack", true);
                //enemySound.Play();
                yield return new WaitForSeconds(1.1f);
                GameObject InstantFire = Instantiate(fire,fireWayPoint.transform.position, transform.rotation);
                Rigidbody rigidFire = InstantFire.GetComponent<Rigidbody>();
                rigidFire.velocity = transform.forward * 20;
                yield return new WaitForSeconds(1.5f);
                break;
            case Type.B: // 네임드 오크 공격
                //nav.enabled = false;
                isAttack = true;
                animator.SetBool("IsAttack", true);
                //enemySound.Play();
                yield return new WaitForSeconds(0.3f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(0.9f);
                meleeArea.enabled = false;
                yield return new WaitForSeconds(1.0f);

                break;
            case Type.G:// 골렘 공격
                nav.enabled = false;
                isAttack = true;
                animator.SetBool("IsAttack", true);
                //enemySound.Play();
                yield return new WaitForSeconds(0.3f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(1.0f);
                meleeArea.enabled = false;
                yield return new WaitForSeconds(1.0f);
                break;

        }
       
        isAttack = false;
        nav.enabled = true;
        animator.SetBool("IsAttack", false);

       
    }
    // 히트데미지
    // Update is called once per frame
    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);


        Debug.Log("현재 체럭:" + curHealth);
        if (curHealth > 0&&enemyType !=Type.G)
        {
            mat.color = Color.white;
        }
         if (curHealth <= 2000&&gameObject.tag=="Enemy" && enemyType != Type.G)
        {
            mat.color = Color.yellow;

        }
        if (gameObject.tag == "Player")
        {
            mat.color = Color.green;
        }
        if(curHealth<=0)
        {

            StopCoroutine(Attack());
            
            currentstate = EnemyState.dead;
            this.gameObject.tag = "Dead";
            StopCoroutine(Check());
            isDead = true;
            mat.color = Color.grey;
            gameObject.layer = 11;
            nav.enabled = false;//네비비활성화
            animator.SetTrigger("IsDie");
            if(enemyType ==Type.B)
            {
                ProtectItem.SetActive(true);//임시 ui오브젝트
                Potal.SetActive(true); 
            }
            if(enemyType ==Type.G)
            {
                meleeArea.enabled = false;
                Destroy(ProtectItem, 1.5f);
            }
            if(this.gameObject.name=="WitchBoss"&&enemyType ==Type.W)
            {
                Potal.SetActive(true);//마녀약점 설명서

            }
            if (this.gameObject.name == "BigBoss_Orc_real" && enemyType == Type.O)
            {
                meleeArea.enabled = false;
                Potal.SetActive(true);//오크약점 설명서

            }
            if(enemyType ==Type.M)
            {
                Ending();
            }
            Destroy(gameObject, 2);
        }

    }

}
