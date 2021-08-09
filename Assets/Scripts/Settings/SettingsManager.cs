using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameToolkit.Localization;


public static class SettingsManager{
	
	private const string KEY_AUDIO_BACKGROUND_ENABLED =		"settings.audio.background.enabled";
	private const string KEY_AUDIO_EFFECTS_ENABLED =		"settings.audio.effects.enabled";


	
	public static bool IsAudioBackgroundEnabled{
		get{
			return PlayerPrefs.GetInt( KEY_AUDIO_BACKGROUND_ENABLED, 1 ) == 1 ? true : false;
		}
	}
	public static bool IsAudioEffectsEnabled{
		get{
			return PlayerPrefs.GetInt( KEY_AUDIO_EFFECTS_ENABLED, 1 ) == 1 ? true : false;
		}
	}

	public static SystemLanguage Language{
		get{
			return Localization.Instance.CurrentLanguage;
		}
		set{
			Localization.Instance.CurrentLanguage = value;
		}
	}




	public class EventChangeBool : UnityEvent<bool>{}
	public static EventChangeBool OnChangeAudioBackground = new EventChangeBool();
	public static EventChangeBool OnChangeAudioEffect = new EventChangeBool();


	
	public static void SetAudioBackgroundEnabled( bool isEnabled ){
		PlayerPrefs.SetInt( KEY_AUDIO_BACKGROUND_ENABLED, isEnabled == true ? 1 : 0 );
		OnChangeAudioBackground.Invoke( isEnabled );
	}
	public static void SetAudioEffectsEnabled( bool isEnabled ){
		PlayerPrefs.SetInt( KEY_AUDIO_EFFECTS_ENABLED, isEnabled == true ? 1 : 0 );
		OnChangeAudioEffect.Invoke( isEnabled );
	}


	public static void CheckLanguage(){
		SystemLanguage language = Application.systemLanguage;

		switch( language ){

			case SystemLanguage.Russian:
			case SystemLanguage.Ukrainian:
				language = SystemLanguage.Russian;
				break;

			case SystemLanguage.Japanese:
				language = SystemLanguage.Japanese;
				break;

			case SystemLanguage.Korean:
				language = SystemLanguage.Korean;
				break;

			case SystemLanguage.Chinese:
				language = SystemLanguage.Chinese;
				break;

			default:
				language = SystemLanguage.English;
				break;

		}

		Language = language;
	}


}