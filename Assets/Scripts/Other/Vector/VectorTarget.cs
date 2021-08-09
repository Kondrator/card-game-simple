using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VectorTarget{
		
	private Transform parent = null;
	public Transform Parent{ get{ return parent; } }

	private Transform targetInWorld = null;
	private Transform targetInUI = null;
	private Vector3? targetPointInScreen = null;


	public Vector3 Position{
		get{
			if( targetInWorld != null ){
				return targetInWorld.position;
			}
			if( targetInUI != null ){
				return targetInUI.position;
			}
			if( targetPointInScreen.HasValue == true ){
				return targetPointInScreen.Value;
			}
			return Vector3.zero;
		}
	}
	public Vector3 PositionFix{
		get{
			if( targetInWorld != null ){
				return targetInWorld.position;
			}
			if( targetInUI != null ){
				return GetPositionFixInScreen( targetInUI.position );
			}
			if( targetPointInScreen.HasValue == true ){
				return GetPositionFixInScreen( targetPointInScreen.Value );
			}
			return Vector3.zero;
		}
	}


	public Vector3 PositionInScreen{
		get{
			if( targetInWorld != null ){
				return Camera.main.WorldToScreenPoint( targetInWorld.position );
			}
			if( targetInUI != null ){
				return targetInUI.position;
			}
			if( targetPointInScreen.HasValue == true ){
				return targetPointInScreen.Value;
			}
			return Vector3.zero;
		}
	}
	public Vector3 PositionInScreenFix{
		get{
			if( targetInWorld != null ){
				return GetPositionFixInScreen( Camera.main.WorldToScreenPoint( targetInWorld.position ) );
			}
			if( targetInUI != null ){
				return GetPositionFixInScreen( targetInUI.position );
			}
			if( targetPointInScreen.HasValue == true ){
				return GetPositionFixInScreen( targetPointInScreen.Value );
			}
			return Vector3.zero;
		}
	}

	public bool IsHave{
		get{
			if( targetInWorld != null
				|| targetInUI != null
				|| targetPointInScreen.HasValue == true
			){
				return true;
			}
			return false;
		}
	}
		
	public bool IsWorld{ get{ return targetInWorld != null; } }
	public bool IsUI{ get{ return targetInUI != null; } }
	public bool IsScreen{ get{ return targetPointInScreen.HasValue; } }


	private VectorTarget(){}

	public void SetParent( Transform parent ){
		this.parent = parent;
	}
	public Vector3 GetPositionFixInScreen( Vector3 position ){
		if( parent != null ){
			position.x /= parent.lossyScale.x;
			position.y /= parent.lossyScale.y;
			position.z /= parent.lossyScale.z;
		}
		return position;
	}


	/// <summary>
	/// Target is Game Object in UI.
	/// </summary>
	public static VectorTarget FromWorld( Transform targetInWorld ){
		VectorTarget target = new VectorTarget();
		target.targetInWorld = targetInWorld;
		return target;
	}

	/// <summary>
	/// Target is Game Object in UI.
	/// </summary>
	public static VectorTarget FromUI( Transform targetInUI ){
		VectorTarget target = new VectorTarget();
		target.targetInUI = targetInUI;
		return target;
	}

	/// <summary>
	/// Target is point in screen.
	/// </summary>
	public static VectorTarget FromScreen( Vector3 targetPointInScreen ){
		VectorTarget target = new VectorTarget();
		target.targetPointInScreen = targetPointInScreen;
		return target;
	}

	/// <summary>
	/// Target is world point to screen.
	/// </summary>
	public static VectorTarget FromWorldToScreen( Vector3 targetPointInWorld ){
		VectorTarget target = new VectorTarget();
		target.targetPointInScreen = Camera.main.WorldToScreenPoint( targetPointInWorld );
		return target;
	}

}