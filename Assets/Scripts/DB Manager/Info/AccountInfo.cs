using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;


public class AccountInfo : DataInfo{

	


	private CustomizationInfo customization = null;
	public CustomizationInfo Customization { get { return customization = customization ?? new CustomizationInfo(); } }




	public AccountInfo() :base(){}
	public AccountInfo( JObject data ) :base( data ){}



	


	public override JObject ToJSON(){
		
		JObject data = new JObject();


		// customization
		data[CustomizationInfo.KEY_INFO] = Customization.ToJSON();


		//Debug.Log( "SAVE - ACCOUNT DATA: " + data );

		return data;
	}

	public override void FromJSON( JObject data ){

		if( data == null ){
			return;
		}

		//Debug.Log( "LOAD - ACCOUNT DATA: " + data );
		

		// character
		Customization.FromJSON( data[CustomizationInfo.KEY_INFO] as JObject );

	}

}
