using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TalkManager talkManager;
    public GameObject talkPanel;
    public Text talktext;// 텍스트
    public GameObject scanObject;//스캔할 오브젝트
    public static bool isAction;
    public int talkIndex;



    void Start()
    {
        DontDestroyOnLoad(gameObject);
       
    }


    public void Action(GameObject scanObj)
    {

            scanObject = scanObj;
            ObjData objData = scanObject.GetComponent<ObjData>();
            Talk(objData.id, objData.isObject);

        talkPanel.SetActive(isAction);

    }
    void Talk(int id, bool isObject)
    {

        string talkData = talkManager.GetTalk(id, talkIndex);


        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }
        if (isObject)
        {
            talktext.text = talkData;

        }
        else
        {
            talktext.text = talkData;
        }
        isAction = true;
        talkIndex++;

    }

}
