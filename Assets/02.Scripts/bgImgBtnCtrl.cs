using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bgImgBtnCtrl : MonoBehaviour {

    private Button button;
    private Sprite baseSprite;
    private BGCtrl bgCtrl;

    void Awake()
    {
        //배경을 담당할 오브젝트의 BGCtrl를 가져온다.
        bgCtrl = GameObject.FindGameObjectWithTag("BG").GetComponent<BGCtrl>();
        //버튼 컴포넌트를 넣어두자!
        button = GetComponent<Button>();
        //버튼 이벤트 동적 할당
        button.GetComponent<Button>().onClick.AddListener(delegate { OnBgImgButton(); });
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
            bgCtrl.ChangeSprite(baseSprite);
        }
    }
}
