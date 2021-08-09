using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
 
[CustomPropertyDrawer(typeof(Sprite))]
public class SpriteDrawer : PropertyDrawer {
 
	private static GUIStyle s_TempStyle = new GUIStyle();
 
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ){                                    
		var ident = EditorGUI.indentLevel;
		//EditorGUI.indentLevel = 0;
 
		Rect spriteRect;
     
		//create object field for the sprite
		spriteRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		EditorGUI.PropertyField( spriteRect, property );
		
 
		//if this is not a repain or the property is null exit now
		if( IsVisibleSprite( property ) ){
			return;
		}

		//draw a sprite
		Sprite sprite = property.objectReferenceValue as Sprite;
		
		spriteRect.y += EditorGUIUtility.singleLineHeight + 4;
		spriteRect.height = 64;
		spriteRect.width = spriteRect.height * (sprite.rect.width / sprite.rect.height);

		MyOperationEditor.DrawTexturePreview( spriteRect, sprite );
             
		EditorGUI.indentLevel = ident;
	}


 
	public override float GetPropertyHeight( SerializedProperty property, GUIContent label ){

		if( IsVisibleSprite( property ) ){
			return base.GetPropertyHeight( property, label );
		}

		return base.GetPropertyHeight( property, label ) + 70f;
	}



	private bool IsVisibleSprite( SerializedProperty property ){
		return	property.objectReferenceValue == null
				|| property.hasMultipleDifferentValues == true;
	}

}
