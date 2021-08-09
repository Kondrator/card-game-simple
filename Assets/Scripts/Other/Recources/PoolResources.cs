using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using System.Text;

public class PoolResources : MonoBehaviour {



	private static PoolResources singleton_;
	private static PoolResources singleton {
		get {
			if( singleton_ == null ) {
				GameObject goNew = new GameObject( typeof( PoolResources ).Name );
				singleton_ = goNew.AddComponent<PoolResources>();
			}
			return singleton_;
		}
	}

	private Dictionary<string, Object> pool;
	private Dictionary<string, Object> Pool {
		get {
			if( pool == null ) {
				pool = new Dictionary<string, Object>();
			}
			return pool;
		}
	}

	private Dictionary<string, List<System.Action<Object>>> loading;
	private Dictionary<string, List<System.Action<Object>>> Loading {
		get {
			if( loading == null ) {
				loading = new Dictionary<string, List<System.Action<Object>>>();
			}
			return loading;
		}
	}






	/// <summary>
	/// Get object by path from resources
	/// </summary>
	/// <typeparam name="T">Type of object</typeparam>
	/// <param name="path">Path to object in resources</param>
	/// <param name="name">Name of object</param>
	/// <param name="callback"></param>
	public static void Get<T>( string path, string name, System.Action<T> callback ) where T : Object {

		// fix path for sprite
		if( typeof(T) == typeof(Sprite) ) {
			// fast generate string "path[name]"
			StringBuilder pathBuilder = new StringBuilder();
			pathBuilder.Append( path );
			pathBuilder.Append( "[" );
			pathBuilder.Append( name );
			pathBuilder.Append( "]" );

			path = pathBuilder.ToString();
		}


		// check have in pool
		if( singleton.Pool.ContainsKey( path ) == true ) {
			T obj = singleton.Pool[path] as T;
			if( obj != null ) {
				callback?.Invoke( obj );
			}
			return;
		}

		// check have in loading
		if( singleton.Loading.ContainsKey( path ) == true ) {
			singleton.Loading[path].Add( ( Object obj ) => callback?.Invoke( obj as T ) );
			return;
		}

		// add to pool loading
		singleton.Loading[path] = new List<System.Action<Object>>();
		singleton.Loading[path].Add( ( Object obj ) => callback?.Invoke( obj as T ) );


		// load asset async
		Addressables.LoadAssetAsync<T>( path )
		.Completed += ( AsyncOperationHandle<T> operation ) => {

			if( operation.Status == AsyncOperationStatus.Succeeded ) {

				// add to pool
				singleton.Pool[path] = operation.Result;

				// callback completed loading
				for( int i = 0; i < singleton.Loading[path].Count; i++ ) {
					Object obj = singleton.Pool[path];
					if( obj != null ) {
						singleton.Loading[path][i]( obj );
					}
				}
				singleton.Loading[path] = null;

			}
		};

	}
}
