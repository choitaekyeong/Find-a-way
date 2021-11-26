using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무엇을 찾고있는지 확인하는 클래스
public class Aspect : MonoBehaviour
{
   public enum aspect
    {
        Player,
        Enemy
    }
    public aspect aspectName;
}
