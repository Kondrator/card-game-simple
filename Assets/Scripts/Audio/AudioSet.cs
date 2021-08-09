using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AudioSet{

	[SerializeField]
	private ResourceAudioClip[] clips = null;


	/// <summary>
	/// Count clips in set.
	/// </summary>
	public int Count{
		get{
			if( clips != null ){
				return clips.Length;
			}
			return 0;
		}
	}


	/// <summary>
	/// Get clip from set by index.
	/// </summary>
	public ResourceAudioClip this[int index]{
		get{
			if( index >= 0 && index < Count ){
				return clips[index];
			}
			return null;
		}
	}



	/// <summary>
	/// Get random clip in set.
	/// </summary>
	public ResourceAudioClip GetRandom( AudioClip ignore = null ){
		
		int count = Count;
		if( count > 0 ){
			if( count == 1 ){
				return clips[0];
			}

			ResourceAudioClip clipGet = null;
			do{
				clipGet = clips[Random.Range( 0, count )];
			}while( clipGet.Name == ignore?.name );
			return clipGet;
		}

		return null;
	}


	/// <summary>
	/// Get random clip in set.
	/// </summary>
	public void GetRandom( System.Action<AudioClip> callback, AudioClip ignore = null ) {
		GetRandom( ignore )?.Get( callback.Invoke );
	}




	public void PreGet() {
		for( int i = 0; i < Count; i++ ) {
			this[i].PreGet();
		}
	}

}
