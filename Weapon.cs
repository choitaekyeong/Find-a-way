using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    //공격타입
    public enum Type
    {
        Emelee,//적군 근접무기
        Melee,//아군 근접무기
        Range,//원거리 무기 
        Mind//마인드컨트롤
    }

    public Type type;
    public int damage;//데미지
    public float rate;//범위
    public BoxCollider meleeArea;//충돌
    public GameObject effect;//이펙트(아직 미구현 10.27)
    public BoxCollider MindArea;// 마인드 컨트롤
    AudioSource WeaponSound;

     void Start()
    {
        WeaponSound = GetComponent<AudioSource>();  
    }

    public void Use()
    {
        if (type == Type.Melee)//밀리일때
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type ==Type.Emelee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type ==Type.Mind)
        {
            StopCoroutine("MindControll");
            StartCoroutine("MindControll");

        }

    }
    IEnumerator Swing()
    {
        //0.1f만큼 쉬고 콜라이더 활성
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        effect.SetActive(true);
        WeaponSound.Play();
        //여기에다 이펙트
        yield return new WaitForSeconds(0.4f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = false;
        effect.SetActive(false);
        //yield은 결과를 전달하는 키워드


    }
    IEnumerator MindControll()
    {
        Debug.Log("마인드컨트롤 무기 사용");
        //0.1f만큼 쉬고 콜라이더 활성
        yield return new WaitForSeconds(0.1f);
        MindArea.enabled = true;
        //여기에다 이펙트
        yield return new WaitForSeconds(0.3f);
        MindArea.enabled = false;
        yield return new WaitForSeconds(0.3f);
        MindArea.enabled = false;

     

    }
 
}
