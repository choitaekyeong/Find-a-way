using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MainBoss : Enemy
{
   // public GameObject fireBall;
   // public Transform magicBolt;//매직볼트
    //public Transform shootingRock;//돌 공격!


    public bool IsAttack_boss;
    public GameObject Rock;//돌공격
    public GameObject BindObj;//바인드공격
    public GameObject MagicBoltObj;
    Vector3 lookvec;
    public bool isLook;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        //capsulecoll = GetComponent<CapsuleCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        nav.isStopped = true;
      
        StartCoroutine(TarGet());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookvec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.transform.position + lookvec);
            //StartCoroutine(Think());
        }


    }

    IEnumerator TarGet()
    {
            targets.Clear();
             yield return new WaitForSeconds(0.2f);
            Debug.Log("체크중");
            //리스트값  초기화
            Collider[] colls = Physics.OverlapSphere(transform.position, 70.0f);//인식 범위 안에 있는 오브젝트들 인식
            for (int i = 0; i < colls.Length; i++)
            {
                if (this.gameObject.tag == "Enemy")//적군상태일때
                {
                    if (colls[i].gameObject.tag == "Player" || colls[i].gameObject.tag == "PlayerUnit")
                    {
                        target = colls[i].gameObject.transform;
                        targets.Add(target.transform.gameObject);
                    //isLook = true;
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
                float distanceTotarget = Vector3.Distance(found.transform.position, transform.position);
                // Debug.Log("distanceTotarget:" + distanceTotarget);
                if (distanceTotarget < foundDis)
                {
                    targeting = found;
                    foundDis = distanceTotarget;
                    // Debug.Log("distanceTotarget" + distanceTotarget);
                }

            }

        }


        if (targets.Count != 0)
            {
                Debug.Log("보스공격패턴시작해야지");
                isLook = true;
                //IsLookThat();
                StartCoroutine(Think());
                
            }

          


    }

    IEnumerator Think() //보스 무슨공격할지 생각중
    {
            if(target==null)//만약공격할려하는데 타켓이 죽었다.
            {
                Debug.Log("타겟찾아야지");
                StartCoroutine(TarGet());//그럼 새로운 타겟 찾아.
            }
            
            yield return new WaitForSeconds(0.2f);
            Debug.Log("생각중");
            int ranAction = Random.Range(0, 5);
            Debug.Log("몇번쟤패턴:" + ranAction);
            switch (ranAction)
            {
                case 0:
                case 1:
                    StartCoroutine(MagicBolt());
                    break;
                case 2:
                case 3:
                    StartCoroutine(ShootingRock());
                    break;
            case 4:
                StartCoroutine(Bind());
                break;

            }
        //}
    
       // IsAttack_boss = false;
    }
    IEnumerator MagicBolt()
    {
        IsAttack_boss = true;
        Debug.Log("매직볼트");
        animator.SetTrigger("doMagicBolt");
        //yield return new WaitForSeconds(1.0f);// 애니메이션 임시 속도
        GameObject InstantMagicBolt = Instantiate(MagicBoltObj,fireWayPoint.transform.position, transform.rotation);
        Rigidbody rigidBolt = InstantMagicBolt.GetComponent<Rigidbody>();
        rigidBolt.velocity = transform.forward * 10;
        IsAttack_boss = false;
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Think());
    }
    IEnumerator ShootingRock()
    {
        IsAttack_boss = true;
        // IsAttack_boss = true;
        Debug.Log("돌던지기");
        animator.SetTrigger("doRockShooting");
        //yield return new WaitForSeconds(1.0f);
        GameObject IstantRock = Instantiate(Rock, target.transform.position, target.transform.rotation);
        //Rigidbody istanRock = IstantRock.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(2.5f);
        //isLook =true;
        IsAttack_boss = false;
        StartCoroutine(Think());
    }
    IEnumerator Bind()
    {
        IsAttack_boss = true;
        Debug.Log("바인드");
        animator.SetTrigger("doMindControl");//바인드 스킬임 바꾸기귀찮아서 이렇게적어놈
        //yield return new WaitForSeconds(1.0f);// 애니메이션 임시 속도
        GameObject IstantBind = Instantiate(BindObj, target.transform.position, target.transform.rotation);
        yield return new WaitForSeconds(2.5f);
        IsAttack_boss = false;

        StartCoroutine(Think());

    }

}
