using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIWindowManager;
using Kondrat.MVC;


public class AccountController : Controller<AccountModel, AccountView> {

	public const string NOTIFY_ACCOUNT_SAVE_DATA = "account.save.data";



	protected override void PreInitiate() {}

	protected override void Initiate() {

		Add(
			SceneController.NOTIFY_LOADED,
			( NotifyData data ) => {
				Model.LoadAccountInfoIfNeed();
				Model.SaveAccountInfo();
			}
		);


		Add(
			new string[] { SceneController.NOTIFY_DESTROYED, UIWindowMenu.NOTIFY_QUIT_GAME },
			( NotifyData data ) => {
				Model.SaveAccountInfo();
			}
		);


		Add(
			AccountController.NOTIFY_ACCOUNT_SAVE_DATA,
			( NotifyData data ) => {
				Model.SaveAccountInfo();
			}
		);


	}

}
