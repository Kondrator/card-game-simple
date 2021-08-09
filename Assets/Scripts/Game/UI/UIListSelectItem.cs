using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIWindowManager;


public class UIListSelectItem : MonoBehaviour {

	[SerializeField]
	private Image image = null;

	[SerializeField]
	private Button button = null;

	[SerializeField]
	private Transform containerSelect = null;



	private DBItem data = null;
	private System.Action<DBItem> select;



	private void Start() {
		button.AddListenerOnClick( () => {
			select?.Invoke( data );
		} );
	}



	public void Set( DBItem data, System.Action<DBItem> select ){
		this.data = data;
		this.select = select;

		this.data.IconUI.Get( image.SetSprite );
	}


	public void SetSelect( bool isSelect ) {
		containerSelect?.SetActive( isSelect );
	}


}