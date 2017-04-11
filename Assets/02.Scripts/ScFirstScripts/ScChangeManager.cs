using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//현재 씬이 로드되고 있는지 아닌지를 나타낸다.
public enum SCENESTATE
{
    NONE = 0,
    LOAD,
    END
}


public class ScChangeManager : MonoBehaviour {

    private CameraFadeInOut fadeInOutCtrl = null;
    private float FadeOutTime = 0.5f;
    private float FadeInTime = 0.5f;

    public SCENESTATE sceneState = SCENESTATE.NONE;

    private AsyncOperation async = null;

    //public GameObject LoadPage;
    //public Image LoadPageProgressImg;


    //싱글턴 패턴을 위한 인스턴스 변수 선언
    private static ScChangeManager instance = null;

    public static ScChangeManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gObj = new GameObject("_ScChangeManager");
                instance = gObj.AddComponent<ScChangeManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        //SceneChangeManager 클래스를 인스턴스에 대입
        instance = this;

        //씬이 넘어가도 오브젝트가 삭제되지 않도록 한다.
        DontDestroyOnLoad(this.gameObject);

        //init을 통해 설정하자.
        Init();
    }

    //초기화 설정할 것들이 있다면 이곳에 추가하자.
    public void Init()
    {

        //fadeInOutCtrl 아무것도 없다면 추가하자.
        if (fadeInOutCtrl == null)
        {
            fadeInOutCtrl = GetComponent<CameraFadeInOut>();
            if (fadeInOutCtrl == null)
            {
                fadeInOutCtrl = this.gameObject.AddComponent<CameraFadeInOut>();
            }
        }
    }

    //씬을 바꾸기 위해 외부에서 호출하는 함수
    public void OnSceneChange(string name)
    {
        //CoLoadGame 코루틴을 실행한다.
        StartCoroutine(CoLoadGame(name));
    }

    //외부에서 FadeIn을 요구할 때 사용할 함수
    public void OnFadeIN()
    {
        //로딩이 끝났으니 END로.
        sceneState = SCENESTATE.END;
        // 로드가 끝난후 페이드 인 들어가게 바꾼다.
        this.fadeInOutCtrl.FadeIn(this.FadeInTime);
    }

    //CoLoadGame에서 사용하는 함수
    //현재 프로젝트에서는 주석처리하고 OnFadeIn을 사용 중
    IEnumerator CoDoneCheck()
    {
        while(!async.isDone)
        {
            yield return null;   
        }

        Debug.Log("넘어간뒤" + async.isDone);
        if (async.isDone)
        {
            sceneState = SCENESTATE.END;
            //LoadPageProgressImg.fillAmount = async.progress;
            //LoadPage.SetActive(false);
        }

        // 로드가 끝난후 페이드 인 들어가게 바꾼다.
        this.fadeInOutCtrl.FadeIn(this.FadeInTime);
    }

    //OnSceneChange에서 실행시키는 코루틴 함수
    //씬 로딩에 대해 실행된다.
    IEnumerator CoLoadGame(string sceneName)
    {
        //카메라 페이드 아웃
        this.fadeInOutCtrl.FadeOut(this.FadeOutTime);

        //카메라가 페이드 아웃 될 시간동안 실행을 미룬다.
        yield return new WaitForSeconds(this.FadeOutTime);

        //현재 로드상태를 설정한다.
        sceneState = SCENESTATE.LOAD;

        //로딩 페이지를 활성화 시킨다.
        //LoadPage.SetActive(true);

        //비동기 로딩을 하기 위한 변수
        async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        //장면이 준비된 즉시 장면이 활성화되는 것을 막는다.
        async.allowSceneActivation = false;

        //로딩이 완료되었는지 확일할 불 변수
        bool isDone = false;

        while (!isDone)
        {
            //layer.text (async.progress * 111.111f + 0.00011f).ToString() + "%";
            //LoadPageProgressImg.fillAmount = async.progress;
            yield return null;

            if (async.progress >= 0.9f)
                isDone = true;

            //Debug.Log((async.progress * 111.111f + 0.00011f).ToString() + "%");
        }

        //LoadPageProgressImg.fillAmount = async.progress;

        //장면이 준비된 즉시 장면이 활성화되는 것을 허용한다.
        async.allowSceneActivation = true;

        //DontDestroyOnLoad(LoadPage);

        //로드가 끝났는지 체크할 코루틴을 실행한다.
        //StartCoroutine(CoDoneCheck());
    }
}
