using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour {

	
	private static AudioManager singleton_;
	public static AudioManager singleton{
		get{
			if( singleton_ == null ){
				GameObject goNew = new GameObject( typeof(AudioManager).Name );
				singleton_ = goNew.AddComponent<AudioManager>();
			}
			return singleton_;
		}
	}


	private AudioSource audioSource = null;
	public AudioSource AudioSource{
		get{
			if( audioSource == null ){
				audioSource = Camera.main.AddComponent<AudioSource>();
			}
			return audioSource;
		}
	}


	private List<AudioClip> blockListRepeat = new List<AudioClip>();


	public static void PlayEffect( AudioClip clip, float delayBlockRepeat = 0.01f ) {
		
		PlayEffect( clip, singleton.AudioSource, delayBlockRepeat );

	}

	public static void PlayEffect( AudioClip clip, AudioSource source, float delayBlockRepeat = 0.01f ) {
		
		if( SettingsManager.IsAudioEffectsEnabled == false ){
			return;
		}
		
		if( clip != null
			&& source != null
		){
			if( delayBlockRepeat > 0 ){
				if( singleton.blockListRepeat.Contains( clip ) == true ){
					return;

				}else{
					singleton.blockListRepeat.Add( clip );
					TimerExecutor.Add( delayBlockRepeat,  () =>{
						singleton.blockListRepeat.Remove( clip );
					} );
				}
			}
			source.PlayOneShot( clip );
		}

	}


}
