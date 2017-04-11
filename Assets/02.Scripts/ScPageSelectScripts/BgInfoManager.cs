using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgInfoManager : MonoBehaviour {

    private Sprite bgSprite;
    public Sprite BgSprite
    {
        set
        {
            bgSprite = value;
        }
        get
        {
            return bgSprite;
        }
    }


    //싱글턴 패턴을 위한 인스턴스 변수 선언
    private static BgInfoManager instance = null;

    public static BgInfoManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gObj = new GameObject("_BgInfoManager");
                instance = gObj.AddComponent<BgInfoManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        //BgInfoManager 클래스를 인스턴스에 대입
        instance = this;

        //씬이 넘어가도 오브젝트가 삭제되지 않도록 한다.
        DontDestroyOnLoad(this.gameObject);

    }
}
