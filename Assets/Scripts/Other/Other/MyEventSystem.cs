using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MyEventSystem : MonoBehaviour {


	private static MyEventSystem singleton_;
	public static MyEventSystem singleton{
		get{
			if( singleton_ == null ){
				EventSystem eventSystem = FindObjectOfType<EventSystem>();
				if( eventSystem == null ){
					return null;
				}
				eventSystem.AddComponent<MyEventSystem>();
			}
			return singleton_;
		}
	}


	public static EventSystem System{ get{ return singleton.system; } }


	private EventSystem system = null;

	public static bool Enabled{
		get{
			if( singleton != null ){
				return singleton.system.enabled;
			}
			return false;
		}
		set{
			if( singleton != null ){
				singleton.system.enabled = value;
			}
		}
	}

	void Awake(){
		system = GetComponent<EventSystem>();
		if( system == null ){
			Destroy( this );
			return;
		}

		singleton_ = this;
	}


}
