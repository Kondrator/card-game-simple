using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Diagnostics;
using UnityEditor.AddressableAssets.Settings;


public class MenuSettings : MonoBehaviour {



	[MenuItem( "Settings/Addressable/Build", false, 100 )]
	static void MenuItem_Addressable_Build() {
		AddressableAssetSettings.BuildPlayerContent();
	}





	#region LOCAL SAVES

	private static string PATH_TO_FOLDER {
		get {
			return Application.persistentDataPath.Replace( @"/", @"\" );
		}
	}

	private static string PATH_TO_FILE_SAVE {
		get {
			return PATH_TO_FOLDER + "\\account.data";
		}
	}




	[MenuItem( "Settings/Local Save/Open", false, 50 )]
	static void MenuItem_LocalSaves_Open() {
		if( File.Exists( PATH_TO_FILE_SAVE ) == true ) {
			Process.Start( "explorer.exe", "/select," + PATH_TO_FILE_SAVE );

		} else if( Directory.Exists( PATH_TO_FOLDER ) == true ) {
			Process.Start( "explorer.exe", "/open," + PATH_TO_FOLDER );

		} else {
			UnityEngine.Debug.Log( "Folder of data project not exists" );
		}
	}


	[MenuItem( "Settings/Local Save/Clear", false, 51 )]
	static void MenuItem_LocalSaves_Clear() {
		if( File.Exists( PATH_TO_FILE_SAVE ) == true ) {
			if( EditorUtility.DisplayDialog( "Local save - Delete", "Delete local save?", "Delete", "Cancel" ) ) {
				File.Delete( PATH_TO_FILE_SAVE );
				UnityEngine.Debug.Log( "File save deleted success" );
			}

		} else {
			UnityEngine.Debug.Log( "File save not exists" );
		}
	}

#endregion


}
