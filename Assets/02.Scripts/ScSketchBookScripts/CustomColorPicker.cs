using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomColorPicker : MonoBehaviour {

    public UIMenuCtrl umc;

	public void ColorDebug(Color _c)
    {
        Debug.Log(_c);
        umc.OnColorCustomColor(_c);
    }
}
