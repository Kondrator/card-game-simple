using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



[System.Serializable]
public class Duration {

    [SerializeField]
    private ulong milliseconds = 0;



    public ulong Milliseconds { get { return milliseconds; } }

    public ulong Seconds { get { return Milliseconds / 1000; } }

    public ulong Minutes { get { return Seconds / 60; } }

    public ulong Hours { get { return Minutes / 60; } }




    public Duration( ulong milliseconds ) {
        this.milliseconds = milliseconds;
    }



    public static implicit operator ulong( Duration duration ) => duration.milliseconds;
    public static implicit operator Duration( ulong milliseconds ) => new Duration( milliseconds );





#if UNITY_EDITOR

    [System.Serializable]
    private enum Type {
        Milliseconds = 0,
        Seconds = 1,
        Minutes = 2,
        Hours = 3,
    }

    [SerializeField]
    private Type type = Type.Milliseconds;

    [SerializeField]
    private bool isVisibleInfo = false;



    [CustomPropertyDrawer( typeof( Duration ), true )]
	private class DurationDrawer : PropertyDrawer {


        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            SerializedProperty propertyMilliseconds = property.FindPropertyRelative( "milliseconds" );
            SerializedProperty propertyType = property.FindPropertyRelative( "type" );
            SerializedProperty propertyIsVisibleInfo = property.FindPropertyRelative( "isVisibleInfo" );

            try {

                using( new EditorGUILayout.VerticalScope( propertyIsVisibleInfo.boolValue ? MyOperationEditor.StyleBox : GUIStyle.none ) ) {
                    using( new EditorGUILayout.HorizontalScope() ) {
                        EditorGUILayout.PrefixLabel( label );

                        switch( (Type)propertyType.enumValueIndex ) {

                            case Type.Milliseconds:
                                propertyMilliseconds.longValue = EditorGUILayout.LongField( propertyMilliseconds.longValue );
                                break;

                            case Type.Seconds:
                                propertyMilliseconds.longValue = EditorGUILayout.LongField( propertyMilliseconds.longValue / 1000 ) * 1000;
                                break;

                            case Type.Minutes:
                                propertyMilliseconds.longValue = EditorGUILayout.LongField( propertyMilliseconds.longValue / 1000 / 60 ) * 1000 * 60;
                                break;

                            case Type.Hours:
                                propertyMilliseconds.longValue = EditorGUILayout.LongField( propertyMilliseconds.longValue / 1000 / 60 / 60 ) * 1000 * 60 * 60;
                                break;

                        }

                        propertyType.enumValueIndex = (int)(Type)EditorGUILayout.EnumPopup( (Type)propertyType.enumValueIndex, GUILayout.Width( 90 ) );
                        propertyIsVisibleInfo.boolValue = EditorGUILayout.Toggle( propertyIsVisibleInfo.boolValue, GUILayout.Width( 20 ) );
                    }

                    if( propertyIsVisibleInfo.boolValue == true ) {
                        using( new EditorGUILayout.VerticalScope() ) {
                            GUI.enabled = false;
                            DrawInfo( Type.Milliseconds, propertyMilliseconds.longValue );
                            DrawInfo( Type.Seconds, propertyMilliseconds.longValue );
                            DrawInfo( Type.Minutes, propertyMilliseconds.longValue );
                            DrawInfo( Type.Hours, propertyMilliseconds.longValue );
                            GUI.enabled = true;
                        }
                    }
                }

            } catch { }


            if( GUI.changed ) {
                property.serializedObject.ApplyModifiedProperties();
            }

        }

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
			return 0;
		}




        private void DrawInfo( Type type, long milliseconds ) {

            long result = milliseconds;

            switch( type ){
                case Type.Seconds:  result = result / 1000;             break;
                case Type.Minutes:  result = result / 1000 / 60;        break;
                case Type.Hours:    result = result / 1000 / 60 / 60;   break;
            }

            if( result == 0 ) {
                return;
            }

            EditorGUILayout.TextField( type.ToString(), result.ToStringSplitSymbol() );

        }

	}
#endif

}