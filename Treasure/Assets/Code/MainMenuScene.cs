using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuScene : MonoBehaviour
{
    // Start is called before the first frame update
    public Text CreditsText;
    public Text ReturnText;
    //
    public Camera Camera_1;
    public Camera Camera_2;
    //
    public bool isCreditShot = false;

    void Start()
    {
        CreditsText.gameObject.SetActive(true);
        ReturnText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //
    public void StartGame(){
        Debug.Log("StartGame");
    }
    public void onClickCredits(){
        if(isCreditShot){
            isCreditShot = false;
            Camera_2.gameObject.SetActive(false);
            Camera_1.gameObject.SetActive(true);
            //
            CreditsText.gameObject.SetActive(true);
            ReturnText.gameObject.SetActive(false);
        }else{
            isCreditShot = true;
            Camera_1.gameObject.SetActive(false);
            Camera_2.gameObject.SetActive(true);
            //
            CreditsText.gameObject.SetActive(false);
            ReturnText.gameObject.SetActive(true);
        }
    }
}
