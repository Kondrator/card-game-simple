using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIWindowManager;


public class UIListSelect : UIWindowListArray<UIListSelectItem, DBItem> {


	private DBItem[] dbItems = null;
	private System.Action<DBItem> select = null;
	private int idSelect = -1;



	protected override void OnAwake() {
		base.OnAwake();

		OnClose.AddListener( () => {
			dbItems = null;
			select = null;
			idSelect = -1;
		} );

		OnCloseAnimationCompleted.AddListener( () => {
			Clear();
		} );
	}


	protected override void SettingItemExtra( UIListSelectItem container, DBItem item, int index ){
		container.Set( item, select );
		container.SetSelect( item.ID == idSelect );
	}



	public void Show( DBItem[] dbItems, System.Action<DBItem> select, int idSelect = -1 ) {
		this.dbItems = dbItems;
		this.select = select;
		this.idSelect = idSelect;

		base.Open();
	}


	public void SetSelect( int idSelect ) {
		this.idSelect = idSelect;

		for( int i = 0; i < GetCount(); i++ ) {
			GetItemUI( i ).SetSelect( GetItemData( i ).ID == idSelect );
		}
	}



	protected override DBItem[] GetItems(){
		return dbItems ?? new DBItem[0];
	}

}