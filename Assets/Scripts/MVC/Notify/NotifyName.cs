using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



namespace Kondrat.MVC {

	[Serializable]
    public class NotifyName {

		private static Item singleton = null;
		private static Item Singleton { get { return singleton = singleton ?? Item.GenerateItem(); } }




		[SerializeField]
		[HideInInspector]
		private string path = "";




		/// <summary>
		/// Get name of notify
		/// </summary>
		public string GetValue() {
			return Singleton.GetValue( path );
		}



		public NotifyName() {
			singleton = Singleton;
		}
		





		private class Item {

			private string name = "";
			public string Name { get { return name; } }

			private string value = null;
			public string Value { get { return value; } }

			private List<Item> childrens = null;

#if UNITY_EDITOR
			public List<Item> Childrens { get { return childrens; } }
#endif


			private int index = -1;
			public int Index { get { return index; } set { index = value; } }




			public Item( string name, string value = null ) {
				this.name = name;
				this.value = value;

				childrens = new List<Item>();
			}



			/// <summary>
			/// Add children
			/// </summary>
			public void AddChildren( Item children ) {
				childrens.Add( children );
			}


			/// <summary>
			/// Check exists children by name
			/// </summary>
			public bool ContainsChildren( string name ) {
				return childrens.Exists( item => item.name == name );
			}

			/// <summary>
			/// Find children by name
			/// </summary>
			public Item FindChildren( string name ) {
				return childrens.Find( item => item.name == name );
			}



			/// <summary>
			/// Find item recursive (check only selected items)
			/// </summary>
			/// <param name="callback">If is NULL - return last selected</param>
			/// <returns></returns>
			public Item FindSelected( Func<Item, bool> callback = null ) {

				if( index == -1 ) {
					return this;
				}

				if( childrens[index].value != null ) {
					return this;
				}

				if( callback != null
					&& callback( this )
				) {
					return this;
				}

				return childrens[index].FindSelected( callback );
			}



			/// <summary>
			/// Clear select for all
			/// </summary>
			public void Clear() {
				index = -1;

				for( int i = 0; i < childrens.Count; i++ ) {
					childrens[i].Clear();
				}
			}

			/// <summary>
			/// Get path selected
			/// </summary>
			public string Get() {

				if( index == -1 ) {
					return name;
				}

				return name + "." + childrens[index].Get();
			}

			/// <summary>
			/// Set path select
			/// </summary>
			public void Set( string path ) {

				string[] parts = path.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
				if( parts.Length <= 1 ) {
					return;
				}

				index = -1;

				for( int i = 0; i < childrens.Count; i++ ) {
					if( childrens[i].name == parts[1] ) {
						index = i;
						childrens[i].Set( path.Remove( 0, path.IndexOf( "." ) + 1 ) );
						break;
					}
				}

			}





			/// <summary>
			/// Get value selected
			/// </summary>
			public string GetValue() {

				if( index == -1 ) {
					return value ?? "";
				}

				return childrens[index].GetValue();
			}

			/// <summary>
			/// Get value by path
			/// </summary>
			public string GetValue( string path ) {

				string[] parts = path.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
				if( parts.Length <= 1 ) {
					return value ?? "";
				}

				for( int i = 0; i < childrens.Count; i++ ) {
					if( childrens[i].name == parts[1] ) {
						return childrens[i].GetValue( path.Remove( 0, path.IndexOf( "." ) + 1 ) );
					}
				}

				return value ?? "";
			}




			public static void AddNested( Item data, string path, Type type ) {

				FieldInfo field = type.GetField( "NAME" );
				if( field != null ) {
					Add( data, path, field.GetValue( null ).ToString() );

				} else {
					Add( data, path );

					Type[] typesNested = type.GetNestedTypes();
					for( int i = 0; i < typesNested.Length; i++ ) {
						AddNested( data, string.Format( "{0}.{1}", path, typesNested[i].Name ), typesNested[i] );
					}
				}

			}


			public static void Add( Item data, string name, string value = null ) {

				Item current = data;
				string[] parts = name.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries );

				for( int i = 0; i < parts.Length; i++ ) {
					string part = parts[i];

					if( current.ContainsChildren( part ) ) {
						current = current.FindChildren( part );

					} else {
						string itemValue = i + 1 == parts.Length ? value : null;
						current.AddChildren( new Item( part, itemValue ) );
						current = current.FindChildren( part );
					}
				}

			}


			public static Item GenerateItem() {

				Item data = new Item( "root" );

				Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
				for( int a = 0; a < assemblies.Length; a++ ) {

					Type[] types = assemblies[a].GetTypes();
					for( int t = 0; t < types.Length; t++ ) {

						Type typeNOTIFY = types[t].GetNestedType( "NOTIFY" );
						if( typeNOTIFY != null ) {
							AddNested( data, string.Format( "{0}.{1}", types[t].Namespace, types[t].Name ), typeNOTIFY );
						}

						FieldInfo[] fields = types[t].GetFields();
						for( int i = 0; i < fields.Length; i++ ) {
							if( fields[i].Name.Contains( "NOTIFY_" ) ) {
								Add( data, string.Format( "{0}.{1}.{2}", types[t].Namespace, types[t].Name, fields[i].Name ), fields[i].GetValue( null ).ToString() );
							}
						}
					}
				}

				return data;
			}

		}
		


#if UNITY_EDITOR



		[CustomPropertyDrawer( typeof(NotifyName) )]
		private class NotifyNamePropertyDrawer : PropertyDrawer {


			private Item data = null;
			private Item Data { get { return data ??= Item.GenerateItem(); } }



			private bool isExtended = false;



			public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {


				SerializedProperty propertyPath = property.FindPropertyRelative( "path" );
				Data.Clear();
				Data.Set( propertyPath.stringValue );




				Rect rectToggle = new Rect( position.x, position.y, 14, EditorGUIUtility.singleLineHeight );

				isExtended = GUI.Toggle( rectToggle, isExtended, "" );

				position.x = rectToggle.x + rectToggle.width + EditorGUIUtility.standardVerticalSpacing;
				position.width -= rectToggle.width + EditorGUIUtility.standardVerticalSpacing;


				if( isExtended == true ) {
					DrawExtendedGUI( Data, position );

				} else {
					DrawSimpleGUI( position );
				}


				string path = Data.Get();

				if( path != propertyPath.stringValue ) {
					propertyPath.stringValue = path;
					GUI.changed = true;
				}

				if( GUI.changed ) {
					property.serializedObject.ApplyModifiedProperties();
					EditorUtility.SetDirty( property.serializedObject.targetObject );
				}

			}

			public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {

				if( isExtended == true ) {
					return GetExtendedCount( Data, 1 ) * EditorGUIUtility.singleLineHeight;
				}

				return EditorGUIUtility.singleLineHeight;
			}





			public int GetExtendedCount( Item target, int count = 1 ) {

				if( target.Value != null ) {
					return count - 1;
				}

				if( target.Index != -1 ) {
					return GetExtendedCount( target.Childrens[target.Index], count + 1 );
				}

				return count;
			}


			/// <summary>
			/// Extended GUI
			/// </summary>
			public void DrawExtendedGUI( Item target, Rect position ) {

				if( target.Value != null ) {
					return;
				}

				Rect rectPopup = new Rect( position.x, position.y, position.width, EditorGUIUtility.singleLineHeight );

				string[] labels = target.Childrens.Select( item => item.Name ).ToArray();
				target.Index = EditorGUI.Popup( rectPopup, target.Name, target.Index, labels );

				rectPopup.y += EditorGUIUtility.singleLineHeight;

				if( target.Index != -1 ) {
					DrawExtendedGUI( target.Childrens[target.Index], rectPopup );
				}

			}


			/// <summary>
			/// Simple GUI
			/// </summary>
			public void DrawSimpleGUI( Rect position ) {

				if( Data.Value != null ) {
					return;
				}


				Rect rectPopup = new Rect( position.x, position.y, position.width - 25, position.height );
				Rect rectButton = new Rect( rectPopup.x + rectPopup.width, rectPopup.y, 25, position.height );


				Item itemLastSelected = Data.FindSelected();

				string[] labels = itemLastSelected.Childrens.Select( item => item.Name ).ToArray();
				itemLastSelected.Index = EditorGUI.Popup( rectPopup, itemLastSelected.Name, itemLastSelected.Index, labels );

				if( GUI.Button( rectButton, "✖" ) ) {
					itemLastSelected.Index = -1;
					Data.FindSelected( item => item.Childrens[item.Index].Index == -1 ).Index = -1;
				}

			}




		}
#endif


	}

}