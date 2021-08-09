using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
#endif



[System.Serializable]
public class ResourceItem {

    [SerializeField]
    private string path = "";

    [SerializeField]
    private string name = "";
    public string Name { get { return name; } }



    //private const string NAME_RESOURCES = "Resources";

    public void Get<T>( System.Action<T> callback ) where T : Object {

        /*if( path.Contains( NAME_RESOURCES ) == false ) {
            callback?.Invoke( null );
            return;
        }*/

        PoolResources.Get<T>( path, name, callback );
    }



#if UNITY_EDITOR

    [SerializeField]
    private string guid = "";
    public string GUID { get { return guid; } }



    public T GetEditor<T>() where T : Object {

        Object[] objects = AssetDatabase.LoadAllAssetsAtPath( path );
        if( objects.Length > 0 ) {
            return objects.FirstOrDefault( item => item.name == name && item is T ) as T;
        }

        return AssetDatabase.LoadAssetAtPath<T>( path );
    }


    [CustomPropertyDrawer( typeof( ResourceItem ), true )]
    public class ResourceItemEditor : PropertyDrawer {

        public const float SIZE_SPRITE = 100;

        private static Dictionary<string, bool> isExistInAddressables = null;
        private static Dictionary<string, bool> IsExistInAddressables { get { return isExistInAddressables = isExistInAddressables ?? new Dictionary<string, bool>(); } }


        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            EditorGUI.BeginProperty( position, label, property );


            System.Type typeObject = MyOperationClass.GetType( property.type ).GetTypeBase<ResourceItem<Object>>().GetGenericArguments()[0];

            SerializedProperty propertyGUID = property.FindPropertyRelative( "guid" );
            string guid = propertyGUID.stringValue;

            SerializedProperty propertyPath = property.FindPropertyRelative( "path" );
            string path = propertyPath.stringValue;

            SerializedProperty propertyName = property.FindPropertyRelative( "name" );
            string name = propertyName.stringValue;


            // get asset
            Object obj = AssetDatabase.LoadAllAssetsAtPath( path ).FirstOrDefault( item => item.name == name );

            if( obj == null ) {
                obj = AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( guid ), typeObject );
            }

            bool isExistInAddressables =    obj == null
                                            || IsExistInAddressables.ContainsKey( path ) == false
                                            || IsExistInAddressables[path] == true;

            // prepare rects
            Rect rectLabel = new Rect( position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight );
            Rect rectWarning = new Rect( rectLabel.x + rectLabel.width + EditorGUIUtility.standardVerticalSpacing, position.y, isExistInAddressables == false ? 15 + EditorGUIUtility.standardVerticalSpacing : 0, EditorGUIUtility.singleLineHeight );
            Rect rectObjectField = new Rect( rectWarning.x + rectWarning.width, position.y, position.width - rectWarning.x - rectWarning.width - 30, EditorGUIUtility.singleLineHeight );
            Rect rectButtonClear = new Rect( rectObjectField.x + rectObjectField.width + EditorGUIUtility.standardVerticalSpacing, position.y, 45, EditorGUIUtility.singleLineHeight );
            Rect rectHelpBox = new Rect( position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight * 2 );

            // prepare rects extra
            if( typeObject == typeof( Sprite ) ) {
                rectObjectField.x += rectObjectField.width - SIZE_SPRITE;
                rectObjectField.width = SIZE_SPRITE;
                rectObjectField.height = SIZE_SPRITE;

                rectButtonClear.height = SIZE_SPRITE;
            }

            // draw drop object
            GUI.Label( rectLabel, obj != null ? obj.name : "None" );
            obj = EditorGUI.ObjectField( rectObjectField, obj, typeObject, false );

            if( isExistInAddressables == false ) {
                GUI.Label( rectWarning, new GUIContent( "W", "Check setting this object in \"Addressables\"" ), EditorStyles.boldLabel );
            }

            // clear
            if( GUI.Button( rectButtonClear, "clear" ) ) {
                obj = null;
                path = "";
                name = "";
                guid = "";
            }
            guid = AssetDatabase.AssetPathToGUID( path );
            path = AssetDatabase.GetAssetPath( obj );
            name = obj ? obj.name : "";

            // set value
            propertyGUID.stringValue = guid;
            propertyPath.stringValue = path;
            propertyName.stringValue = name;
            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();


            // check exists in addressables
            System.Action<bool> callback = result => {
                if( IsExistInAddressables.ContainsKey( path ) == false
                    || IsExistInAddressables[path] != result
                ) {
                    IsExistInAddressables[path] = result;
                    EditorUtility.SetDirty( property.serializedObject.targetObject );
                }
            };
            EditorCoroutineUtility.StartCoroutine( CheckExistInAddressables( path, callback ), this );

        }


        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {

            System.Type typeObject = MyOperationClass.GetType( property.type ).GetTypeBase<ResourceItem<Object>>().GetGenericArguments()[0];
            if( typeObject == typeof(Sprite) ) {
                return SIZE_SPRITE;
            }

            return EditorGUIUtility.singleLineHeight;
        }


        private System.Collections.IEnumerator CheckExistInAddressables ( string path, System.Action<bool> callback ) {
            AsyncOperationHandle<IList<IResourceLocation>> handler = Addressables.LoadResourceLocationsAsync( path );
            yield return handler;
            callback?.Invoke( handler.Result.Count > 0 );
            Addressables.Release( handler );
        }


    }


    public class ResourceItemImporter : AssetPostprocessor {

        
        // update links for resource items (if changed asset)
        public static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths ) {

            // get prefabs from project
            string[] paths = AssetDatabase.FindAssets( "t:Prefab", new string[] { "Assets/Resources", "Assets/Prefabs" } );
            paths = paths.Select( item => AssetDatabase.GUIDToAssetPath( item ) ).ToArray();

            // changed assets
            List<string> assetsChanged = new List<string>();
            assetsChanged.AddRange( deletedAssets );
            assetsChanged.AddRange( movedFromAssetPaths );

            if( assetsChanged.Count == 0 ) {
                return;
            }

            // optimize
            System.Type typeResourceItem = typeof( ResourceItem );

            // find field ResourceItem in prefabs
            for( int i = 0; i < paths.Length; i++ ) {

                // check perfab
                GameObject goPrefab = AssetDatabase.LoadAssetAtPath<GameObject>( paths[i] );

                // get components
                // find need component
                MonoBehaviour[] components = goPrefab.GetComponentsInChildren<MonoBehaviour>();
                for( int k = 0; k < components.Length; k++ ) {

                    // get fields in component
                    // find ResourceItem field
                    FieldInfo[] fields = MyOperationClass.GetFields<SerializeField>( components[k] );
                    for( int f = 0; f < fields.Length; f++ ) {

                        // this field is ResourceItem ?
                        if( fields[f].FieldType.IsSubclassOf( typeResourceItem ) ) {

                            // get value field of ResourceItem
                            ResourceItem item = fields[f].GetValue( components[k] ) as ResourceItem;

                            // value is changed ?
                            if( item != null
                                && assetsChanged.Exists( asset => item.path.Contains( asset ) )
                            ) {

                                // instance prefabe for set and save new params
                                GameObject goPrefabInstance = PrefabUtility.InstantiatePrefab( goPrefab ) as GameObject;
                                MonoBehaviour[] componentsInstance = goPrefabInstance.GetComponentsInChildren<MonoBehaviour>();
                                FieldInfo[] fieldsiInstance = MyOperationClass.GetFields<SerializeField>( componentsInstance[k] );

                                // update path of ResourceItem by saved guid
                                ResourceItem itemInstance = fieldsiInstance[f].GetValue( componentsInstance[k] ) as ResourceItem;
                                string path = AssetDatabase.GUIDToAssetPath( item.guid );
                                Object obj = AssetDatabase.LoadAssetAtPath<Object>( path );
                                itemInstance.path = path;

                                // set new changed value to field
                                fieldsiInstance[f].SetValue( componentsInstance[k], itemInstance );

                                // save prefab (apply changes)
                                PrefabUtility.ApplyPrefabInstance( goPrefabInstance, InteractionMode.AutomatedAction );
                                Object.DestroyImmediate( goPrefabInstance );
                                
                                // log
                                Debug.Log( string.Format( "Updated <b>ResourceItem Link</b> in asset <b>{0}</b> <i>(field <b>{1}</b>)</i>", paths[i], fields[f].Name ) );
                            }
                        }

                    }
                }
            }

        }// function

    }// class


#endif

}


[System.Serializable]
public class ResourceItem<T> : ResourceItem where T : Object {

    public void Get( System.Action<T> callback ) {
        base.Get<T>( callback );
    }

    public void PreGet() {
        Get( null );
    }



#if UNITY_EDITOR

    public T GetEditor() {
        return base.GetEditor<T>();
    }

#endif

}



#if UNITY_EDITOR
public static class ResourceItemExtension {

    public static bool IsNull( this ResourceItem target ) {
        return  target == null
                || string.IsNullOrEmpty( target.GUID );
    }
        
}
#endif