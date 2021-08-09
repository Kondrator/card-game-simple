using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFitScale : MonoBehaviour {

	private RectTransform rect = null;

	void Awake(){
		rect = this.transform as RectTransform;
	}

	void OnEnable(){
		// restore scale for correct calculate
		this.transform.localScale = Vector3.one;

		float width = rect.GetWidth() * rect.transform.lossyScale.x;
		if( width > Screen.width ){
			float scale = Screen.width / width;
			this.transform.localScale = new Vector3( scale, scale, 1f );

		}else{
			this.transform.localScale = Vector3.one;
		}
	}

}
