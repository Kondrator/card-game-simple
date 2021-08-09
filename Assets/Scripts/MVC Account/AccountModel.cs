using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Kondrat.MVC;


public class AccountModel : Model {

	public class NOTIFY {

		public class CURRENCY {

			public class CHANGED {

				public const string NAME = "account.info.currency.changed";


				/// <summary>
				/// Type = bool
				/// </summary>
				public const string PARAM_IS_INCREASE = "increase";


				public static string NAME_ID( int id ) {
					return CURRENCY.NAME_ID( NAME, id );
				}
			}


			public class NOT_ENOUGH {

				public const string NAME = "account.info.currency.not.enough";


				/// <summary>
				/// Type = int
				/// </summary>
				public const string PARAM_VALUE_NOT_ENOUGH = "value.not.enough";


				public static string NAME_ID( int id ) {
					return CURRENCY.NAME_ID( NAME, id );
				}
			}



			/// <summary>
			/// Type = int (ID of CurrencyItem)
			/// </summary>
			public const string PARAM_ID = "id";

			/// <summary>
			/// Type = InfinityNumber
			/// </summary>
			public const string PARAM_VALUE = "value";



			protected static string NAME_ID( string name, int id ) {
				return name.Replace( "currency", string.Format( "currency.{0}", id ) );
			}

		}

	}



	private static AccountInfo accountInfo = null;
	public AccountInfo AccountInfo{
		get{
			LoadAccountInfoIfNeed();
			return accountInfo;
		}
	}







#region FILE DATA

	private string PATH_FILE_ACCOUNT{
		get{
			return Application.persistentDataPath + "/account.data";
		}
	}

	/// <summary>
	/// Create new account info with empty data.
	/// </summary>
	public void CreateAccountInfo(){
		accountInfo = new AccountInfo();
	}

	/// <summary>
	/// Clear current session.
	/// </summary>
	public void ClearSession(){
		accountInfo = new AccountInfo();
		SaveAccountInfo();
	}

	/// <summary>
	/// Load data account.
	/// </summary>
	public void LoadAccountInfo(){
		
		// lcoal
		accountInfo = new AccountInfo();

		if( File.Exists( PATH_FILE_ACCOUNT ) ){
			try{
				string content = File.ReadAllText( PATH_FILE_ACCOUNT );
				if( string.IsNullOrEmpty( content ) ){
					return;
				}
				content = Encoding.Default.DecodeBase64( content );
				accountInfo.FromJSON( (JObject)JsonConvert.DeserializeObject( content ) );

			} catch( JsonException exception ){
#if UNITY_EDITOR
				Debug.LogError( "JSON ERROR: <b>" + exception.Message + "</b>\n" + exception.StackTrace );
#endif
			}
		}

	}

	/// <summary>
	/// If data account not have - load.
	/// Else to do nothing.
	/// </summary>
	public void LoadAccountInfoIfNeed(){
		
		if( accountInfo == null ){
			LoadAccountInfo();
		}

	}

	/// <summary>
	/// Save data account.
	/// </summary>
	public void SaveAccountInfo(){

		string content = Encoding.Default.EncodeBase64( AccountInfo.ToString() );
		File.WriteAllText( PATH_FILE_ACCOUNT, content );

	}

#endregion

}
