using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SpriteAnimation : MonoBehaviour, IMyManagerMonoBehaviour {

	[SerializeField]
	private SpriteRenderer target = null;
	[SerializeField]
	private Image targetUI = null;
	[SerializeField]
	private Sprite[] sprites = new Sprite[0];
	[SerializeField, Range( 0.01f, 5f )]
	private float duration = 1f;


	private int index = 0;
	private float timer = 0, timeStep = 0;


	void OnEnable(){
		index = -1;
		timer = 0;
		timeStep = duration / (float)sprites.Length;
		Next();
		Continue();
	}

	void OnDisable(){
		Pause();
	}


	public void Pause(){
		MyManagerMonoBehaviour.Remove( this );
	}
	public void Continue(){
		MyManagerMonoBehaviour.Add( this );
	}


	public void UpdateMe( float timeDelta, MyManagerMonoBehaviourType type ){
		
		timer += timeDelta;
		if( timer > timeStep ){
			timer = 0;

			Next();
		}

	}



	private void Next(){
		index++;
		if( index >= sprites.Length
			|| index < 0
		){
			index = 0;
		}

		if( target != null ){
			target.sprite = sprites[index];
		}
		if( targetUI != null ){
			targetUI.sprite = sprites[index];
		}
	}




}
