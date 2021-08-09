using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayAudioOnTouch : MonoBehaviour {

	[SerializeField]
	private AudioClip clip = null;


	void OnMouseDown(){
		if( MyOperationUI.IsCursorOverUI() == false ){
			AudioManager.PlayEffect( clip );
		}
	}


}
