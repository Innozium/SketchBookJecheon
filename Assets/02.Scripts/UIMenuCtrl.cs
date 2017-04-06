using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIMenuCtrl : MonoBehaviour {

    public Material normal;
    public Material water;
    private float width = 0.025f;
    private Color color = Color.black;

    private Sprite[] sprites;

    public GameObject gridObj;
    public GameObject sizeGridObj;
    public GameObject colorGridObj;
    public GameObject stampGridObj;
    private StampBtnCtrl[] contentBts;


    private void Awake()
    {
        StartCoroutine(CoStamp_Load_Image());
    }

    #region Stamp Set Up!

    IEnumerator CoStamp_Load_Image()
    {

        //파일이 있는가 확인
        if (!Directory.Exists(Application.dataPath + "/../Resources"))
        {   //파일이 없다면 파일을 생성한다.
            System.IO.Directory.CreateDirectory(Application.dataPath + "/../Resources");
        }

        //해당 파일에서 .png의 확장자를 가지는 모든 파일의 이름을 배열에 저장한다.
        string[] filePaths = Directory.GetFiles(Application.dataPath + "/../Resources", "*.png");

        //content의 자식중에 ScrBtnCtrl 컴포넌트를 가진 애들을 저장
        contentBts = stampGridObj.transform.GetComponentsInChildren<StampBtnCtrl>();
        //Sprite 사이즈는 filePaths 배열의 길이 만큼
        sprites = new Sprite[filePaths.Length];

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
            sprites[i] = Sprite.Create(new_texture, rec, new Vector2(0.5f, 0.5f), 100);
            print(sprites.Length);

        }

        //불러와진 이미지가 1개 이상이라면 버튼에 이미지를 적용한다.
        if (sprites.Length > 0)
        {
            for (int i = 0; i < contentBts.Length; i++)
            {
                if (sprites.Length > i)
                    contentBts[i].StampBtnSetup(sprites[i]);
            }
        }


    }

    #endregion

    #region 브러시 종류
    public void OnPenButton()
    {
        PenManager.Instance.LineChange(normal, color, SELECT_PEN.Pen);
        sizeGridObj.SetActive(true);
        colorGridObj.SetActive(true);
        stampGridObj.SetActive(false);
    }

    public void OnBrushButton()
    {
        PenManager.Instance.LineChange(water, color, SELECT_PEN.Brush);
        sizeGridObj.SetActive(true);
        colorGridObj.SetActive(true);
        stampGridObj.SetActive(false);
    }

    public void OnEraserButton()
    {
        PenManager.Instance.LineChange(normal, Color.white, SELECT_PEN.Eraser);
        sizeGridObj.SetActive(true);
        colorGridObj.SetActive(true);
        stampGridObj.SetActive(false);
    }

    public void OnStampButton()
    {
        PenManager.Instance.LineChange(normal, sprites[0], SELECT_PEN.Stamp);
        sizeGridObj.SetActive(false);
        colorGridObj.SetActive(false);
        stampGridObj.SetActive(true);
    }

    #endregion

    #region 색
    public void OnColorRed()
    {
        if(PenManager.Instance.ColorChange(Color.red))
        {
            color = Color.red;
        }
    }

    public void OnColorBlack()
    {
        if (PenManager.Instance.ColorChange(Color.black))
        {
            color = Color.black;
        }
    }

    public void OnColorGreen()
    {
        if (PenManager.Instance.ColorChange(Color.green))
        {
            color = Color.green;
        }
    }

    public void OnColorYellow()
    {
        if (PenManager.Instance.ColorChange(Color.yellow))
        {
            color = Color.yellow;
        }
    }

    public void OnColorBlue()
    {
        if (PenManager.Instance.ColorChange(Color.blue))
        {
            color = Color.blue;
        }
    }
    #endregion

    #region 크기
    public void OnPenSize0026()
    {
        width = 0.026f;
        PenManager.Instance.width = width;
    }

    public void OnPenSize005()
    {
        width = 0.05f;
        PenManager.Instance.width = width;
    }

    public void OnPenSize02()
    {
        width = 0.2f;
        PenManager.Instance.width = width;
    }

    public void OnPenSize05()
    {
        width = 0.5f;
        PenManager.Instance.width = width;
    }
    #endregion

    #region 취소, 전체 삭제
    public void OnBackButton()
    {
        PenManager.Instance.OnBackButton();
    }

    public void OnDelButton()
    {
        PenManager.Instance.OnDestroyButton();
    }
    #endregion

}
