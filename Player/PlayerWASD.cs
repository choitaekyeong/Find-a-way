using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWASD : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 5.0f;
    float h, v = 0;
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {

        body = GetComponent<Rigidbody>();
        // 중력해제
        body.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // 즉시 회전
            //body.rotation = Quaternion.Euler(0, angle, 0);

            // 부드러운 회전 
              body.rotation = Quaternion.Slerp(body.rotation, Quaternion.Euler(0, angle, 0), rotationSpeed * Time.fixedDeltaTime);      
        }

        body.position = direction * moveSpeed * Time.fixedDeltaTime;
    }
}
