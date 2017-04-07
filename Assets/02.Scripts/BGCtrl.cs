using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCtrl : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //SpriteRenderer 컴포넌트를 가져온다.
        spriteRenderer = GetComponent<SpriteRenderer>();
        //태그명을 BG로 설정한다.
        this.gameObject.tag = "BG";
    }

    //이미지를 바꾼다.
    public void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
