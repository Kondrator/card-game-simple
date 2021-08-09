using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ColliderEvent : MonoBehaviour {

	public UnityEvent OnDown = new UnityEvent();
	public UnityEvent OnUp = new UnityEvent();
	public UnityEvent OnClick = new UnityEvent();

	public enum Direction {
		Left = 0,
		Top = 1,
		Right = 2,
		Bottom = 3,
	}
	public class UnityEventDirection : UnityEvent<Direction> { }
	public UnityEventDirection OnSwap = new UnityEventDirection();


	private Vector2? posDown = null;




	void OnMouseDown(){
		posDown = null;
		if( MyOperationUI.IsCursorOverUI() == false ){
			OnDown.Invoke();
			posDown = Input.mousePosition;
		}
	}

	void OnMouseUp(){
		if( posDown.HasValue == true ) {
			OnUp.Invoke();
			if( Vector2.Distance( posDown.Value, Input.mousePosition ) < 0.05f * Screen.width ){
				OnClick.Invoke();
			}
		}
		posDown = null;
	}

	private void OnMouseExit() {
		if( posDown.HasValue == true ) {
			Vector2 posDiff = (Vector2)Input.mousePosition - posDown.Value;

			if( Mathf.Abs( posDiff.x ) > Mathf.Abs( posDiff.y ) ) {
				if( posDiff.x < 0 ) {
					OnSwap.Invoke( Direction.Left );

				} else if( posDiff.x > 0 ) {
					OnSwap.Invoke( Direction.Right );

				}

			} else {
				if( posDiff.y < 0 ) {
					OnSwap.Invoke( Direction.Bottom );

				} else if( posDiff.y > 0 ) {
					OnSwap.Invoke( Direction.Top );
				}
			}
		}

		posDown = null;
	}

	private void OnDisable() {
		posDown = null;
	}




	/// <summary>
	/// Create ColliderEvent for all colliders in target
	/// </summary>
	/// <param name="target">Target</param>
	/// <param name="withChildren">Find colliders in children ?</param>
	/// <returns></returns>
	public static ColliderEvent[] Setting( Transform target, bool withChildren = true ){
		Collider[] colliders = target.GetComponentsInChildren<Collider>();
		ColliderEvent[] events = new ColliderEvent[colliders.Length];

		for( int i = 0; i < colliders.Length; i++ ){
			events[i] = colliders[i].AddComponent<ColliderEvent>();
		}

		return events;
	}

}



public static class ColliderEventExtensions {


	/// <summary>
	/// Add listener to event - On Down
	/// </summary>
	public static void AddOnDown( this ColliderEvent colliderEvent, UnityAction listener ) {
		colliderEvent.OnDown.AddListener( listener );
	}

	/// <summary>
	/// Add listener to event - On Down
	/// </summary>
	public static void AddOnDown( this ColliderEvent[] colliderEvent, UnityAction listener ) {
		for( int i = 0; i < colliderEvent.Length; i++ ) {
			colliderEvent[i].AddOnDown( listener );
		}
	}


	/// <summary>
	/// Add listener to event - On Up
	/// </summary>
	public static void AddOnUp( this ColliderEvent colliderEvent, UnityAction listener ) {
		colliderEvent.OnUp.AddListener( listener );
	}

	/// <summary>
	/// Add listener to event - On Up
	/// </summary>
	public static void AddOnUp( this ColliderEvent[] colliderEvent, UnityAction listener ) {
		for( int i = 0; i < colliderEvent.Length; i++ ) {
			colliderEvent[i].AddOnUp( listener );
		}
	}


	/// <summary>
	/// Add listener to event - On Click
	/// </summary>
	public static void AddOnClick( this ColliderEvent colliderEvent, UnityAction listener ) {
		colliderEvent.OnClick.AddListener( listener );
	}

	/// <summary>
	/// Add listener to event - On Click
	/// </summary>
	public static void AddOnClick( this ColliderEvent[] colliderEvent, UnityAction listener ) {
		for( int i = 0; i < colliderEvent.Length; i++ ) {
			colliderEvent[i].AddOnClick( listener );
		}
	}


	/// <summary>
	/// Add listener to event - On Swap
	/// </summary>
	public static void AddOnSwap( this ColliderEvent colliderEvent, UnityAction<ColliderEvent.Direction> listener ) {
		colliderEvent.OnSwap.AddListener( listener );
	}

	/// <summary>
	/// Add listener to event - On Swap
	/// </summary>
	public static void AddOnSwap( this ColliderEvent[] colliderEvent, UnityAction<ColliderEvent.Direction> listener ) {
		for( int i = 0; i < colliderEvent.Length; i++ ) {
			colliderEvent[i].AddOnSwap( listener );
		}
	}

}