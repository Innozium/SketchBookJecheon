using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bgImgBtnCtrl : MonoBehaviour {

    private Transform tr;

    private Button button;
    private Sprite baseSprite;

    //private BGCtrl bgCtrl;

    private Transform checkImgTr;
    private Image checkImg;

    public GameObject selectCheckBtnObj;

    void Awake()
    {
        //Transform 정보를 저장한다.
        tr = GetComponent<Transform>();

        //배경을 담당할 오브젝트의 BGCtrl를 가져온다.
        //bgCtrl = GameObject.FindGameObjectWithTag("BG").GetComponent<BGCtrl>();

        //버튼 컴포넌트를 넣어두자!
        button = GetComponent<Button>();

        //버튼 이벤트 동적 할당
        button.GetComponent<Button>().onClick.AddListener(delegate { OnBgImgButton(); });
       
        //체크표시할 이미지를 저장한다.
        checkImgTr = GameObject.Find("CheckImg").GetComponent<Transform>();
        checkImg = checkImgTr.GetComponent<Image>();
    }

    //버튼에 표시될 이미지와 클릭시 저장될 Image의 정보를 저장!
    public void BgImgBtnSetup(Sprite baseSprite)
    {
        this.baseSprite = baseSprite;
        GetComponent<Image>().sprite = baseSprite;
    }

    //버튼이 눌렸을 때 실행될 함수
    public void OnBgImgButton()
    {
        if (baseSprite != null)
        {
            //bgCtrl.ChangeSprite(baseSprite);

            //BgInfoManager에 baseSprite 이미지를 저장시킨다.
            BgInfoManager.Instance.BgSprite = baseSprite;

            //Check를 활성화 시키고 어디에 체크되었는지 나타내기 위해
            //위치를 이동시킨다.
            checkImgTr.position = tr.position;
            checkImg.enabled = true;
            //확인 버튼을 활성화 시킨다.
            selectCheckBtnObj.SetActive(true);
            //확인버튼과 체크표시가 활성화 되어 있을 시간.
            ScPageManager.Instance.SelectBtnResetTime = 10.0f;
        }
    }
}
