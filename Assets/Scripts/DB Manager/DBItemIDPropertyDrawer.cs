using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif



[Obsolete( "DBItemID is deprecated, please use DBID<T> or DBGroup<T> instead." )]
[AttributeUsage( AttributeTargets.Field, AllowMultiple = false, Inherited = true )]
public class DBItemIDAttribute : PropertyAttribute {

    
    private Type typeItem = null;
    public Type TypeItem { get { return typeItem; } }
    public Type TypeDBManager { get { return typeItem.GetTypeBase<DBItem<DBManager>>().GetGenericArguments()[0]; } }

    private GUIContent label = null;
    public GUIContent Label { get { return label; } }


    public DBItemIDAttribute( Type typeItem ) {
        SetTypeItem( typeItem );
    }

    public DBItemIDAttribute( Type typeItem, string label ) {
        SetTypeItem( typeItem );

        if( string.IsNullOrEmpty( label ) ) {
            this.label = GUIContent.none;

        } else {
            this.label = new GUIContent( label );
        }
    }



    private void SetTypeItem( Type typeItem ) {
        this.typeItem = typeItem;

        if( MyOperationClass.GetTypeBase<DBItem<DBManager>>( typeItem ) == null ) {
            this.typeItem = typeof( DBItem<DBManager> );
        }
    }

}




#if UNITY_EDITOR

[CustomPropertyDrawer( typeof(DBItemIDAttribute) )]
public class DBItemIDAttributePropertyDrawer : PropertyDrawer {


    private new DBItemIDAttribute attribute = null;
    private DBItemIDAttribute Attribute { get { return attribute = attribute ?? base.attribute as DBItemIDAttribute; } }

    private DBItemSelectDrawer drawer = null;
    private DBItemSelectDrawer Drawer { get { return drawer = drawer ?? new DBItemSelectDrawer( Attribute.Label, Attribute.TypeDBManager, Attribute.TypeItem ); } }


    
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ){

        if( property.type == typeof(DBGroup).FullName ) {
            EditorGUILayout.LabelField( "ok" );
            return;
        }

        Drawer.OnGUI( position, property, label );
    }


    public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {

        if( property.type == typeof( DBGroup ).FullName ) {
            return 0;
        }

        return Drawer.GetPropertyHeight( property );
    }

}




public class DBItemSelectDrawer {

    private GUIContent label = null;
    private GUIContent Label { get { return label; } }

    private Type typeDBManager = null;
    private Type TypeDBManager { get { return typeDBManager; } }

    private Type typeDBItem = null;
    private Type TypeDBItem { get { return typeDBItem; } }






    private DBManager dbManager = null;
    private DBManager DBManager { get { return dbManager = dbManager ?? DBManager.LoadAssset( typeDBManager ); } }

    private List<DBItem> items = null;
    private List<DBItem> Items { get { return items = items ?? DBManager.GetList( typeDBItem ); } }


    private GUIStyle styleName = null;
    private GUIStyle StyleName {
        get {
            if( styleName == null ) {
                styleName = new GUIStyle( "label" );
                styleName.fontStyle = FontStyle.Bold;
            }
            return styleName;
        }
    }



    public DBItemSelectDrawer( Type typeDBManager, Type typeDBItem ) {
        this.typeDBManager = typeDBManager;
        this.typeDBItem = typeDBItem;
    }

    public DBItemSelectDrawer( GUIContent label, Type typeDBManager, Type typeDBItem ) {
        this.typeDBManager = typeDBManager;
        this.typeDBItem = typeDBItem;
    }



    private DBItem GetItem( int id ) {
        return Items.Find( value => value.ID == id );
    }




    public void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

        try {
            DBItem item = GetItem( property.intValue );


            // label
            Rect rectLabel = new Rect( position.x, position.y, EditorGUIUtility.labelWidth, position.height );
            if( Label == GUIContent.none
                || label == GUIContent.none
            ) {
                rectLabel.width = 0;

            } else {
                GUI.Label( rectLabel, Label != null ? new GUIContent( Label ) : label );
            }


            // button select (rect)
            Rect rectButtonSelect = new Rect( position.x + rectLabel.width, position.y, position.width - rectLabel.width, EditorGUIUtility.singleLineHeight );
            Rect? rectButtonSelectPrefab = null;

            // item
            if( item != null ) {

                // icon
                Rect rectIcon = new Rect( position.x + rectLabel.width + 5, position.y + 5, position.height - 10, position.height - 10 );
                MyOperationEditor.DrawTexturePreview( rectIcon, item.IconUI.GetEditor() );

                // title
                Rect rectTitle = new Rect( rectIcon.x + rectIcon.width + 5, position.y, position.width - rectLabel.width - rectIcon.width - 10, EditorGUIUtility.singleLineHeight );
                GUI.Label( rectTitle, item.Name, StyleName );

                // description
                Rect rectDescripton = new Rect( rectIcon.x + rectIcon.width + 5, position.y + 15, position.width - rectLabel.width - rectIcon.width - 10, EditorGUIUtility.singleLineHeight );
                GUI.Label( rectDescripton, item.Description );

                // fix button select (rect)
                rectButtonSelect = new Rect( rectIcon.x + rectIcon.width + 5, position.y + 30, position.width - rectLabel.width - rectIcon.width - 35, EditorGUIUtility.singleLineHeight );
                rectButtonSelectPrefab = new Rect( rectButtonSelect.x + rectButtonSelect.width, rectButtonSelect.y, 25, rectButtonSelect.height );

            }

            // button select
            if( GUI.Button( rectButtonSelect, "select", EditorStyles.miniButton ) ) {
                EditorWindowSelect.Show(
                    string.Format( "Select {0}", Label != null ? Label.text : TypeDBItem.ToString().ToStringSplitWords() ),
                    Items.Select( value => new EditorWindowSelect.Item( value.ID, value.Name, value.IconUI.GetEditor(), value.Description, AssetDatabase.GetAssetPath( value ) ) ).ToArray(),
                    ( EditorWindowSelect.Item value ) => {
                        property.intValue = value.ID;
                        property.serializedObject.ApplyModifiedProperties();
                    },
                    property.intValue
                );
            }

            // button select prefab in project
            if( rectButtonSelectPrefab.HasValue == true
                && GUI.Button( rectButtonSelectPrefab.Value, "〇", EditorStyles.miniButton )
            ) {
                MyOperationEditor.InspectTarget( item.gameObject );
            }
            
        } catch( Exception exc ) {
            //Debug.LogError( exc.Message + "\r\n" + exc.StackTrace );
        }
    }


    public void OnGUI( Rect position, SerializedProperty property ) {
        OnGUI( position, property, new GUIContent( TypeDBItem.Name.ToStringSplitWords() ) );
    }




    public void OnGUI( SerializedProperty property, GUIContent label ) {

        float height = GetPropertyHeight( property );

        using( new EditorGUILayout.VerticalScope( GUILayout.Height( height ) ) ) {
            Rect rect = EditorGUILayout.GetControlRect();
            rect.height = height;

            OnGUI( rect, property, label );
        }
    }

    public void OnGUI( SerializedProperty property ) {
        OnGUI( property, new GUIContent( TypeDBItem.Name.ToStringSplitWords() ) );
    }




    public float GetPropertyHeight( SerializedProperty property ) {

        DBItem item = GetItem( property.intValue );
        if( item != null ) {
            return EditorGUIUtility.singleLineHeight * 3;
        }

        return EditorGUIUtility.singleLineHeight;
    }

}


public class DBItemEditorSelect<TDBManager, TDBitem> : DBItemSelectDrawer where TDBManager : DBManager where TDBitem : DBItem {

    public DBItemEditorSelect() :base( typeof(TDBManager), typeof(TDBitem) ) {
        
    }

}


#endif