using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer{

	public override void OnGUI( Rect _position, SerializedProperty _property, GUIContent _label ){

		_property.intValue = EditorGUI.MaskField( _position, _label, _property.intValue, _property.enumNames );

	}

}

/*
							--- HOW TO USE ---

	[System.Flags]
	public enum MyMaskedEnum{
		Flag0 =		(1 << 0),
		Flag1 =		(1 << 1),
		Flag2 =		(1 << 2),
		Flag3 =		(1 << 3)
	}
 
	class MyObject : MonoBehaviour{
		
		[SerializeField] [EnumFlagsAttribute]
		MyMaskedEnum m_flags;
		
	}

							--- HOW TO USE ---

 */

#endif