using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif


public class GameObjectInstantiate : MonoBehaviour {

	[SerializeField]
	private GameObject target = null;

	private GameObject instance = null;


	void OnEnable(){
		if( instance == null ){
			instance = PoolGameObject.Get( target, this.transform );
			instance.transform.ResetTransform();
		}
	}

	void OnDisable(){
		if( instance != null ){
			instance.Deactivate();
			instance = null;
		}
	}




#if UNITY_EDITOR
	[CustomEditor(typeof(GameObjectInstantiate))]
	private class GameObjectInstantiateEditor : Editor {
		
		private GameObjectInstantiate go = null;
		void OnEnable(){
			go = this.target as GameObjectInstantiate;
		}

		public override void OnInspectorGUI(){
			base.OnInspectorGUI();

			if( Application.isPlaying == false
				&& go.target != null
			){
				if( MyOperationEditor.DrawButtonMini( "Instantiate (test)" ) ){
					MyOperation.Instantiate( go.target, go.transform );
				}
			}
		}

	}
#endif

}
