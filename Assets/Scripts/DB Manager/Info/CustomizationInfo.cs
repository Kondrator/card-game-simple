using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;


public class CustomizationInfo : DataInfo {


	private int idTableBackground = 0;
	public int IDTableBackground { get => idTableBackground; set => idTableBackground = value; }

	private int idCardBack = 0;
	public int IDCardBack { get => idCardBack; set => idCardBack = value; }

	private int idCardFront = 0;
	public int IDCardFront { get => idCardFront; set => idCardFront = value; }




	public CustomizationInfo(){}
	public CustomizationInfo( JObject data ) :base( data ){}








	public const string KEY_INFO = "customization";

	private const string KEY_TABLE_BG = "id.table.bg";
	private const string KEY_CARD_BACK = "id.card.back";
	private const string KEY_CARD_FRONT = "id.card.front";


	public override JObject ToJSON(){
		
		JObject data = new JObject();


		data[KEY_TABLE_BG] = IDTableBackground;
		data[KEY_CARD_BACK] = IDCardBack;
		data[KEY_CARD_FRONT] = IDCardFront;

		return data;
	}

	public override void FromJSON( JObject data ){

		if( data == null ){
			return;
		}

		IDTableBackground = data[KEY_TABLE_BG].GetValue( IDTableBackground );
		IDCardBack = data[KEY_CARD_BACK].GetValue( IDCardBack );
		IDCardFront = data[KEY_CARD_FRONT].GetValue( IDCardFront );

	}


}
