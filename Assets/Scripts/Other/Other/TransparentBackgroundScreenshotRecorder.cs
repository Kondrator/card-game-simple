using UnityEngine;
using System.Collections;
using System.IO;
using System;

/*
Link:
https://medium.com/@mormo_music/render-screenshots-with-transparent-backgrounds-from-unity3d-free-version-dc098c5bbba2

Usage:
1. Attach this script to your chosen camera's game object.
2. Set that camera's Clear Flags field to Solid Color.
3. Use the inspector to set frameRate and framesToCapture
4. Choose your desired resolution in Unity's Game window (must be less than or equal to your screen resolution)
5. Turn on "Maximise on Play"
6. Play your scene. Screenshots will be saved to YourUnityProject/Screenshots by default.
*/

public class TransparentBackgroundScreenshotRecorder : MonoBehaviour {

#region private fields
	private GameObject whiteCamGameObject;
	private Camera whiteCam;
	private GameObject blackCamGameObject;
	private Camera blackCam;
	private Camera mainCam;
	private Texture2D textureBlack;
	private Texture2D textureWhite;
	private Texture2D textureTransparentBackground;
#endregion

	private string PathFolder{ get{ return pathFolder; } }
	private string pathFolder = "";
	
	private string ImageName{ get{ return imageName; } }
	private string imageName = "";
	private string ImageExtension{ get{ return imageExtension; } }
	private string imageExtension = "";
	
	private int CameraWidth{ get{ return 256; } }
	private int CameraHeight{ get{ return 256; } }
	
	private float CameraRectWidth{ get{ return (float)CameraWidth / (float)Screen.width; } }
	private float CameraRectHeight{ get{ return (float)CameraHeight / (float)Screen.height; } }



	void Awake(){
		mainCam = gameObject.GetComponent<Camera>();
		CreateBlackAndWhiteCameras();
		CacheAndInitialiseFields();
	}

	/*void LateUpdate(){
		if( Input.GetKeyDown( KeyCode.E ) ){
			StartCoroutine( Capture( Environment.GetFolderPath( Environment.SpecialFolder.Desktop ), "test" ) );
		}
	}*/


	public IEnumerator Capture( string pathFolder, string imageName, string imageExtension = "png" ){
		this.pathFolder = pathFolder;
		this.imageName = imageName;
		this.imageExtension = imageExtension;
		yield return CaptureFrame();
	}
	public IEnumerator CaptureToPersistentDataPath( string imageName, string imageExtension = "png" ){
		yield return Capture( Application.persistentDataPath, imageName, imageExtension );
	}


	IEnumerator CaptureFrame(){
		yield return new WaitForEndOfFrame();

		/*
		// png
		RenderCamToTexture( blackCam, textureBlack );
		RenderCamToTexture( whiteCam, textureWhite );
		CalculateOutputTexture();
		*/

		// jpg
		RenderCamToTexture( whiteCam, textureTransparentBackground );
		
		SavePng();
	}

	void RenderCamToTexture( Camera cam, Texture2D tex ){
		cam.enabled = true;
		cam.Render();
		RenderTexture.active = cam.targetTexture;
		WriteScreenImageToTexture( tex );
		cam.enabled = false;
	}

	void CreateBlackAndWhiteCameras(){
		whiteCamGameObject = (GameObject) new GameObject();
		whiteCamGameObject.name = "White Background Camera";
		whiteCam = whiteCamGameObject.AddComponent<Camera>();
		whiteCam.CopyFrom( mainCam );
		whiteCam.backgroundColor = Color.white;
		whiteCam.depth = mainCam.depth;
		whiteCamGameObject.transform.SetParent( gameObject.transform, true );

		blackCamGameObject = (GameObject) new GameObject();
		blackCamGameObject.name = "Black Background Camera";
		blackCam = blackCamGameObject.AddComponent<Camera>();
		blackCam.CopyFrom( mainCam );
		blackCam.backgroundColor = Color.black;
		blackCam.depth = mainCam.depth;
		blackCamGameObject.transform.SetParent( gameObject.transform, true );
	}

	void WriteScreenImageToTexture( Texture2D tex ){
		tex.ReadPixels( new Rect ( 0, 0, CameraWidth, CameraHeight ), 0, 0 );
		tex.Apply();
	}

	void CalculateOutputTexture(){
		Color color;
		for( int y = 0; y < textureTransparentBackground.height; ++y ){
			// each row
			for( int x = 0; x < textureTransparentBackground.width; ++x ){
				// each column
				float alpha = textureWhite.GetPixel( x, y ).r - textureBlack.GetPixel( x, y ).r;
				alpha = 1.0f - alpha;
				if( alpha == 0 ){
					color = Color.clear;

				}else{
					color = textureBlack.GetPixel( x, y ) / alpha;
				}
				color.a = alpha;
				textureTransparentBackground.SetPixel( x, y, color );
			}
		}
	}

	void SavePng(){
		string name = string.Format( "{0}/{1}.{2}", PathFolder, ImageName, imageExtension );
		var pngShot = textureTransparentBackground.EncodeToPNG();
		File.WriteAllBytes( name, pngShot );
	}

	void CacheAndInitialiseFields(){
		textureBlack = new Texture2D( CameraWidth, CameraHeight, TextureFormat.RGB24, false );
		textureWhite = new Texture2D( CameraWidth, CameraHeight, TextureFormat.RGB24, false );
		textureTransparentBackground = new Texture2D( CameraWidth, CameraHeight, TextureFormat.ARGB32, false );
	}
}