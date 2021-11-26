using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    public int dmage;
    public bool isFire;

     void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }
    }
     void OnTriggerEnter(Collider other)
    {
     if(!isFire &&other.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
            //transform.Rotate(Vector3.right * 30 * Time.deltaTime);
            Destroy(gameObject, 3);
       
    }
}
