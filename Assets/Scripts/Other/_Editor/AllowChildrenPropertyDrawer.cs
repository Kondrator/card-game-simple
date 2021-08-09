using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif




public class OnlyChildrenAttribute : PropertyAttribute {}



#if UNITY_EDITOR

[CustomPropertyDrawer( typeof( OnlyChildrenAttribute ) )]
public class AllowChildrenPropertyDrawer : PropertyDrawer {

    private Type typeGameObject = null;
    private Type TypeGameObject {
        get {
            return typeGameObject = typeGameObject ?? typeof( GameObject );
        }
    }

    private Type typeComponent = null;
    private Type TypeComponent {
        get {
            return typeComponent = typeComponent ?? typeof( Component );
        }
    }




    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

        // draw object
        switch( property.propertyType ) {

            case SerializedPropertyType.ObjectReference:
                EditorGUILayout.PropertyField( property );
                break;

            default:
                EditorGUILayout.HelpBox(    string.Format(  "{0}\r\n{1}\r\nIncorrent type: only object reference",
                                                            typeof( OnlyChildrenAttribute ).Name.ToStringSplitWords(),
                                                            label.text
                                                        ),
                                            MessageType.Error
                                            
                                        );
                return;

        }


        // check have value
        if( property.objectReferenceValue == null ) {
            return;
        }


        // get need params
        Type typeValue = property.objectReferenceValue?.GetType();
        Transform transformValue = null;
        Component componentTarget = property.serializedObject.targetObject as Component;


        // get transform of target value
        if( typeValue == TypeGameObject ) {
            transformValue = (property.objectReferenceValue as GameObject).transform;

        } else if( typeValue.IsSubclassOf( TypeComponent ) ){
            transformValue = (property.objectReferenceValue as Component).transform;
        }


        // check transform value in children
        if( transformValue != null ) {
            Transform[] childrens = componentTarget.GetComponentsInChildren<Transform>();
            childrens = childrens.RemoveItem( System.Array.IndexOf( childrens, componentTarget.transform ) );
            if( childrens.Contains( transformValue ) == false ) {
                EditorUtility.DisplayDialog(    componentTarget.GetType().Name.ToStringSplitWords(),
                                                string.Format( "Put {0} from children", label.text ),
                                                "Close"
                                            );
                property.objectReferenceValue = null;

                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty( property.serializedObject.targetObject );
            }
        }

    }



    public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
        return 0;
    }

}

#endif