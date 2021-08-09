using GameToolkit.Localization;
using UnityEngine;
using Kondrat.MVC;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class CardSuitData : Element {

    public class PopupAttribute : PropertyAttribute { }




    [System.Serializable]
    public class Item {

        [SerializeField]
        private int id = -1;
        public int ID {
            get { return id; }
#if UNITY_EDITOR
            set { id = value; }
#endif
        }



        [SerializeField]
        private LocalizedString localizedString = null;

        public string Name {
            get { return localizedString?.Text; }
        }



        [SerializeField]
        private string symbol = "";
        public string Symbol {
            get { return symbol; }
        }

        [SerializeField]
        private Sprite icon = null;
        public Sprite Icon {
            get { return icon; }
        }

        [SerializeField]
        private Color color = Color.white;
        public Color Color {
            get { return color; }
        }




#if UNITY_EDITOR
        [SerializeField]
        private bool isEditableID = true;
        public bool IsEditableID {
            get { return isEditableID; }
            set { isEditableID = value; }
        }
#endif

    }


    [SerializeField]
    private Item[] items = new Item[0];




    public int Count { get => items.Length; }





    /// <summary>
    /// Get suit type by id
    /// </summary>
    public Item GetByID( int id ) {

        if( items == null ) {
            return null;
        }

        return items.FirstOrDefault( el => el.ID == id );
    }


    /// <summary>
    /// Get all suit types
    /// </summary>
    public Item[] GetAll() {

        if( items == null ) {
            return new Item[0];
        }

        return items;
    }





#if UNITY_EDITOR
    [CustomEditor( typeof(CardSuitData) )]
    private class CardSuitDataEditor : Editor<CardSuitData> {

        public override void OnInspectorGUI() {

            EditorGUILayout.Space();

            component.items = MyOperationEditor.DrawArray( new MyOperationEditor.DrawArrayInfo<Item>() {
                Items = component.items,

                DrawTitleIndex = ( data, index ) => {
                    EditorGUILayout.LabelField( string.Format( "[{0}] {1}", data.ID, data.Name ?? "Название не указано" ) );
                    return data;
                },

                DrawContentIndex = ( data, index ) => {

                    EditorGUILayout.Space();

                    using( new EditorGUILayout.VerticalScope( MyOperationEditor.StyleBox ) ) {
                        SerializedProperty property = serializedObject.FindProperty( "items" ).GetArrayElementAtIndex( index );

                        using( new EditorGUILayout.HorizontalScope() ) {
                            SerializedProperty propertyIsEditableID = property.FindPropertyRelative( "isEditableID" );
                            SerializedProperty propertyID = property.FindPropertyRelative( "id" );

                            EditorGUILayout.PrefixLabel( "ID" );
                            propertyIsEditableID.boolValue = EditorGUILayout.Toggle( propertyIsEditableID.boolValue, GUILayout.Width( 14 ) );

                            GUI.enabled = propertyIsEditableID.boolValue;
                            propertyID.intValue = EditorGUILayout.IntField( propertyID.intValue );
                            GUI.enabled = true;
                        }

                        EditorGUILayout.PropertyField( property.FindPropertyRelative( "localizedString" ) );
                        EditorGUILayout.PropertyField( property.FindPropertyRelative( "symbol" ) );
                        EditorGUILayout.PropertyField( property.FindPropertyRelative( "icon" ) );
                        EditorGUILayout.PropertyField( property.FindPropertyRelative( "color" ) );
                    }

                    return data;
                },

                DrawSeparator = data => {
                    MyOperationEditor.DrawSeparator();
                    return data;
                },

                GUIStyleItem = MyOperationEditor.StyleBox,

                IsNeedAdd = () => true,
            } );


            if( GUI.changed ) {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty( component );
            }

        }

    }






    [CustomPropertyDrawer( typeof( PopupAttribute ) )]
    public class PopupDrawer : PropertyDrawer {

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            GUIContent[] options = Asset.items.Select( el => new GUIContent( string.Format( "{0} {1}", el.Symbol, el.Name ) ) ).ToArray();

            property.intValue = EditorGUI.Popup( position, label, property.intValue, options );

        }

    }





    private static CardSuitData asset = null;
    public static CardSuitData Asset { get { return asset = asset ?? MyOperationEditor.FindAssetByType<CardSuitData>(); } }

    public static int PopupType( int value ) {

        string[] options = Asset.items.Select( el => el.Name ).ToArray();

        return EditorGUILayout.Popup( "Type", value, options );
    }
#endif


}