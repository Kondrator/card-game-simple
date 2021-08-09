using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class TransformBounds : MonoBehaviour {


	// boudns settings
	[SerializeField]
	private Vector3	boundsMin = new Vector3( 1f, 0.3f, 1f ),
					boundsMax = new Vector3( 1f, 1f, 1f );

	// used fields
	private Vector3 posBegin;

	
	public Vector3 BoundsMin{ get{ return boundsMin; } }
	public Vector3 BoundsMax{ get{ return boundsMax; } }


	public Vector3 size = Vector3.one;
	public Vector3 Size { get { return size; } }


	void Start(){
		
		FixBounds();

		posBegin = this.transform.position;

	}

	private void FixBounds(){
		
		boundsMin = new Vector3( Mathf.Abs( boundsMin.x ), Mathf.Abs( boundsMin.y ), Mathf.Abs( boundsMin.z ) );
		boundsMax = new Vector3( Mathf.Abs( boundsMax.x ), Mathf.Abs( boundsMax.y ), Mathf.Abs( boundsMax.z ) );

		size.x = boundsMin.x + boundsMax.x;
		size.y = boundsMin.y + boundsMax.y;
		size.z = boundsMin.z + boundsMax.z;

	}



	private Vector3 FixPoint( Vector3 point, Vector3 center ){
	
		Vector3 position = Vector3.zero;

		position.x = Mathf.Clamp( point.x,    center.x - boundsMin.x,    center.x + boundsMax.x );
		position.y = Mathf.Clamp( point.y,    center.y - boundsMax.y,    center.y + boundsMin.y );
		position.z = Mathf.Clamp( point.z,    center.z - boundsMin.z,    center.z + boundsMax.z );

		return position;
	}

	
	public Vector3 FixWorld( Vector3 pointInWorld ){

		return FixPoint( pointInWorld, this.transform.position );
	}
	public Vector3 FixWorldStatic( Vector3 pointInWorld ){

		return FixPoint( pointInWorld, posBegin );
	}
	public Vector3 FixLocal( Vector3 pointInLocal ){

		return FixPoint( pointInLocal, Vector3.zero );
	}


	/// <summary>
	/// Check point inside bounds.
	/// TRUE - point inside bounds.
	/// FALSE - point outside bounds.
	/// </summary>
	public bool CheckInsideWorld( Vector3 pointInWorld ){

		return FixPoint( pointInWorld, this.transform.position ).Equals( pointInWorld );
	}
	/// <summary>
	/// Check point inside bounds.
	/// TRUE - point inside bounds.
	/// FALSE - point outside bounds.
	/// </summary>
	public bool CheckInsideWorldStatic( Vector3 pointInWorld ){

		return FixPoint( pointInWorld, posBegin ).Equals( pointInWorld );
	}
	/// <summary>
	/// Check point inside bounds.
	/// TRUE - point inside bounds.
	/// FALSE - point outside bounds.
	/// </summary>
	public bool CheckInsideLocal( Vector3 pointInLocal ){

		return FixPoint( pointInLocal, Vector3.zero ).Equals( pointInLocal );
	}


	

#if UNITY_EDITOR

	void OnDrawGizmos(){
		
		Vector3 center = this.transform.position;
		if( Application.isPlaying
			&& this.isDrawForStatic == true
		){
			center = this.posBegin;
		}

		Vector3 boundsMin = new Vector3(	this.boundsMin.x * this.transform.lossyScale.x,
											this.boundsMin.y * this.transform.lossyScale.y,
											this.boundsMin.z * this.transform.lossyScale.z
										);
		Vector3 boundsMax = new Vector3(	this.boundsMax.x * this.transform.lossyScale.x,
											this.boundsMax.y * this.transform.lossyScale.y,
											this.boundsMax.z * this.transform.lossyScale.z
										);

		Vector3[] top = new Vector3[4];
		top[0] = new Vector3( center.x - boundsMin.x,    center.y + boundsMin.y,    center.z - boundsMin.z );
		top[1] = new Vector3( center.x - boundsMin.x,    center.y + boundsMin.y,    center.z + boundsMax.z );
		top[2] = new Vector3( center.x + boundsMax.x,    center.y + boundsMin.y,    center.z + boundsMax.z );
		top[3] = new Vector3( center.x + boundsMax.x,    center.y + boundsMin.y,    center.z - boundsMin.z );
			
		Vector3[] bottom = new Vector3[4];
		bottom[0] = new Vector3( center.x - boundsMin.x,    center.y - boundsMax.y,    center.z - boundsMin.z );
		bottom[1] = new Vector3( center.x - boundsMin.x,    center.y - boundsMax.y,    center.z + boundsMax.z );
		bottom[2] = new Vector3( center.x + boundsMax.x,    center.y - boundsMax.y,    center.z + boundsMax.z );
		bottom[3] = new Vector3( center.x + boundsMax.x,    center.y - boundsMax.y,    center.z - boundsMin.z );
			


		Handles.color = Color.magenta;
		GUI.enabled = !Application.isPlaying;

		DrawRectangle( top );
		DrawRectangle( bottom );
		for( int i = 0; i < 4; i++ ){
			Handles.DrawLine( top[i], bottom[i] );
		}

		Handles.color = Color.white;
		GUI.enabled = true;

	}
	
	private void DrawRectangle( Vector3[] points ){
			
		for( int i = 0; i < 4; i++ ){
			if( i == 0 ){
				Handles.DrawLine( points[0], points[3] );

			}else{
				Handles.DrawLine( points[i - 1], points[i] );
			}
		}

	}




	[SerializeField]
	private bool isDrawForStatic = false;

	[CustomEditor(typeof(TransformBounds))]
	private class TransformBoundsEditor : Editor{
		
		private TransformBounds bounds = null;
		private Vector3 boundsMin, boundsMax;
		void OnEnable(){
			bounds = base.target as TransformBounds;
			SaveBounds();
		}

		private void SaveBounds(){
			boundsMin = bounds.boundsMin;
			boundsMax = bounds.boundsMax;
		}

		public override void OnInspectorGUI(){

			GUI.enabled = !Application.isPlaying;
			base.OnInspectorGUI();
			GUI.enabled = true;

		}

	}
#endif



}
