using UnityEngine;
using System.Collections;
using Ads;


public static class MyOperationAds{
	
	public const string NOTIFY_LOADING_START =		"my.operation.ads.loading.start";
	public const string NOTIFY_LOADING_COMPLETED =	"my.operation.ads.loading.completed";

	

	private static IAds[] ADS = new IAds[]{
		#if UNITY_ANDROID
			//new AdsAdMob()
			new AdsUnity()

		#elif UNITY_IPHONE
			new AdsUnity()

		#endif
	};


	
	/// <summary>
	/// Показ обыкновенной рекламы, которую можно закрыть.
	/// </summary>
	/// <returns>
	/// True - реклама успешно открыта.
	/// False - не удалось открыть рекламу (не поддерживается).
	/// </returns>
	public static bool ShowSimple(){
		return ADS.RandomItem().ShowSimple();
	}

	/// <summary>
	/// Показ рекламы, за которую обычно выдают рекламы (нельзя закрыть).
	/// </summary>
	/// <param name="succesfully">Слушатель - успешный, полный просмотр рекламы.</param>
	/// <param name="skipped">Слушатель - пропуск рекламы.</param>
	/// <param name="failed">Слушатель - ошибка при показе релкамы.</param>
	/// <returns>
	/// True - реклама успешно открыта.
	/// False - не удалось открыть рекламу (не поддерживается).
	/// </returns>
	public static bool ShowReward( System.Action succesfully, System.Action skipped = null, System.Action failed = null ){
		return ADS.RandomItem().ShowReward( succesfully, skipped, failed );
	}

}
