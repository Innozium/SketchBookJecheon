using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScPageUICtrl : MonoBehaviour {

    public GameObject bgGridObj;
    private bgImgBtnCtrl[] bgImgBtnCtrls;
    private Sprite[] bgSprites;

    private GameObject selectCheckBtnObj;

    #region 배경 이미지 Set Up!
    IEnumerator CoStamp_Load_Image()
    {

        //파일이 있는가 확인
        if (!Directory.Exists(Application.dataPath + "/../Resources/Bg"))
        {   //파일이 없다면 파일을 생성한다.
            System.IO.Directory.CreateDirectory(Application.dataPath + "/../Resources/Bg");
        }

        //해당 파일에서 .png의 확장자를 가지는 모든 파일의 이름을 배열에 저장한다.
        string[] filePaths = Directory.GetFiles(Application.dataPath + "/../Resources/Bg", "*.png");

        //content의 자식중에 ScrBtnCtrl 컴포넌트를 가진 애들을 저장
        bgImgBtnCtrls = bgGridObj.transform.GetComponentsInChildren<bgImgBtnCtrl>();
        //Sprite 사이즈는 filePaths 배열의 길이 만큼
        bgSprites = new Sprite[filePaths.Length];

        for (int i = 0; i < filePaths.Length; i++)
        {
            //print(filePaths[i]);
            //반복문을 통해 www로 이미지 파일을 다운로드
            WWW www = new WWW("file://" + filePaths[i]);
            yield return www;
            //Texture2D를 만든 뒤 www로 생성한다.
            Texture2D new_texture = new Texture2D(1024, 1024);
            www.LoadImageIntoTexture(new_texture);

            Rect rec = new Rect(0, 0, new_texture.width, new_texture.height);
            //Sprite를 새롭게 생성한다.
            bgSprites[i] = Sprite.Create(new_texture, rec, new Vector2(0.5f, 0.5f), 100);
            print(bgSprites.Length);

        }

        //불러와진 이미지가 1개 이상이라면 버튼에 이미지를 적용한다.
        if (bgSprites.Length > 0)
        {
            for (int i = 0; i < bgImgBtnCtrls.Length; i++)
            {
                if (bgSprites.Length > i)
                {
                    bgImgBtnCtrls[i].BgImgBtnSetup(bgSprites[i]);
                    bgImgBtnCtrls[i].selectCheckBtnObj = selectCheckBtnObj;
                }             
            }
        }

        ScChangeManager.Instance.OnFadeIN();
    }
    #endregion

    private void Awake()
    {
        //selectCheckBtnObj에 오브젝트를 찾은 뒤에 비활성화
        selectCheckBtnObj = GameObject.Find("SelectCheckBtn");
        selectCheckBtnObj.SetActive(false);
    }


    // Use this for initialization
    void Start () {
        StartCoroutine(CoStamp_Load_Image());
	}

    //ScSketchBook 씬으로 옮긴다.
    public void OnSelectCheckBtn()
    {
        ScChangeManager.Instance.OnSceneChange("ScSketchBook");
    }

}
