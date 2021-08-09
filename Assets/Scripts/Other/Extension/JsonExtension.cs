using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

public static class JsonExtension{

	/// <summary>
	/// Convert JSON object to Vector3.
	/// return Vector3.zero -> if object not have fields "x", "y", "z" and they not int.
	/// </summary>
	/// <returns></returns>
	public static Vector3 ToVector3( this JObject target ){
		return ((JToken)target).ToVector3();
	}
	/// <summary>
	/// Convert JSON object to Vector3.
	/// return Vector3.zero -> if object not have fields "x", "y", "z" and they not int.
	/// </summary>
	/// <returns></returns>
	public static Vector3 ToVector3( this JToken target ){
		
		Vector3 vector = Vector3.zero;

		try{
			//string[] values = target["v"].ToString().Split( '|' );
			//vector.x = (float)System.Convert.ToDouble( values[0] );
			//vector.y = (float)System.Convert.ToDouble( values[1] );
			//vector.z = (float)System.Convert.ToDouble( values[2] );
			
			// TODO
			vector.x = (float)target["x"];
			vector.y = (float)target["y"];
			vector.z = (float)target["z"];
		}catch{}

		return vector;
	}

	


	/// <summary>
	/// Convert JSON object to Vector2.
	/// return Vector2.zero -> if object not have fields "x", "y" and they not int.
	/// </summary>
	/// <returns></returns>
	public static Vector2 ToVector2( this JObject target ){
		return ((JToken)target).ToVector2();
	}
	/// <summary>
	/// Convert JSON object to Vector2.
	/// return Vector2.zero -> if object not have fields "x", "y" and they not int.
	/// </summary>
	/// <returns></returns>
	public static Vector2 ToVector2( this JToken target ){
		
		Vector2 vector = Vector2.zero;

		try{
			//string[] values = target["v"].ToString().Split( '|' );
			//vector.x = (float)System.Convert.ToDouble( values[0] );
			//vector.y = (float)System.Convert.ToDouble( values[1] );
			
			// TODO
			vector.x = (float)target["x"];
			vector.y = (float)target["y"];
		}catch{}

		return vector;
	}






	/// <summary>
	/// Convert Vector3 to JSON object.
	/// </summary>
	public static JObject ToJObject( this Vector3 target ){
		
		JObject json = new JObject();
		
		//json["v"] = target.x.ToStringMini( 3 ) + "|" + target.y.ToStringMini( 3 ) + "|" + target.z.ToStringMini( 3 );
		json["x"] = target.x.ToStringMini( 3 );
		json["y"] = target.y.ToStringMini( 3 );
		json["z"] = target.z.ToStringMini( 3 );

		return json;
	}




	public static T GetValue<T>( this JToken target, T valueDefault ){
		
		T value = valueDefault;

		try{
			value = target.Value<T>();
		}catch{
			return valueDefault;
		}

		return value;
	}






	/// <summary>
	/// Read field in target with name "success" and return her value in BOOL.
	/// </summary>
	public static bool IsSuccess( this JToken target, bool succesfullyDefault = false ){
		
		string key = "success";

		if( target != null && target[key] != null ){
			return (bool)target[key];
		}

		return succesfullyDefault;
	}


	/// <summary>
	/// Convert JSON to string and remove all [\r\n		].
	/// </summary>
	public static string ToStringFix( this JContainer target ){
		
		string data = target.ToString();

		return data.Replace( "\r", "" ).Replace( "\n", "" ).Replace( " ", "" ).Replace( "	", "" );
	}














	public static JArray ToJArray<T>( this object dataInfo, List<T> items ) where T : DataInfo{
		
		JArray array = new JArray();
		if( items == null ){
			return array;
		}

		for( int i = 0; i < items.Count; i++ ){
			array.Add( items[i].ToJSON() );
		}

		return array;
	}
	public static JArray ToJArray<T>( this object dataInfo, T[] items ) where T : DataInfo{
		return ToJArray( dataInfo, new List<T>( items ) );
	}
	public static List<T> FromJArray<T>( this object dataInfo, JArray array, List<T> list = null ) where T : DataInfo{
		
		if( list == null ){
			list = new List<T>();
		}

		List<T> items = new List<T>();
		if( array == null ){
			return list;
		}

		for( int i = 0; i < array.Count; i++ ){
			T item = MyOperationClass.GetInstance<T>( array[i] as JObject );
			items.Add( item );
		}

		return items;
	}






	public static JArray ToJDictionary<T>( this object dataInfo, Dictionary<string, T> items, Func<T, JToken> convert ) {

		JArray array = new JArray();
		if( items == null ) {
			return array;
		}

		foreach( string key in items.Keys ) {
			JObject item = new JObject();
			item["key"] = key;
			item["value"] = convert( items[key] );
			array.Add( item );
		}

		return array;
	}
	public static Dictionary<string, T> FromJDictionary<T>( this object dataInfo, JArray array, Func<JToken, T> convert, Dictionary<string, T> dictionary = null ) {

		if( dictionary == null ) {
			dictionary = new Dictionary<string, T>();
		}

		if( array == null ){
			return dictionary;
		}

		for( int i = 0; i < array.Count; i++ ) {
			JObject item = array[i] as JObject;
			dictionary[item["key"].GetValue( "" )] = convert( item["value"] as JToken );
		}

		return dictionary;
	}






	public delegate int DDataInfoGetID<T>( T data );
	public delegate T DDataInfoCreate<T>( int id );
	public static JArray ToJArrayIDs<T>( this object dataInfo, List<T> items, DDataInfoGetID<T> callback ){
		
		JArray array = new JArray();
		if( items == null ){
			return array;
		}

		for( int i = 0; i < items.Count; i++ ){
			array.Add( callback( items[i] ) );
		}

		return array;
	}
	public static JArray ToJArrayIDs<T>( this object dataInfo, T[] items, DDataInfoGetID<T> callback ){
		return ToJArrayIDs<T>( dataInfo, new List<T>( items ), callback );
	}
	public static List<T> FromJArrayIDs<T>( this object dataInfo, JArray array, DDataInfoCreate<T> callback ){
		
		List<T> items = new List<T>();
		if( array == null
			|| array.Count == 0
		){
			return items;
		}

		for( int i = 0; i < array.Count; i++ ){
			int id = array[i].GetValue( -1 );
			if( id != -1 ){
				items.Add( callback( id ) );
			}
		}

		return items;
	}







}
