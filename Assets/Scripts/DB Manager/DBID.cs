using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif




[System.Serializable]
public class DBID<T> : DBID where T : DBItem {

    [SerializeField]
    [HideInInspector]
    private int id = 0;


    private int ID {
        get { return id; }
#if UNITY_EDITOR
        set { id = value; }
#endif
    }



    public static implicit operator int( DBID<T> dbid ) => dbid.id;
    public static implicit operator DBID<T>( int id ) => new DBID<T>() { id = id };


    public static bool operator == ( DBID<T> a, DBID<T> b ) {
        return a.ID == b.ID;
    }

    public static bool operator != ( DBID<T> a, DBID<T> b ) {
        return a == b == false;
    }




    public override bool Equals( object obj ) {
        return base.Equals( obj );
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }






    public T GetItem( DBManager dbManager ) {
        return dbManager.Get<T>( id );
    }


    public int CompareTo( int id ) {
        return this.id.CompareTo( id );
    }

}




[System.Serializable]
public abstract class DBID {



#if UNITY_EDITOR



	[CustomPropertyDrawer( typeof(DBID), true )]
	private class DBIDPropertyDrawer : PropertyDrawer {
        
		private DBItemSelectDrawer drawer = null;
		private DBItemSelectDrawer Drawer( System.Type type ) {

            if( drawer == null ) {
                System.Type[] typesGeneric = type.GetGenericArguments();

                System.Type typeDBItem = typesGeneric[0];
                System.Type typeDBManager = typeDBItem.GetTypeBase<DBItem<DBManager>>().GetGenericArguments()[0];

                drawer = new DBItemSelectDrawer( typeDBManager, typeDBItem );
            }

			return drawer;
		}



		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            if( Drawer( fieldInfo.FieldType ) == null ) {
                EditorGUILayout.HelpBox( string.Format( "Inherit class DBID<T>" ), MessageType.Error );
                return;
            }

            SerializedProperty properryID = property.FindPropertyRelative( "id" );

            if( property.name.ToLower() != "id" ) {
                label = new GUIContent( property.name.Replace( "id", "" ).ToStringSplitWords() );
            }

            Drawer( fieldInfo.FieldType ).OnGUI( position, properryID, label );

        }

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
			return Drawer( fieldInfo.FieldType ).GetPropertyHeight( property.FindPropertyRelative( "id" ) );
		}

	}
#endif

}