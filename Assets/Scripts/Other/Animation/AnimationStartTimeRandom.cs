using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof(Animation) )]
public class AnimationStartTimeRandom : MonoBehaviour {

	private new Animation animation = null;


	void Awake(){
		animation = GetComponent<Animation>();
	}


	void OnEnable() {
		animation[animation.clip.name].time = Random.Range( 0, animation.clip.length );
	}


}
