using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using GameToolkit.Localization;


#if UNITY_EDITOR
using UnityEditor;
#endif



[System.Serializable]
public class LocalizedString {

    [SerializeField]
    private string text = "";

    [SerializeField]
    private LocalizedText localizedText = null;



    public string Text {
        get { return localizedText ? localizedText.Value : text; }
    }




    public static implicit operator LocalizedString( string text ) => new LocalizedString() { text = text };
    public static implicit operator LocalizedString( LocalizedText localizedText ) => new LocalizedString() { localizedText = localizedText };

    public static implicit operator string( LocalizedString localizedString ) => localizedString.Text;


#if UNITY_EDITOR

    [SerializeField]
    private bool isLocalized = false;



    [CustomPropertyDrawer( typeof( LocalizedString ), true )]
    public class LocalizedStringEditor : PropertyDrawer {

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            EditorGUI.BeginProperty( position, label, property );


            SerializedProperty propertyText = property.FindPropertyRelative( "text" );
            SerializedProperty propertyLocalizedText = property.FindPropertyRelative( "localizedText" );
            SerializedProperty propertyIsLocalized = property.FindPropertyRelative( "isLocalized" );


            using( new EditorGUILayout.HorizontalScope() ) {

                EditorGUILayout.LabelField( propertyIsLocalized.boolValue == true ? string.Format( "{0} (localized)", property.displayName ) : property.displayName, GUILayout.Width( EditorGUIUtility.labelWidth ) );
                propertyIsLocalized.boolValue = EditorGUILayout.Toggle( propertyIsLocalized.boolValue, GUILayout.Width( 14 ) );

                if( propertyIsLocalized.boolValue == false ) {
                    propertyText.stringValue = EditorGUILayout.TextField( propertyText.stringValue );
                    propertyLocalizedText.objectReferenceValue = null;

                } else {
                    propertyText.stringValue = "";
                    propertyLocalizedText.objectReferenceValue = EditorGUILayout.ObjectField( propertyLocalizedText.objectReferenceValue, typeof( LocalizedText ), false ) as LocalizedText;
                }
            }

            if( propertyLocalizedText.objectReferenceValue != null ) {
                GUI.enabled = false;
                EditorGUILayout.TextField( " ", ((LocalizedText)propertyLocalizedText.objectReferenceValue).GetLocaleValue( SystemLanguage.Russian ) );
                GUI.enabled = true;
            }


            EditorGUI.EndProperty();


            if( GUI.changed ) {
                EditorUtility.SetDirty( property.serializedObject.targetObject );
                property.serializedObject.ApplyModifiedProperties();
            }
            
        }


        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
            return 0;
        }


    }

#endif

}