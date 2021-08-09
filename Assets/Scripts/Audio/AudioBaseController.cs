using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kondrat.MVC;


public abstract class AudioBaseController : Controller {


	public class NOTIFY {

		public class LOOP {

			public class PLAY {

				public const string NAME = "audio.base.controller.loop.play";

			}

			public class STOP {

				public const string NAME = "audio.base.controller.loop.stop";

			}


			/// <summary>
			/// Type = AudiClip
			/// </summary>
			public const string PARAM_CLIP = "clip";

		}



		public class REPEAT {

			public class PLAY {

				public const string NAME = "audio.base.controller.repeat.play";

			}

			public class STOP {

				public const string NAME = "audio.base.controller.repeat.stop";

			}


			/// <summary>
			/// Type = AudioSet
			/// </summary>
			public const string PARAM_AUDIO_SET = "set";

		}

	}



	[SerializeField]
	[Range( 0, 1f )]
	private float maxVolume = 1f;
	


	
	private AudioSource audioSource = null;
	private AudioSource AudioSource{
		get{
			if( audioSource == null ){
				audioSource = this.AddComponent<AudioSource>();
			}
			return audioSource;
		}
	}



	private System.Action callbaclAudioEnded = null;
	private float timerRepeatAudio = 0;
	private AudioSet audioSetRepeatCurrent = null;
	private bool isLoading = false;
	private bool isApplicationPause = false;





	protected void Start() {
		SettingsManager.OnChangeAudioBackground.AddListener( enabled => {
			if( enabled == false
				&& AudioSource.isPlaying == true
			) {
				AnimateVolume( AudioSource.volume, 0 );

			}else if(	enabled == true
						&& AudioSource.isPlaying == false
			) {
				AnimateVolume( AudioSource.volume, maxVolume );
			}
		} );
	}

	protected void Update(){
		if( SettingsManager.IsAudioBackgroundEnabled == false ) {
			return;
		}

		if( callbaclAudioEnded != null
			&& AudioSource.isPlaying == false
			&& isLoading == false
			&& isApplicationPause == false
		) {
			timerRepeatAudio -= Time.deltaTime;
			if( timerRepeatAudio < 0 ){
				callbaclAudioEnded();
			}
		}
	}


	private void OnApplicationPause( bool pause ) {
		isApplicationPause = pause;
	}




	protected override void PreInitiate() { }

	protected override void Initiate() {

		Add(
			NOTIFY.LOOP.PLAY.NAME,
			( NotifyData data ) => {
				PlayLoop( data.GetParam<AudioClip>( NOTIFY.LOOP.PARAM_CLIP ) );
			}
		);

		Add(
			NOTIFY.LOOP.STOP.NAME,
			( NotifyData data ) => {
				StopPlay();
			}
		);



		Add(
			NOTIFY.REPEAT.PLAY.NAME,
			( NotifyData data ) => {
				PlayRepeat( data.GetParam<AudioSet>( NOTIFY.REPEAT.PARAM_AUDIO_SET ) );
			}
		);

		Add(
			NOTIFY.REPEAT.STOP.NAME,

			( NotifyData data ) => {
				StopRepeat();
			}
		);

	}



	/// <summary>
	/// Start play audio in loop mode.
	/// </summary>
	protected void PlayLoop( AudioClip clip ){
		AudioSource.clip = clip;
		AudioSource.loop = true;
		AudioSource.Play();
		AnimateVolume( 0, maxVolume );
	}

	/// <summary>
	/// Stop loop current audio.
	/// </summary>
	protected void StopPlay( bool isFast = false ){
		if( isFast == true ){
			AudioSource.Stop();

		}else{
			AnimateVolume( AudioSource.volume, 0 );
		}
	}

	/// <summary>
	/// Play audio when callback end of play audio.
	/// </summary>
	protected void PlayRepeat( AudioSet audioSet, float timeRepeatMin = 0.2f, float timeRepeatMax = 0.5f ) {
		if( audioSetRepeatCurrent == audioSet ){
			return;
		}

		audioSetRepeatCurrent = audioSet;


		this.callbaclAudioEnded = () => {
			isLoading = true;

			// load random audio
			audioSet.GetRandom(
				clip => {
					isLoading = false;

					// is loaded
					AudioSource.clip = clip;
					AudioSource.Play();
					timerRepeatAudio = Random.Range( timeRepeatMin, timeRepeatMax );

					AudioSource.loop = false;
					AnimateVolume( 0, maxVolume );
					timerRepeatAudio = 0;

				},
				AudioSource.clip
			);
		};

		AnimateVolume( AudioSource.volume, 0 );

	}

	/// <summary>
	/// Play audio when callback end of play audio.
	/// </summary>
	protected void PlayRepeatOne( AudioSet audioSet, float timeRepeatMin = 0.2f, float timeRepeatMax = 0.5f ){
		if( audioSetRepeatCurrent == audioSet ){
			return;
		}

		audioSetRepeatCurrent = audioSet;
		this.callbaclAudioEnded = StopRepeat;

		AudioSource.loop = false;
		AnimateVolume( 0, maxVolume );
		timerRepeatAudio = 0;

		AudioSource.clip = null;
		audioSet.GetRandom(
			clip => {
				AudioSource.clip = AudioSource.clip;
				AudioSource.Play();
				timerRepeatAudio = Random.Range( timeRepeatMin, timeRepeatMax );
			},
			AudioSource.clip
		);
	}

	/// <summary>
	/// Stop current play audio for repeat.
	/// </summary>
	protected void StopRepeat(){
		audioSetRepeatCurrent = null;
		this.callbaclAudioEnded = null;
		AnimateVolume( AudioSource.volume, 0 );
	}


	


	private TimerExecutor.Item timerAudioSource = null;
	protected void AnimateVolume( float from, float to, float length = 0.5f ){
		if( timerAudioSource != null ){
			timerAudioSource.ForceComplete( false );
			timerAudioSource = null;
		}

		to = SettingsManager.IsAudioBackgroundEnabled == true ? to : 0;
		AudioSource.volume = from;

		if( to > from ) {
			AudioSource.Play();
		}

		timerAudioSource = TimerExecutor.Add(
			length,
			( float progress ) =>{
				AudioSource.volume = Mathf.Lerp( from, to, progress );
			},
			() =>{
				if( to == 0 ){
					AudioSource.Pause();
				}
				timerAudioSource = null;
			}
		);
	}


	

}



public abstract class AudioBaseController<TView> : AudioBaseController where TView : View {

	private new TView view = null;
	public new TView View {
		get {
			if( view == null ) {
				view = GetComponent<TView>();
				if( view == null ) {
					view = Find( view );
				}
			}
			return view;
		}
	}

}