using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraSingleton : MonoBehaviour {

	private static new Camera camera = null;
	public static Camera Camera{
		get{
			if( camera == null ){
				camera = Camera.main;
			}
			return camera;
		}
	}


	public static bool IsExist {
		get {
			return Camera != null;
		}
	}


}
