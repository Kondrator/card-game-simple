using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif




[System.Serializable]
public class DBGroup<T> : DBGroup where T : DBItem {

    [SerializeField]
    [HideInInspector]
    private int[] ids = null;


    private int[] IDs {
        get { return ids; }
#if UNITY_EDITOR
        set { ids = value; }
#endif
    }



    public int Count { get { return IDs.Length; } }
    public int this[int index] { get { return IDs[index]; } }


    public static implicit operator int[]( DBGroup<T> group ) => group.ids;
    public static implicit operator DBGroup<T>( int[] ids ) => new DBGroup<T>() { ids = ids };




    public bool Contains( int id ) {
        return ids.Contains( id );
    }


    public int GetRandom() {
        return IDs.RandomItem();
    }

}




[System.Serializable]
public abstract class DBGroup {



#if UNITY_EDITOR

    [SerializeField]
    [HideInInspector]
    private bool isDetail = true;


    [CustomPropertyDrawer( typeof(DBGroup), true )]
	private class DBGroupPropertyDrawer : PropertyDrawer {


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


        private List<DBItem> items = null;
        private List<DBItem> GetItems( System.Type type ) {

            if( items == null ) {
                System.Type[] typesGeneric = type.GetGenericArguments();

                System.Type typeDBItem = typesGeneric[0];
                System.Type typeDBManager = typeDBItem.GetTypeBase<DBItem<DBManager>>().GetGenericArguments()[0];

                return items = items ?? DBManager.LoadAssset( typeDBManager ).GetList( typeDBItem );
            }

            return items;
        }



        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            if( Drawer( fieldInfo.FieldType ) == null ) {
                EditorGUILayout.HelpBox( string.Format( "Inherit class DBGroup<T>" ), MessageType.Error );
                return;
            }

            SerializedProperty propetyIsDetail = property.FindPropertyRelative( "isDetail" );

            SerializedProperty propetyIDs = property.FindPropertyRelative( "ids" );
            int[] ids = new int[propetyIDs.arraySize];
            for( int i = 0; i < ids.Length; i++ ) {
                ids[i] = propetyIDs.GetArrayElementAtIndex( i ).intValue;
            }

            try {

                EditorGUILayout.Space();

                using( new EditorGUILayout.VerticalScope( MyOperationEditor.StyleBox ) ) {

                    using( new EditorGUILayout.HorizontalScope() ) {
                        MyOperationEditor.DrawTitle( string.Format( property.displayName ) );

                        if( MyOperationEditor.DrawButtonMini( propetyIsDetail.boolValue == true ? "detail" : "simple", 50 ) ) {
                            propetyIsDetail.boolValue = !propetyIsDetail.boolValue;
                        }
                    }
                    EditorGUILayout.Space();

                    MyOperationEditor.DrawArray( new MyOperationEditor.DrawArrayInfo<int>() {
                        Items = new int[propetyIDs.arraySize],

                        DrawTitleIndex = ( int data, int index ) => {
                            if( propetyIsDetail.boolValue == true ) {
                                EditorGUILayout.Space();
                                return data;
                            }

                            SerializedProperty propertyID = propetyIDs.GetArrayElementAtIndex( index );
                            DBItem item = GetItems( fieldInfo.FieldType ).Find( el => el.ID == propertyID.intValue );
                            EditorGUILayout.LabelField( item != null ? item.Name : "Not defined" );

                            return data;
                        },

                        DrawContentIndex = ( int data, int index ) => {
                            if( propetyIsDetail.boolValue == false ) {
                                return data;
                            }

                            SerializedProperty propertyID = propetyIDs.GetArrayElementAtIndex( index );
                            Drawer( fieldInfo.FieldType ).OnGUI( propertyID, GUIContent.none );
                            if( ids.Count( el => el == propertyID.intValue ) > 1 ) {
                                EditorGUILayout.HelpBox( "Already contains", MessageType.Warning );
                            }
                            return data;
                        },

                        DrawSeparator = data => {
                            EditorGUILayout.Space();
                            return data;
                        },

                        GUIStyleItem = MyOperationEditor.StyleBox,



                        CallbackMove = ( from, to ) => {
                            propetyIDs.MoveArrayElement( from, to );
                            propetyIDs.serializedObject.ApplyModifiedProperties();
                        },

                        CallbackAddedIndex = ( data, index ) => {
                            propetyIDs.InsertArrayElementAtIndex( index );
                            propetyIDs.serializedObject.ApplyModifiedProperties();
                        },

                        CallbackRemovedIndex = ( data, index ) => {
                            propetyIDs.DeleteArrayElementAtIndex( index );
                            propetyIDs.serializedObject.ApplyModifiedProperties();
                        }
                    } );


                    EditorGUILayout.Space();
                    using( new EditorGUILayout.HorizontalScope() ) {

                        // add one
                        if( MyOperationEditor.DrawButtonMini( "+" ) ) {
                            propetyIDs.InsertArrayElementAtIndex( propetyIDs.arraySize );
                            propetyIDs.GetArrayElementAtIndex( propetyIDs.arraySize - 1 ).intValue = -1;
                            propetyIDs.serializedObject.ApplyModifiedProperties();
                        }

                        // add many
                        if( MyOperationEditor.DrawButtonMini( "+ many" ) ) {
                            EditorWindowSelect.Show(
                                "Select",
                                GetItems( fieldInfo.FieldType )
                                .Select( value => new EditorWindowSelect.Item( value.ID, value.Name, value.IconUI.GetEditor(), value.Description, AssetDatabase.GetAssetPath( value ) ) )
                                .ToArray(),
                                ( EditorWindowSelect.Item value ) => {
                                    if( value.ID != -1
                                        && ids.Contains( value.ID ) == false
                                    ) {
                                        propetyIDs.InsertArrayElementAtIndex( propetyIDs.arraySize );
                                        propetyIDs.GetArrayElementAtIndex( propetyIDs.arraySize - 1 ).intValue = value.ID;
                                        propetyIDs.serializedObject.ApplyModifiedProperties();
                                    }
                                }
                            );
                        }
                    }

                }

            } catch { }


            if( GUI.changed == true ) {
                EditorUtility.SetDirty( property.serializedObject.targetObject );
            }

        }

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
			return 0;
		}

	}
#endif

}