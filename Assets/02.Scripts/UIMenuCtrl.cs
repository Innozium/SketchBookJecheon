using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuCtrl : MonoBehaviour {

    public Material normal;
    public Material water;
    private float width = 0.025f;
    private Color color = Color.black;

    public Sprite[] sprite;

    #region 브러시 종류
    public void OnPenButton()
    {
        PenManager.Instance.LineChange(normal, color, SELECT_PEN.Pen);
    }

    public void OnBrushButton()
    {
        PenManager.Instance.LineChange(water, color, SELECT_PEN.Brush);
    }

    public void OnEraserButton()
    {
        PenManager.Instance.LineChange(normal, Color.white, SELECT_PEN.Eraser);
    }

    public void OnStampButton()
    {
        PenManager.Instance.LineChange(normal, sprite[0], SELECT_PEN.Stamp);
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
