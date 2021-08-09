using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DictionaryInfo : DataInfo {

	/// <summary>
	/// Extra parameters for specified items
	/// </summary>
	private Dictionary<string, JValue> Dict { get { return dict = dict ?? new Dictionary<string, JValue>(); } }
	private Dictionary<string, JValue> dict = null;




	public DictionaryInfo() { }
	public DictionaryInfo( JObject data ) : base( data ) { }




	/// <summary>
	/// Set extra parameter
	/// </summary>
	/// <param name="key">Name of parameter</param>
	/// <param name="value">Value of parameter</param>
	public void Set( string key, object value ) {
		Dict[key] = new JValue( value );
	}



	/// <summary>
	/// Get extra parameter
	/// </summary>
	/// <param name="key">Name of parameter</param>
	public T Get<T>( string key, T defaultValue ) {
		if( Dict.ContainsKey( key ) == false ) {
			return defaultValue;
		}

		return Dict[key].GetValue( defaultValue );
	}




	/// <summary>
	/// All extra parameters equals
	/// </summary>
	public bool Equals( DictionaryInfo other ) {

		List<string> keys = Dict.Keys.ToList();

		if( keys.Count != other.Dict.Keys.Count ) {
			return false;
		}

		for( int i = 0; i < keys.Count; i++ ) {
			if( other.Dict.ContainsKey( keys[i] ) == false ) {
				return false;
			}

			if( Dict[keys[i]].Equals( other.Dict[keys[i]] ) == false ) {
				return false;
			}
		}

		return true;
	}





	private const string KEY_DATA = "data";


	public override JObject ToJSON() {

		JObject data = new JObject();

		data[KEY_DATA] = dict.ToJDictionary( dict, value => value );

		return data;
	}

	public override void FromJSON( JObject data ) {

		if( data == null ) {
			return;
		}

		dict = dict.FromJDictionary( data[KEY_DATA] as JArray, value => value as JValue ?? JValue.CreateNull(), dict );

	}

}
