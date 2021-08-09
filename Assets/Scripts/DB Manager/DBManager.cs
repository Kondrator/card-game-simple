using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Kondrat.MVC;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif



public class DBManager : Element {

	[AttributeUsage( AttributeTargets.Class )]
	public class EditorInfo : Attribute {

		private string nameCategoryEng = "";
		public string NameCategoryEng { get { return nameCategoryEng; } }

		private string nameCategoryRusCode = "";
		public string NameCategoryRusCode { get { return nameCategoryRusCode; } }

		private string nameCategoryJson = "";
		public string NameCategoryJson { get { return nameCategoryJson; } }

		private string nameStoreID = null;
		public string NameStoreID { get { return nameStoreID; } }


		public EditorInfo( string nameCategoryEng, string nameCategoryRusCode, string nameCategoryJson, string nameStoreID = null ) {
			this.nameCategoryEng = nameCategoryEng;
			this.nameCategoryRusCode = nameCategoryRusCode;
			this.nameCategoryJson = nameCategoryJson;
			this.nameStoreID = nameStoreID;
		}
	}

	[AttributeUsage( AttributeTargets.Class )]
	public class DisableEditID : Attribute {}




	[SerializeField]
	private DBItem[] items = null;
	private DBItem[] Items{
		get { return items = items ?? new DBItem[0]; }
#if UNITY_EDITOR
		set { items = value; }
#endif
	}








	
	/// <summary>
	/// Get item by ID from need list.
	/// </summary>
	private T Get<T>( List<T> items, int id ) where T : DBItem{
		return items.Find( item => item.ID == id );
	}

	/// <summary>
	/// Get item by ID from need group type. And Cast to need type.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	/// <param name="id">ID of need item.</param>
	/// <returns>Finded item.</returns>
	public T Get<T>( int id ) where T : DBItem {
		return (T)Get( GetList<T>(), id );
	}

	/// <summary>
	/// Get item by ID from need group type. And Cast to need type.
	/// </summary>
	/// <param name="type">Type of item.</param>
	/// <param name="id">ID of need item.</param>
	/// <returns>Finded item.</returns>
	public DBItem Get( System.Type type, int id ) {
		return Get( GetList( type ), id );
	}


	/// <summary>
	/// Get items group as type.
	/// </summary>
	/// <param name="type">Type of item list.</param>
	/// <returns>List items of need type.</returns>
	public List<DBItem> GetList( System.Type type ){
		
		List<DBItem> list = new List<DBItem>();

		for( int i = 0; i < Items.Length; i++ ){
			if( Items[i].GetType() == type ){
				list.Add( Items[i] );
			}
		}

		return list;
	}

	/// <summary>
	/// Get items group as type.
	/// </summary>
	/// <typeparam name="T">Cast item to type in list.</typeparam>
	/// <param name="type">Type of item list.</param>
	/// <returns>List items of need type.</returns>
	public List<T> GetList<T>( System.Type type ) where T : DBItem {
		
		List<DBItem> listGet = GetList( type );
		List<T> listReturn = new List<T>();

		for( int i = 0; i < Items.Length; i++ ){
			if( Items[i] is T ){
				listReturn.Add( (T)listGet[i] );
			}
		}

		return listReturn;
	}

	/// <summary>
	/// Get items group as type.
	/// </summary>
	/// <typeparam name="T">Type of items.</typeparam>
	/// <returns>List items of need type.</returns>
	public List<T> GetList<T>() where T : DBItem {
		
		List<T> list = new List<T>();

		Type type = typeof(T);

		for( int i = 0; i < Items.Length; i++ ){
			if( Items[i].Type == type ) {
				list.Add( (T)Items[i] );
			}
		}

		return list;
	}





#region Editor Inspector
#if UNITY_EDITOR


	private static Dictionary<Type, DBManager> dbManagerCache = null;
	private static Dictionary<Type, DBManager> DBManagerCache {
		get {
			return dbManagerCache = dbManagerCache ?? new Dictionary<Type, DBManager>();
		}
	}



	public static DBManager LoadAsssetByItem( DBItem item ) {
		return LoadAsssetByItem( item.GetType() );
	}

	public static DBManager LoadAsssetByItem( Type typeItem ) {
		System.Type typeDBManager = typeItem.GetTypeBase<DBItem<DBManager>>().GetGenericArguments()[0];
		return LoadAssset( typeDBManager );
	}

	public static DBManager LoadAssset( Type typeDBManager ) {
		if( DBManagerCache.ContainsKey( typeDBManager ) == true ) {
			return DBManagerCache[typeDBManager];
		}

		List<UnityEngine.Object> objects = MyOperationEditor.FindAssetsByType( typeDBManager );
		if( objects.Count > 0 ) {
			DBManagerCache[typeDBManager] = AssetDatabase.LoadAssetAtPath<DBManager>( AssetDatabase.GetAssetPath( objects[0] ) );
			return DBManagerCache[typeDBManager];
		}

		return null;
	}



	public void AddItem_Editor( DBItem item ){

		if( Items.Contains( item ) == false ){
			Items = Items.AddItem( item );
			EditorUtility.SetDirty( this );
		}

		CheckItems_Editor();
	}

	public bool Contains_Editor( DBItem item ){
		return Items.Contains( item );
	}

	private void CheckItems_Editor(){
			
		// fix items
		for( int i = 0; i < Items.Length; i++ ){

			// null item
			if( Items[i] == null ){
				Items = Items.RemoveItem( i-- );
				EditorUtility.SetDirty( this );

			// not have ID
			}else if( Items[i].ID == -1 ){
				// generate ID
				GameObject goItemPrefab = PrefabUtility.InstantiatePrefab( Items[i].gameObject ) as GameObject;
				goItemPrefab.GetComponent<DBItem>().ID = GetUniqueID_Editor( GetList( Items[i].GetType() ) );
				PrefabUtility.ApplyPrefabInstance( goItemPrefab, InteractionMode.AutomatedAction );
				DestroyImmediate( goItemPrefab );

				EditorUtility.SetDirty( this );
			}
		}

	}

	private int GetUniqueID_Editor( List<DBItem> items ){
			
		int id = 0;

		for( int i = 0; i < items.Count; i++ ){
			if( id <= items[i].ID ){
				id = items[i].ID + 1;
			}
		}

		return id;
	}





	private static Dictionary<System.Type, string[]> cacheOptionsDisplayed = null;
	private static Dictionary<System.Type, string[]> CacheOptionsDisplayed {
		get {
			if( cacheOptionsDisplayed == null ) {
				cacheOptionsDisplayed = new Dictionary<Type, string[]>();
			}
			return cacheOptionsDisplayed;
		}
	}

	private static Dictionary<System.Type, int[]> cacheOptionsValues = null;
	private static Dictionary<System.Type, int[]> CacheOptionsValues {
		get {
			if( cacheOptionsValues == null ) {
				cacheOptionsValues = new Dictionary<Type, int[]>();
			}
			return cacheOptionsValues;
		}
	}

	/// <summary>
	/// Draw popup select DB Item from all.
	/// </summary>
	/// <param name="id">Current selected.</param>
	public static int DrawPopupDBItem<T>( string name, int id ) where T : DBItem {

		System.Type typeDBItem = typeof( T );

		if( CacheOptionsDisplayed.ContainsKey( typeDBItem ) == false
			|| CacheOptionsValues.ContainsKey( typeDBItem ) == false
		) {
			DBManager dbManager = DBManager.LoadAsssetByItem( typeDBItem );
			if( dbManager == null ) {
				return id;
			}

			List<T> items = dbManager.GetList<T>();
			items.Sort( new DBItem.ComparerID<T>() );

			string[] optionsDisplayed = new string[items.Count];
			int[] optionsValues = new int[items.Count];
			for( int i = 0; i < items.Count; i++ ) {
				optionsDisplayed[i] = "[" + items[i].ID + "] " + items[i].Name;
				optionsValues[i] = items[i].ID;
			}

			CacheOptionsDisplayed[typeDBItem] = optionsDisplayed;
			CacheOptionsValues[typeDBItem] = optionsValues;
		}

		if( name == null ){
			return EditorGUILayout.IntPopup( id, CacheOptionsDisplayed[typeDBItem], CacheOptionsValues[typeDBItem] );
		}
		return EditorGUILayout.IntPopup( name, id, CacheOptionsDisplayed[typeDBItem], CacheOptionsValues[typeDBItem] );

	}




	[CustomEditor(typeof(DBManager), true)]
	private class DBManagerEditor : Editor<DBManager> {

		private GUIStyle styleTextFieldID = null;
		private GUIStyle StyleTextFieldID {
			get {
				if( styleTextFieldID == null ) {
					styleTextFieldID = new GUIStyle( EditorStyles.textField );
					styleTextFieldID.alignment = TextAnchor.MiddleCenter;
				}
				return styleTextFieldID;
			}
		}




		private List<DBItem>[] categories;
		
		private Type[] CATEGORIES_QUEUE;
		private string[] CATEGORIES_NAMES_ENG;
		private string[] CATEGORIES_NAMES_RUS_CODES;
		private bool[] CATEGORIES_OPENED;
		private string[] CATEGORIES_NAMES_JSON;

		private bool[] CATEGORIES_CAN_EDIT_ID;



		protected override void OnEnable() {
			base.OnEnable();

			CATEGORIES_QUEUE = new Type[0];
			CATEGORIES_NAMES_ENG = new string[0];
			CATEGORIES_NAMES_RUS_CODES = new string[0];
			CATEGORIES_OPENED = new bool[0];
			CATEGORIES_NAMES_JSON = new string[0];
			CATEGORIES_CAN_EDIT_ID = new bool[0];

			Type[] types = MyOperationClass.GetAllBaseTypes( MyOperationClass.GetType( String.Format( "DBItem`1[{0}]", component.GetType() ) ) );
			for( int i = 0; i < types.Length; i++ ) {
				EditorInfo info = Attribute.GetCustomAttribute( types[i], typeof(EditorInfo) ) as EditorInfo;
				if( info != null ) {
					CATEGORIES_QUEUE = CATEGORIES_QUEUE.AddItem( types[i] );
					CATEGORIES_NAMES_ENG = CATEGORIES_NAMES_ENG.AddItem( info.NameCategoryEng );
					CATEGORIES_NAMES_RUS_CODES = CATEGORIES_NAMES_RUS_CODES.AddItem( info.NameCategoryRusCode );
					CATEGORIES_OPENED = CATEGORIES_OPENED.AddItem( false );
					CATEGORIES_NAMES_JSON = CATEGORIES_NAMES_JSON.AddItem( info.NameCategoryJson );
					CATEGORIES_CAN_EDIT_ID = CATEGORIES_CAN_EDIT_ID.AddItem( Attribute.IsDefined( types[i], typeof(DisableEditID) ) == false );
				}
			}


			Sort();
			GenerateCategories();
		}

		public override void OnInspectorGUI(){

			component.CheckItems_Editor();
			
			MyOperationEditor.DrawSeparator( 5 );
			
			EditorGUILayout.BeginHorizontal();
				DrawButtonPrint();
				if( MyOperationEditor.DrawButtonMini( "Sort", 40 ) ){
					GenerateCategories();
				}
			EditorGUILayout.EndHorizontal();


			// --- DRAW CATEGORIES
			for( int i = 0; i < categories.Length; i++ ){

				MyOperationEditor.DrawSeparator( 5 );
				
				// category title
				EditorGUILayout.BeginHorizontal();
					if( MyOperationEditor.DrawButtonMini( CATEGORIES_OPENED[i] == true ? "♥" : "♡", 25 ) ){
						CATEGORIES_OPENED[i] = !CATEGORIES_OPENED[i];
					}
					// auto open if have uncorrect IDs items in this category
					bool isCorrectCategory = true;
					for( int b = 0; b < categories[i].Count; b++ ){
						bool isCorrect = GetCountSameID( categories[i], categories[i][b].ID ) == 1;
						if( isCorrect == false ){
							isCorrectCategory = false;
						}
						categories[i][b].SettingName();
					}
					if( isCorrectCategory == false ){
						CATEGORIES_OPENED[i] = true;
					}
					EditorGUILayout.LabelField( CATEGORIES_NAMES_ENG[i] + " (" + categories[i].Count + ")", EditorStyles.boldLabel );
				EditorGUILayout.EndHorizontal();
				
				// items in category
				if( CATEGORIES_OPENED[i] == true ){
					float widthID = 40;
					float widthLabel = EditorGUIUtility.labelWidth - widthID;
					float widthLabelMinus = 0;
					int indexRemove = -1;
					for( int b = 0; b < categories[i].Count; b++ ){
						bool isCorrect = GetCountSameID( categories[i], categories[i][b].ID ) == 1;

						using( new EditorGUILayout.HorizontalScope() ){

							GUI.enabled = CATEGORIES_CAN_EDIT_ID[i];

							// edit ID disable
							if( categories[i][b].isEditingID == false ){
								widthLabelMinus = 0;
								if( MyOperationEditor.DrawButtonMini( categories[i][b].ID.ToString(), isCorrect ? Color.white : Color.red, (int)widthID ) ){
									categories[i][b].isEditingID = true;
									categories[i][b].editingID = categories[i][b].ID;
								}

							// edit ID enable
							}else{
								widthLabelMinus = 25;
								categories[i][b].editingID = EditorGUILayout.IntField( categories[i][b].editingID, StyleTextFieldID, GUILayout.Width( widthID ) );
								if( MyOperationEditor.DrawButtonMini( "✓", (int)widthLabelMinus ) ){
									categories[i][b].isEditingID = false;
									categories[i][b].ID = categories[i][b].editingID;
									PrefabUtility.SavePrefabAsset( categories[i][b].gameObject );
								}
								if( MyOperationEditor.DrawButtonMini( "✖", (int)widthLabelMinus ) ){
									categories[i][b].isEditingID = false;
								}
								widthLabelMinus += 4;
							}

							GUI.enabled = true;

							EditorGUILayout.LabelField( categories[i][b].Name, GUILayout.Width( widthLabel - widthLabelMinus ) );
							EditorGUILayout.ObjectField( categories[i][b], CATEGORIES_QUEUE[i], false );

							if( MyOperationEditor.DrawButtonMini( "○", Color.white.Clone( 0.3f ), 25 ) ){
								MyOperationEditor.InspectTarget( categories[i][b].gameObject );
							}

							if( MyOperationEditor.DrawButtonMini( "✖", Color.red.Clone( 0.3f ), 25 ) ){
								indexRemove = b;
							}

						}// using

					}// for

					if( indexRemove != -1
						&& EditorUtility.DisplayDialog( "DB Manager", string.Format( "Remove [{0}] from DB ?", categories[i][indexRemove] ), "Remove", "Cancel" )
					) {
						component.Items = component.Items.RemoveItem( element => element.ID == categories[i][indexRemove].ID );
						categories[i].RemoveAt( indexRemove );
					}
				}
				
			}
			MyOperationEditor.DrawSeparator( 5 );





			// --- DRAW ADD ITEMS

			// check drop
			DBItem item = null;
			item = (DBItem)EditorGUILayout.ObjectField( "Drop for add", item, typeof(DBItem), false );
			if( item != null ){
				if( component.Contains_Editor( item ) == true ){
					EditorUtility.DisplayDialog( "Add to DB Manager", item.ToString() + " - allready contains in DB Manager.", "Close" );

				}else{
					component.AddItem_Editor( item );

					int index = Array.IndexOf( CATEGORIES_QUEUE, item.GetType() );
					if( index != -1 ){
						categories[index].Add( item );
					}
				}
			}


			if( GUI.changed ){
				EditorUtility.SetDirty( component );
				serializedObject.ApplyModifiedProperties();
			}
		}


		private int GetCountSameID( List<DBItem> items, int id ){
			
			int count = 0;

			for( int i = 0; i < items.Count; i++ ){
				if( items[i].ID == id ){
					count++;
				}
			}

			return count;
		}



		private void Sort() {
			Array.Sort( component.items, ( a, b ) => a.ID.CompareTo( b.ID ) );
		}


		private void DrawButtonPrint(){

			if( MyOperationEditor.DrawButtonMini( "To console" ) ){
				print( ToString() );

			}else if( MyOperationEditor.DrawButtonMini( "To clipboard" ) ){
				GUIUtility.systemCopyBuffer = ToString();

			}else if( MyOperationEditor.DrawButtonMini( "To JSON" ) ){
				string pathDescktop = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Desktop );
				string path = EditorUtility.SaveFilePanel( "Export DB to JSON", pathDescktop, "db.json", "json" );
				if( path.Length > 0 ){
					File.WriteAllText( path, ToJSON().ToString() );
				}
			}

		}
		public override string ToString(){
			
			// prepare
			string preCategory = "## ";
			string preItem = "\t";
			string result = "";
			for( int i = 0; i < categories.Length; i++ ){

				if( result.Length > 0 ){
					result += "\n\n";
				}
				result += preCategory + CATEGORIES_NAMES_RUS_CODES[i];

				for( int b = 0; b < categories[i].Count; b++ ){
					result += "\n" + preItem + categories[i][b].ID + " - " + categories[i][b].Name;
				}
			}

			return result;
		}
		public JObject ToJSON(){

			string KEY_ID = "id";
			string KEY_NAME = "name";
			string KEY_ICON = "icon";
			int ICON_SIZE = 64;
			JObject jdata = new JObject();

			for( int i = 0; i < categories.Length; i++ ){
				
				// get current category
				JArray jarray = new JArray();
				for( int b = 0; b < categories[i].Count; b++ ){
					JObject jitem = new JObject();
					jitem[KEY_ID] = categories[i][b].ID;
					jitem[KEY_NAME] = categories[i][b].Name;

					if( categories[i][b].IconUI != null ){
						Texture2D texture = categories[i][b].IconUI.GetEditor().texture;
						string pathIcon = AssetDatabase.GetAssetPath( categories[i][b].IconUI.GetEditor() );
						pathIcon = Application.dataPath.Remove( Application.dataPath.Length - 6, 6 ) + pathIcon;
						jitem[KEY_ICON] = IconToBase64( pathIcon, ICON_SIZE );
					}

					jarray.Add( jitem );
				}
				jdata[CATEGORIES_NAMES_JSON[i]] = jarray;
			}

			return jdata;
		}


		/// <summary>
		/// Icon to base64 and change size.
		/// </summary>
		private string IconToBase64( string pathIcon, int sizeIcon ){

			FileInfo fileInfo = new FileInfo( pathIcon );
			if( fileInfo.Exists == false ){
				return "";
			}
			string extension = fileInfo.Extension.TrimStart( '.' );
			byte[] data = File.ReadAllBytes( pathIcon );
			
			Texture2D texture = LoadPNG( pathIcon );
			TextureScaler.scale( texture, sizeIcon, sizeIcon );

			string base64 = Convert.ToBase64String( texture.EncodeToPNG() );
			return "data:image/" + fileInfo.Extension.TrimStart( '.' ) + ";base64," + base64;
		}

		/// <summary>
		/// Load file to Texture2D.
		/// </summary>
		public static Texture2D LoadPNG(string filePath) {
			Texture2D tex = null;
			byte[] fileData;
 
			if( File.Exists( filePath ) ){
				fileData = File.ReadAllBytes( filePath );
				tex = new Texture2D( 2, 2 );
				tex.LoadImage( fileData ); //..this will auto-resize the texture dimensions.
			}
			return tex;
		}






		private void GenerateCategories(){

			component.CheckItems_Editor();

			categories = new List<DBItem>[CATEGORIES_QUEUE.Length];
			for( int i = 0; i < categories.Length; i++ ){
				categories[i] = new List<DBItem>();
			}

			for( int i = 0; i < component.Items.Length; i++ ){
				component.Items[i].isEditingID = false;

				int index = Array.IndexOf( CATEGORIES_QUEUE, component.Items[i].GetType() );
				if( index != -1 ){
					categories[index].Add( component.Items[i] );
				}
			}

			for( int i = 0; i < categories.Length; i++ ){
				categories[i].Sort( new ComparerByID() );
			}

		}
		private class ComparerByID : Comparer<DBItem>{
			public override int Compare( DBItem x, DBItem y ){
				return x.ID.CompareTo( y.ID );
			}
		}
		

	}

#endif
#endregion
}
