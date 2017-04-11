using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstChange : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //씬을 ScPageSelect로 옮긴다.
        ScChangeManager.Instance.OnSceneChange("ScPageSelect");
		
	}
	
}
