using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScPageManager : MonoBehaviour {

    public GameObject selectCheckBtn;
    public Image checkImg;

    private float selectBtnResetTime = 0.0f;
    public float SelectBtnResetTime
    {
        set
        {
            selectBtnResetTime = value;
        }
        get
        {
            return selectBtnResetTime;
        }
    }


    //싱글턴 패턴을 위한 인스턴스 변수 선언
    private static ScPageManager instance = null;

    public static ScPageManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gObj = new GameObject("_ScPageManager");
                instance = gObj.AddComponent<ScPageManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        //ScPageManager 클래스를 인스턴스에 대입
        instance = this;
    }

    private void Update()
    {
        //확인 버튼의 시간을 체크한다.
        if (selectBtnResetTime > 0.0f) SelectBtntimeCheck();
    }

    //확인 버튼의 시간이 0.0 초과라면 실행된다. 
    private void SelectBtntimeCheck()
    {
        selectBtnResetTime -= Time.deltaTime;
        //시간이 0초 이하로 내려가면 보정 후 비활성화 시킨다.
        if(selectBtnResetTime <= 0.0f)
        {
            selectBtnResetTime = 0.0f;
            selectCheckBtn.SetActive(false);
            checkImg.enabled = false;
        }
    } 


}
