using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class SpriteResourceRenderer : MonoBehaviour {

    [SerializeField]
    private ResourceSprite sprite = null;

    [SerializeField]
    private int orderInLayer = 0;
    public int OrderInLayer { get { return orderInLayer; } set { orderInLayer = value; } }




    private void Awake() {

#if UNITY_EDITOR
        if( Application.isPlaying == false ) {
            return;
        }
#endif

        if( sprite != null ) {
            sprite.Get( sprite => {
                if( sprite != null ) {
                    SpriteRenderer component = this.gameObject.AddComponent<SpriteRenderer>();
                    component.sprite = sprite;
                    component.sortingOrder = orderInLayer;
                }
                Destroy( this );
            } );

        } else {
            Destroy( this );
        }
    }






#if UNITY_EDITOR

    private void OnEnable() {
        UnityEditor.SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable() {
        UnityEditor.SceneView.duringSceneGui -= OnSceneGUI;
    }


    void OnSceneGUI( UnityEditor.SceneView sceneView ) {

        if( Event.current.type == EventType.KeyDown ) {

            switch( Event.current.keyCode ) {

                case KeyCode.F3:
                    this.AddComponent<SpriteRenderer>().sprite = this.sprite.GetEditor();
                    break;

                case KeyCode.F4:
                    SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
                    if( renderer != null ) {
                        DestroyImmediate( renderer );
                    }
                    break;

            }

        }

    }





    [CustomEditor( typeof( SpriteResourceRenderer ) )]
    private class SpriteResourceRendererEditor : Editor<SpriteResourceRenderer> {


        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            SpriteRenderer renderer = component.GetComponent<SpriteRenderer>();
            Sprite sprite = component.sprite.GetEditor();

            if( sprite == null ) {
                if( renderer != null ) {
                    DestroyImmediate( renderer );
                }
                return;
            }

            if( renderer != null
                && renderer.sprite != sprite
            ) {
                renderer.sprite = sprite;
            }


            EditorGUILayout.Space();

            using( new EditorGUILayout.VerticalScope( MyOperationEditor.StyleBox ) ) {

                MyOperationEditor.DrawTitle( "Preview (key = Tilde)" );
                EditorGUILayout.Space();

                using( new EditorGUILayout.HorizontalScope() ) {
                    EditorGUILayout.HelpBox( "F3 - show", MessageType.Info );
                    EditorGUILayout.HelpBox( "F4 - hide", MessageType.Info );
                }
                EditorGUILayout.Space();

                if( MyOperationEditor.DrawButtonMini( renderer == null ? "show" : "hide" ) ) {

                    if( renderer == null ) {
                        renderer = component.AddComponent<SpriteRenderer>();
                        renderer.sprite = component.sprite.GetEditor();

                    }else{
                        DestroyImmediate( renderer );
                    }

                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty( component );

                }

            }

        }

    }
#endif

}
