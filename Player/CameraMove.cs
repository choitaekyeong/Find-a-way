using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;// 타겟 위치
    public float targetY;

    public float xRotMax;
    public float rotSpeed;// 회전속도
    public float scrollSpeed;//스크롤 속도

    public float distance;//거리
    public float minDistance;//최소거리
    public float maxDistance;//최대거리

    private float xRot; //x방향 회전값  
    private float yRot;//y회전값
    private Vector3 targetPos; //
    private Vector3 dir;

     void Update()
    {
        Rotation();
        transform.LookAt(targetPos);
    }
    void Rotation()
    {
        
            xRot += Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime; //x축 회전속
            yRot += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;//
            distance += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

            xRot = Mathf.Clamp(xRot, -40, xRotMax);
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            targetPos = target.position + Vector3.up * targetY;
            dir = Quaternion.Euler(-xRot, yRot, 0f) * Vector3.forward;
            transform.position = targetPos + dir * -distance;
        
    }
    //private void LateUpdate()
    //{
    //    transform.LookAt(targetPos);
    //}
}
