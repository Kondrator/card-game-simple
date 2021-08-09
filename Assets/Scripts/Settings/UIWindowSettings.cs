using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIWindowManager;
using UnityEngine.UI;


public class UIWindowSettings : UIWindow {

	[SerializeField]
	private Button	bAudioBackgroundON = null,
					bAudioBackgroundOFF = null,
					bAudioEffectsON = null,
					bAudioEffectsOFF = null;
	private UIAnimation	animAudioBackgroundON = null,
						animAudioBackgroundOFF = null,
						animAudioEffectsON = null,
						animAudioEffectsOFF = null;


	protected override void OnAwake(){
		base.OnAwake();
		
		// animation: audio background
		if( bAudioBackgroundON != null ){
			animAudioBackgroundON = bAudioBackgroundON.GetComponent<UIAnimation>();
		}
		if( bAudioBackgroundOFF != null ){
			animAudioBackgroundOFF = bAudioBackgroundOFF.GetComponent<UIAnimation>();
		}

		// animation: audio effects
		if( bAudioEffectsON != null ){
			animAudioEffectsON = bAudioEffectsON.GetComponent<UIAnimation>();
		}
		if( bAudioEffectsOFF != null ){
			animAudioEffectsOFF = bAudioEffectsOFF.GetComponent<UIAnimation>();
		}


		
		// events: audio background
		bAudioBackgroundON.AddListenerOnClick( () =>{
			SettingsManager.SetAudioBackgroundEnabled( false );
			SetVisible( bAudioBackgroundON,		animAudioBackgroundON,		false );
			SetVisible( bAudioBackgroundOFF,	animAudioBackgroundOFF,		true );
		} );
		bAudioBackgroundOFF.AddListenerOnClick( () =>{
			SettingsManager.SetAudioBackgroundEnabled( true );
			SetVisible( bAudioBackgroundON,		animAudioBackgroundON,		true );
			SetVisible( bAudioBackgroundOFF,	animAudioBackgroundOFF,		false );
		} );
		
		// events: audio effects
		bAudioEffectsON.AddListenerOnClick( () =>{
			SettingsManager.SetAudioEffectsEnabled( false );
			SetVisible( bAudioEffectsON,		animAudioEffectsON,			false );
			SetVisible( bAudioEffectsOFF,		animAudioEffectsOFF,		true );
		} );
		bAudioEffectsOFF.AddListenerOnClick( () =>{
			SettingsManager.SetAudioEffectsEnabled( true );
			SetVisible( bAudioEffectsON,		animAudioEffectsON,			true );
			SetVisible( bAudioEffectsOFF,		animAudioEffectsOFF,		false );
		} );


		OnOpen.AddListener( () =>{
			SetVisible( bAudioBackgroundON,		animAudioBackgroundON,		SettingsManager.IsAudioBackgroundEnabled == true,	true );
			SetVisible( bAudioBackgroundOFF,	animAudioBackgroundOFF,		SettingsManager.IsAudioBackgroundEnabled == false,	true );
			SetVisible( bAudioEffectsON,		animAudioEffectsON,			SettingsManager.IsAudioEffectsEnabled == true,		true );
			SetVisible( bAudioEffectsOFF,		animAudioEffectsOFF,		SettingsManager.IsAudioEffectsEnabled == false,		true );
		} );

	}



	/*public override void CheckKey(){
		base.CheckKey();

		if( IsOpen == true
			&& IsAnimationOpenCompleted == true
			&& Input.GetMouseButton( 0 ) == true
		){
			GameObject goUnderCursor = MyOperationUI.GetFirstUIUnderCursor();
			if( goUnderCursor == null
				|| goUnderCursor.GetComponentInParent<UIWindowSettings>() == false
			){
				Close();
			}
		}
	}*/



	private void SetVisible( Button button, UIAnimation animation, bool isVisible, bool isFast = false ){
		
		if( animation != null ){
			animation.Visible( isVisible, isFast );

		}else if( button != null ){
			button.gameObject.SetActive( isVisible );
		}

	}


}
