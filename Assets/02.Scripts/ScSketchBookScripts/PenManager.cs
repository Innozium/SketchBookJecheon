using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SELECT_PEN
{
    Pen = 0,
    Brush,
    Eraser,
    Stamp
}


public class PenManager : MonoBehaviour {

    private Transform[] drCount;

    public GameObject StampPrefab;

    public GameObject linePrefab; //라인 프리팹
    public Transform drawPanel; //라인이 부모로 잡을 패널

    private int linesCount = 0;
    private int currentSortingOrder = 0;

    private bool is_DrawPanel = false;
    public SELECT_PEN select_pen = SELECT_PEN.Pen;
    
    public Material material;
    public Material normalMaterial;
    public Color color = Color.black;
    public Sprite sprite;

    //초기화 시간 5분
    private float resetTimer = 300.0f;
    private float init_Time = 300.0f;

    //테스트 마우스용
    private LineCtrl lineCtrlMouse;

    [Range(0.025f, 5)]
    public float width = 0.025f;

    public int stampScale = 40;

    //현재 작동중인 라인의 스크립트가 저장될 리스트
    private List<LineCtrl> lineList = new List<LineCtrl>();

    //싱글턴 패턴을 위한 인스턴스 변수 선언
    private static PenManager instance = null;

    public static PenManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gObj = new GameObject("_PenManager");
                instance = gObj.AddComponent<PenManager>();
            }

            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        #region 테스트용 코드
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    color = Color.black;
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    color = Color.green;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    color = Color.white;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    if(currentSortingOrder > 0)
        //    {
        //        GameObject[] lines = GameObject.FindGameObjectsWithTag("LINE");
        //        for (int i = 0; i < lines.Length; i++)
        //        {
        //            if(lines[i].GetComponent<LineCtrl>().sortingOrder == currentSortingOrder)
        //            {
        //                Destroy(lines[i]);
        //                currentSortingOrder--;
        //                linesCount--;
        //                break;
        //            }
        //        }
        //    }

        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    if (currentSortingOrder > 0)
        //    {
        //        GameObject[] lines = GameObject.FindGameObjectsWithTag("LINE");

        //        for (int i = 0; i < lines.Length; i++)
        //        {
        //            Destroy(lines[i]);

        //        }

        //        currentSortingOrder = 0;
        //        linesCount = 0;
        //    }
        //}
        //라인이 생성되었는지 확인
        //drCount = drawPanel.transform.GetComponentsInChildren<Transform>();

        #endregion


        UseLineFeature();

#if UNITY_EDITOR
        UseLineFeatureMouse();
#endif

        #region 라인 정리용

        //시간 감소!
        if (resetTimer < 0)
        {
            //시간이 0이하라면 모든 그림들을 삭제하여 
            //그림영역을 초기화 및 시간 초기화!
            OnDestroyButton();
            OnTimeReset();
        }
        else
        {
            resetTimer -= Time.deltaTime;
        }

        #endregion

    }


    #region 터치입력 및 라인생성
    private void UseLineFeature()
    {
         for (int i = 0; i < Input.touchCount; i++)
        {
            //터치 입력이 있다면 무조건 시간 초기화!
            OnTimeReset();

            //기존에 없는 새로운 터치 입력
            if (Input.touches[i].phase == TouchPhase.Began && is_DrawPanel)
            {
                //현재 펜의 상태가 스탬프라면!
                if (select_pen == SELECT_PEN.Stamp)
                {
                    //라인을 새롭게 만들자!
                    GameObject stamp = (GameObject)Instantiate(StampPrefab, Vector3.zero, Quaternion.identity);

                    stamp.transform.SetParent(drawPanel);

                    stamp.transform.localScale = new Vector3(stampScale, stampScale, 0.0f);

                    stamp.name = "Stamp";

                    stamp.tag = "LINE";

                    LineCtrl lineCtrl = stamp.GetComponent<LineCtrl>();

                    if (lineCtrl == null)
                    {
                        stamp.AddComponent<LineCtrl>();
                        lineCtrl = stamp.GetComponent<LineCtrl>();
                    }
                    
                    linesCount++;

                    currentSortingOrder++;

                    lineCtrl.SpriteCreate();

                    lineCtrl.SetSortingOrder(currentSortingOrder);

                    lineCtrl.SpriteImgSetUp(sprite);

                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);

                    pos.z = 0;

                    lineCtrl.SpritePos(pos);
                }
                else //그렇지 않다는 것은 라인이다!
                {
                    //라인을 새롭게 만들자!
                    GameObject line = (GameObject)Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

                    line.transform.SetParent(drawPanel);

                    line.name = "Line";

                    line.tag = "LINE";

                    LineCtrl lineCtrl = line.GetComponent<LineCtrl>();

                    if (lineCtrl == null)
                    {
                        line.AddComponent<LineCtrl>();
                        lineCtrl = line.GetComponent<LineCtrl>();
                    }

                    linesCount++;

                    currentSortingOrder++;

                    lineCtrl.Create();

                    lineCtrl.SetSortingOrder(currentSortingOrder);

                    lineCtrl.SetMaterial(material);

                    lineCtrl.SetColor(color);

                    lineCtrl.SetWidth(width, width);

                    lineCtrl.touchId = Input.touches[i].fingerId;

                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);

                    pos.z = 0;

                    lineCtrl.AddPoint(pos);

                    lineList.Add(lineCtrl);
                }

                
            }
            else if (Input.touches[i].phase == TouchPhase.Moved && is_DrawPanel)
            {
                for (int touch = 0; touch < lineList.Count; touch++)
                {
                    if (lineList[touch].touchId == Input.touches[i].fingerId)
                    {
                        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                        pos.z = 0;
                        lineList[touch].AddPoint(pos);
                    }
                }
            }
            else if (Input.touches[i].phase == TouchPhase.Ended || !is_DrawPanel)
            {
                for (int touch = 0; touch < lineList.Count; touch++)
                {
                    if (lineList[touch].touchId == Input.touches[i].fingerId)
                    {
                        lineList.Remove(lineList[touch]);
                    }
                }
            }
        }
    }
    #endregion

    #region 마우스 입력 및 라인생성
    private void UseLineFeatureMouse()
    {
        if (Input.GetMouseButtonDown(0) && is_DrawPanel)
        {
            //입력이 있다면 무조건 시간 초기화!
            OnTimeReset();

            if (select_pen == SELECT_PEN.Stamp)
            {
                //라인을 새롭게 만들자!
                GameObject stamp = (GameObject)Instantiate(StampPrefab, Vector3.zero, Quaternion.identity);

                stamp.transform.SetParent(drawPanel);

                stamp.transform.localScale = new Vector3(stampScale, stampScale, 0.0f);

                stamp.name = "Stamp";

                stamp.tag = "LINE";

                LineCtrl lineCtrl = stamp.GetComponent<LineCtrl>();

                if (lineCtrl == null)
                {
                    stamp.AddComponent<LineCtrl>();
                    lineCtrl = stamp.GetComponent<LineCtrl>();
                }

                linesCount++;

                currentSortingOrder++;
                
                lineCtrl.SpriteCreate();

                lineCtrl.SetSortingOrder(currentSortingOrder);

                lineCtrl.SpriteImgSetUp(sprite);

                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                clickPosition.z = 0;

                lineCtrl.SpritePos(clickPosition);
            }
            else
            {
                GameObject line = (GameObject)Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

                line.transform.SetParent(drawPanel);

                line.name = "Line";

                line.tag = "LINE";

                lineCtrlMouse = line.GetComponent<LineCtrl>();

                lineCtrlMouse.Create();

                linesCount++;

                currentSortingOrder++;

                lineCtrlMouse.SetSortingOrder(currentSortingOrder);

                lineCtrlMouse.SetMaterial(material);

                lineCtrlMouse.SetColor(color);

                lineCtrlMouse.SetWidth(width, width);
            }
           
        }
        else if (Input.GetMouseButtonUp(0) || !is_DrawPanel)
        {
            lineCtrlMouse = null;
        }

        if (lineCtrlMouse != null)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;
            lineCtrlMouse.AddPoint(clickPosition);
        }
    }
    #endregion

    #region 뒤로가기
    public void OnBackButton()
    {
        if (currentSortingOrder > 0)
        {
            GameObject[] lines = GameObject.FindGameObjectsWithTag("LINE");
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].GetComponent<LineCtrl>().sortingOrder == currentSortingOrder)
                {
                    Destroy(lines[i]);
                    currentSortingOrder--;
                    linesCount--;
                    return;
                }
            }
        }
    }
    #endregion

    #region 전체 지우기
    public void OnDestroyButton()
    {
        if (currentSortingOrder > 0)
        {
            GameObject[] lines = GameObject.FindGameObjectsWithTag("LINE");

            for (int i = 0; i < lines.Length; i++)
            {
                Destroy(lines[i]);

            }

            currentSortingOrder = 0;
            linesCount = 0;
        }
    }
    #endregion

    #region is_DrawPanel
    //현재 선택된 패널이 그리기 영역인지 아닌지 판별하기 위해 사용
    //트리거 이벤트는 UICanvas와 DrawCanvas에 존재
    public void OnIsDrawPanel()
    {
        is_DrawPanel = true;
    }

    public void OffIsDrawPanel()
    {
        is_DrawPanel = false;
    }
    #endregion

    #region 라인 종류 변경 및 색 변경
    public void LineChange(Material material, Color color, SELECT_PEN select_pen)
    {
        this.material = material;
        this.color = color;
        this.select_pen = select_pen;
    }

    public void LineChange(Material material, Sprite sprite, SELECT_PEN select_pen)
    {
        this.material = material;
        this.color = Color.white;
        this.sprite = sprite;
        this.select_pen = select_pen;
    }

    public void LineChange(Sprite sprite, SELECT_PEN select_pen)
    {
        this.material = this.normalMaterial;
        this.color = Color.white;
        this.sprite = sprite;
        this.select_pen = select_pen;
    }



    public bool ColorChange(Color color)
    {
        if(select_pen != SELECT_PEN.Eraser)
        {
            this.color = color;
            return true;
        }

        return false;
    }

    #endregion

    #region 리셋 시간 초기화 함수
    public void OnTimeReset()
    {
        resetTimer = init_Time;
    }
    #endregion

    public void testPrint()
    {
        print("test");
    }

}
