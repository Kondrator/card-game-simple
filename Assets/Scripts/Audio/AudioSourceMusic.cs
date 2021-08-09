using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceMusic : MonoBehaviour {

	[SerializeField]
	private AudioClip audioMusic = null;

	private AudioSource audioSource = null;

	void Awake(){
		audioSource = this.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = true;
		audioSource.clip = audioMusic;

		Check();
		SettingsManager.OnChangeAudioBackground.AddListener( ( bool isEnabled ) =>{
			SetEnable( isEnabled );
		} );
	}

	private void Check(){
		SetEnable( SettingsManager.IsAudioBackgroundEnabled );
	}
	private void SetEnable( bool isEnabled ){
		if( audioSource != null ){
			if( isEnabled == true ){
				audioSource.Play();

			}else{
				audioSource.Pause();
			}
		}
	}



}
