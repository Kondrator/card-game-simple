using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceSimple : MonoBehaviour {

	private AudioSource audioSource = null;


	void Awake(){
		audioSource = this.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;

		Check();
		SettingsManager.OnChangeAudioEffect.AddListener( ( bool isEnabled ) =>{
			SetEnable( isEnabled );
		} );
	}


	private void OnEnable() {
		Check();

		if( audioSource.enabled == true ) {
			audioSource.Play();
		}
	}



	private void Check(){
		SetEnable( SettingsManager.IsAudioEffectsEnabled );
	}
	private void SetEnable( bool isEnabled ){
		if( audioSource != null ){
			audioSource.enabled = isEnabled;
		}
	}


	/*
	void OnDestroy(){
		MyOperationGC.Destroy( audioMusic );
	}
	*/


}
