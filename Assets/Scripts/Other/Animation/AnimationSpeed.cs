using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof(Animation) )]
public class AnimationSpeed : MonoBehaviour {

	[SerializeField]
	private float speed = 1f;

	private new Animation animation = null;

	void Awake(){
		animation = GetComponent<Animation>();
		animation[animation.clip.name].speed = speed;
	}


#if UNITY_EDITOR
	void Update(){
		animation[animation.clip.name].speed = speed;
	}
#endif

}
