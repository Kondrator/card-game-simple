using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif



public class DBItem : Kondrat.MVC.Element {

	public class ComparerID<T> : IComparer<T> where T : DBItem{

		public int Compare( T x, T y ){

			if( x.ID < y.ID ){
				return -1;

			}else if( x.ID > y.ID ){
				return 1;
			}

			return string.Compare( x.Name, y.Name );
		}

	}


	
	[SerializeField, HideInInspector]
	private int id = -1;
	public int ID{
		get{ return id; }
#if UNITY_EDITOR
		set{ id = value; }
#endif
	}

	[SerializeField]
	private new LocalizedString name = "Название";
	public virtual string Name { get { return name; } }

	[SerializeField]
	private LocalizedString description = "Описание";
	public virtual string Description { get { return description; } }

	[SerializeField]
	private ResourceSprite iconUI = null;
	public virtual ResourceSprite IconUI {
		get{ return iconUI; }
	}



	private System.Type type = null;
	public System.Type Type { get { return type = type ?? GetType(); } }




	[HideInInspector]
	public UnityEvent OnDown = new UnityEvent();

	void OnMouseDown(){
		if( OnDown != null
			&& OnDown.GetPersistentEventCount() > 0
			&& MyOperationUI.IsCursorOverUI() == false
		){
			OnDown.Invoke();
		}
	}


	public override string ToString(){
		return ID + " - " + Name;
	}


#region Editor Inspector
#if UNITY_EDITOR

	public virtual void SettingName(){
		string path = AssetDatabase.GetAssetPath( this );
		if( string.IsNullOrEmpty( path ) == false ) {
			string name = this.ID + " - " + this.GetType().Name.ToStringSplitWords();
			SettingName( path, name );
		}
	}
	private void SettingName( string path, string name ){
		FileInfo fileInfo = new FileInfo( path );
		if( File.Exists( fileInfo.Directory.FullName + "/" + name + ".prefab"  ) == false ){
			AssetDatabase.RenameAsset( path, name );
			EditorGUIUtility.PingObject( this );
		}
	}


	// this need for DB Manager
	[HideInInspector]
	public bool isEditingID = false;
	[HideInInspector]
	public int editingID = -1;





	public void SetName( LocalizedString name ) {
		this.name = name;
	}
	public void SetDescription( LocalizedString description ) {
		this.description = description;
	}

	public void SetIconUI( ResourceSprite sprite ) {
		this.iconUI = sprite;
	}





	protected virtual void OnInspectorGUI() { }


	protected class DBItemEditor<T> : Editor<T> where T : DBItem {


		protected virtual bool IsCanEdit { get { return true; } }
		protected virtual bool IsVisibleEdit { get { return true; } }



		public override void OnInspectorGUI() {

			if( string.IsNullOrEmpty( AssetDatabase.GetAssetPath( component ) ) == false ){

				component.SettingName();

				EditorGUILayout.Space();

				// load db at path
				DBManager dbManager = DBManager.LoadAsssetByItem( component );
				if( dbManager != null ){

					bool isSelectDBManager = false;
					string selectDBManager = "Show DB Manager";
					bool isContainsInDB = dbManager.Contains_Editor( component );

					// contains item in DB
					if( isContainsInDB == false ){
						EditorGUILayout.HelpBox( "Item not exists in DB Manager.", MessageType.Warning );

						EditorGUILayout.BeginHorizontal();
							// button add item to db
							if( MyOperationEditor.DrawButtonMini( "Add to DB Manager" ) ){
								dbManager.AddItem_Editor( component );
							}
							// button add item to db
							if( MyOperationEditor.DrawButtonMini( "Add to DB Manager (+ reset ID)" ) ){
								component.ID = -1;
								dbManager.AddItem_Editor( component );
							}
							// button select db
							isSelectDBManager = MyOperationEditor.DrawButtonMini( selectDBManager );
						EditorGUILayout.EndHorizontal();

					// not contains item in DB
					}else{
						EditorGUILayout.LabelField( "ID: <b>" + component.id + "</b>    <i>Contains in DB Manager</i>", MyOperationEditor.StyleLabel );
					}

					// button select db
					if( isContainsInDB == true ){
						// button select db
						if( isSelectDBManager == false ){
							isSelectDBManager = MyOperationEditor.DrawButtonMini( selectDBManager );
						}
					}

					if( isSelectDBManager == true ){
						MyOperationEditor.InspectTarget( dbManager.gameObject );
					}

				// not exists DB in path
				}else{
					Debug.LogError( "DB Manager not exists at <b>" + component + "</b>." );
				}

				EditorGUILayout.Space();
			}


			if( IsVisibleEdit == true ) {
				GUI.enabled = IsCanEdit;
				base.OnInspectorGUI();
				GUI.enabled = true;
			}
			
		}

	}


	[CustomEditor( typeof(DBItem), true )]
	protected class DBItemEditor : DBItemEditor<DBItem> {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			component.OnInspectorGUI();
		}

	}



#endif
#endregion
	}


public class DBItem<T> : DBItem where T : DBManager {

	public T DBManager { get { return dbManager = Find<T>(); } }
	private T dbManager = null;

}