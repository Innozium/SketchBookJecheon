using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StampBtnCtrl : MonoBehaviour {

    private Button button;
    private Sprite baseSprite;

    void Awake()
    {
        //버튼 컴포넌트를 넣어두자!
        button = GetComponent<Button>();
        //버튼 이벤트 동적 할당
        button.GetComponent<Button>().onClick.AddListener(delegate { OnStampButton(); });
    }

    //버튼에 표시될 이미지와 클릭시 저장될 Image의 정보를 저장!
    public void StampBtnSetup(Sprite baseSprite)
    {
        this.baseSprite = baseSprite;
        GetComponent<Image>().sprite = baseSprite;
    }

    //버튼이 눌렸을 때 실행될 함수
    public void OnStampButton()
    {
        print("qjxms");
        if (baseSprite != null)
        {
            PenManager.Instance.LineChange(baseSprite, SELECT_PEN.Stamp);
        }
    }
}
