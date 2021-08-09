using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif



public class ResourceGameObjectInstantiate : MonoBehaviour {

	[SerializeField]
	private ResourceGameObject target = null;

	private GameObject instance = null;


	void Start() {
		if( instance == null ) {
			target?.Get( ( GameObject go ) => {
				instance = PoolGameObject.Get( go, this.transform.parent );
				instance.transform.ResetTransform();
				instance.transform.SetSiblingIndex( this.transform.GetSiblingIndex() );
				Destroy( this.gameObject );
			} );
		}
	}



#if UNITY_EDITOR
	[CustomEditor( typeof( ResourceGameObjectInstantiate ) )]
	private class ResourceGameObjectInstantiateEditor : Editor<ResourceGameObjectInstantiate> {


		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if( Application.isPlaying == false
				&& component.target != null
			) {

				using( new EditorGUILayout.HorizontalScope() ) {

					if( MyOperationEditor.DrawButtonMini( "Instantiate (test)" ) ) {
						MyOperation.Instantiate( component.target.GetEditor(), component.transform );
					}

					if( MyOperationEditor.DrawButtonMini( "Clear" ) ) {
						MyOperation.DestroyAllChild( component.transform, true );
					}

				}

				if( GUI.changed ) {
					EditorUtility.SetDirty( component );
				}
				
			}
		}

	}
#endif

}
