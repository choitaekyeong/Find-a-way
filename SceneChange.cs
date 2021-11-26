using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{

    public void ChangeTutorial()
    {
       
        SceneManager.LoadScene("TutorialScene");
    }
    public void ChangeMainTitle()
    {
        SceneManager.LoadScene("DemoScene_01");
    }
    void NextMainStage()
    {
        SceneManager.LoadScene("MainStage1");
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NextMainStage();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }



}
