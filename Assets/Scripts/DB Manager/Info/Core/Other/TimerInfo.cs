using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TimerInfo : DataInfo {



	public bool IsInited { get { return isInited; } set { isInited = value; } }
	private bool isInited = false;


	private ulong time = 0;


	public ulong DurationSeconds {
		get {
			return MyOperationDevice.GetUpTime() - time;
		}
		set {
			time = MyOperationDevice.GetUpTime() - value;
		}
	}






	
	public TimerInfo() {
		Reset();
	}
	public TimerInfo( JObject data ) : base( data ) { }





	/// <summary>
	/// Reset timer to current time
	/// </summary>
	public void Reset() {
		time = MyOperationDevice.GetUpTime();
	}






	private const string KEY_IS_INITED = "inited";
	private const string KEY_TIME = "time";


	public override JObject ToJSON() {

		JObject data = new JObject();

		data[KEY_IS_INITED] = IsInited;
		data[KEY_TIME] = time;

		return data;
	}

	public override void FromJSON( JObject data ) {

		if( data == null ) {
			return;
		}

		Reset();
		isInited = data[KEY_IS_INITED].GetValue( IsInited );
		time = data[KEY_TIME].GetValue( MyOperationDevice.GetUpTime() );

	}

}
