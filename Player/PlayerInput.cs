using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//캐릭터 WASD 이동
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;

    public IsaManager manager;
    GameManager Gmanager;//이동하지못하게
    public float speed = 4.0f;//이동속도
    public float rotateSpeed = 360.0f;//360도 회전
    public Camera cam;
    public GameObject[] weapons;//무기 배열
    public bool[] hasWeapons;//가지고있는무기
    public int MaxHp; //최대체력
    public int CurHp;//현재체력
    float hAxis;//상하
    float vAxis;//좌우
  

    bool rpgModeIsMove;// 이동중
   
    Vector2 moveVec;//캐릭터 좌표
    Vector3 dodgeVec;//회피도중 방향전환이 되지 않도록

    Animator animator;//플레이어 애니메이터
   

    void Awake()
    {
        animator = characterBody.GetComponent<Animator>();
       // floorMask = LayerMask.GetMask("Floor");
    }

    // Update is called once per frame
   void Update()
    {
        if (IsaManager.instance.currentGameState == GameState.RPG)
        {
                GetInput();
                MoveLootat();
                Move();
       

        }
        if (IsaManager.instance.currentGameState == GameState.RTS)
        {
           cameraArm.transform.position = characterBody.transform.position;
            

        }
       // GetInput();//입력함수
       
    }
    void GetInput()
    {
 
            hAxis = Input.GetAxisRaw("Horizontal");//좌우
            vAxis = Input.GetAxisRaw("Vertical");//상하
        
    }
    //wasd함수
    public void Move()
    {


        moveVec = new Vector2(hAxis, vAxis);
    
        rpgModeIsMove = moveVec.magnitude != 0;

        animator.SetBool("IsWalk", rpgModeIsMove);

      
            if (rpgModeIsMove)
            {

            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;

            Vector3 moveDir = lookForward * moveVec.y + lookRight * moveVec.x;

            characterBody.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * speed;


            }
        //moveVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
        //if (isDodge)//예외
        //    moveVec = dodgeVec;
        //if (isSwap||!isFireReady)//예외
        //    moveVec = Vector3.zero;


    }
    public void MoveLootat()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);


        }
        else
        {
            x = Mathf.Clamp(x, 355f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);



    }
 

}
