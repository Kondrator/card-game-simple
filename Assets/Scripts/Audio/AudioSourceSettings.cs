using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioSourceSettings : MonoBehaviour {

	public enum TypeAudio{
		Background,
		Effect
	}

	[SerializeField]
	private TypeAudio typeAudio = TypeAudio.Effect;
	public TypeAudio AudioType{ get{ return typeAudio; } set{ typeAudio = value; } }

	private AudioSource audioSource = null;


	void Awake(){
		audioSource = GetComponent<AudioSource>();
		
		SettingsManager.OnChangeAudioBackground.AddListener( OnChangeBackground );
		SettingsManager.OnChangeAudioEffect.AddListener( OnChangeEffect );
	}

	void OnDestroy(){
		SettingsManager.OnChangeAudioBackground.RemoveListener( OnChangeBackground );
		SettingsManager.OnChangeAudioEffect.RemoveListener( OnChangeEffect );
	}

	void OnEnable(){
		if( typeAudio == TypeAudio.Background ){
			SetEnable( SettingsManager.IsAudioBackgroundEnabled );

		}else{
			SetEnable( SettingsManager.IsAudioEffectsEnabled );
		}
	}



	private void SetEnable( bool isEnable ){
		if( audioSource != null ){
			audioSource.enabled = isEnable;
		}
	}


	private void OnChangeBackground( bool changeToOn ){
		if( typeAudio == TypeAudio.Background ){
			SetEnable( changeToOn );
		}
	}

	private void OnChangeEffect( bool changeToOn ){
		if( typeAudio == TypeAudio.Effect ){
			SetEnable( changeToOn );
		}
	}

}
