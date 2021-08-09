using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDeactivate : MonoBehaviour {


	[SerializeField, Range( 0.1f, 10f )]
	private float time = 2f;

	private TimerExecutor.Item timer = null;


	void OnEnable(){
		
		timer = TimerExecutor.Add( time, time + 1f, ( float progress ) =>{}, () =>{
			this.gameObject.Deactivate();
			timer = null;
		} );

	}

	void OnDisable(){
		
		if( timer != null ){
			timer.ForceComplete( false );

			if( this.gameObject.activeSelf == true ){
				TimerExecutor.Add( 0.1f, () => {
					if( this != null ){
						this.gameObject.Deactivate();
					}
				} );
			}
		}

	}



	public void SetTime( float time ) {
		this.time = time;
	}

}
