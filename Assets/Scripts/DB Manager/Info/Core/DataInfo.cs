using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public abstract class DataInfo{



	public DataInfo(){}
	public DataInfo( JObject data ){
		FromJSON( data );
	}
	

	/*
	protected delegate JObject DToJSON<T>( T item ) where T : DataInfo;
	/// <summary>
	/// Convert list items to JArray.
	/// </summary>
	protected JArray ToJArray<T>( List<T> items, DToJSON<T> toJSON ) where T : DataInfo{
		
		JArray array = new JArray();

		for( int i = 0; i < items.Count; i++ ){
			array[i] = toJSON( items[i] );
		}

		return array;
	}


	protected delegate T DFromJSON<T>( JObject data ) where T : DataInfo;
	/// <summary>
	/// Convert JArray to list items.
	/// </summary>
	protected List<T> FromJArray<T>( JArray array, DFromJSON<T> fromJSON ) where T : DataInfo{
		
		List<T> items = new List<T>();

		for( int i = 0; i < array.Count; i++ ){
			items.Add( fromJSON( (JObject)array[i] ) );
		}

		return items;
	}
	*/


	/*
	protected JArray ToJArray<T>( List<T> items ) where T : DataInfo{
		
		JArray array = new JArray();
		if( items == null ){
			return array;
		}

		for( int i = 0; i < items.Count; i++ ){
			array.Add( items[i].ToJSON() );
		}

		return array;
	}
	protected JArray ToJArray<T>( T[] items ) where T : DataInfo{
		return ToJArray( new List<T>( items ) );
	}
	protected List<T> FromJArray<T>( JArray array, List<T> list = null ) where T : DataInfo{
		
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





	
	protected delegate int DDataInfoGetID<T>( T data );
	protected delegate T DDataInfoCreate<T>( int id );
	protected JArray ToJArrayIDs<T>( List<T> items, DDataInfoGetID<T> callback ){
		
		JArray array = new JArray();
		if( items == null ){
			return array;
		}

		for( int i = 0; i < items.Count; i++ ){
			array.Add( callback( items[i] ) );
		}

		return array;
	}
	protected JArray ToJArrayIDs<T>( T[] items, DDataInfoGetID<T> callback ){
		return ToJArrayIDs<T>( new List<T>( items ), callback );
	}
	protected List<T> FromJArrayIDs<T>( JArray array, DDataInfoCreate<T> callback ){
		
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
	*/



	









	
	/// <summary>
	/// Generate this class to JObject.
	/// </summary>
	public abstract JObject ToJSON();

	/// <summary>
	/// Get data to this class from JObject.
	/// </summary>
	public abstract void FromJSON( JObject data );


	/// <summary>
	/// String with JSON data of this class.
	/// </summary>
	public override string ToString(){
		
		return ToJSON().ToString();
	}

}