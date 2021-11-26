using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject boss;
    public GameObject protect1;
    public GameObject protect2;

    // Start is called before the first frame update
    void Start()
    {
        boss.GetComponent<MainBoss>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(protect1 ==null&&protect2 ==null)
        {
            boss.gameObject.tag = "Enemy";
            boss.GetComponent<MainBoss>().enabled = true;
            Destroy(this.gameObject, 2f);


        }


    }
}
