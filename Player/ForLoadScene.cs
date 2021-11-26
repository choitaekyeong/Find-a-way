using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForLoadScene : MonoBehaviour
{

    private static ForLoadScene f_Instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        if(f_Instance)
        {
            DestroyImmediate(this.gameObject);
        }

        f_Instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

   
}
