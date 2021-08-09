using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Advertisements;


namespace Ads{

	public class AdsUnity : IAds{
	
		public bool ShowSimple(){
#if UNITY_ANDROID || UNITY_IOS
			/*
			if( Advertisement.IsReady() ){
				// показ рекламы
				Advertisement.Show();
				return true;
			}
			*/
			return false;
#else
			return false;
#endif
		}

		public bool ShowReward( System.Action success, System.Action skipped = null, System.Action failed = null ){
#if UNITY_ANDROID || UNITY_IOS
			/*
			if( Advertisement.IsReady( "rewardedVideo" ) ) {
				// обработка callback вызовов показа рекламы
				ShowOptions options = new ShowOptions{ resultCallback = ( ShowResult result ) => {
					switch( result ){
						// все супер
						case ShowResult.Finished:
							if( success != null ){
								success();
							}
							break;
						
						// пропустил - негодяй
						case ShowResult.Skipped:
							if( skipped != null ){
								skipped();
							}
							break;

						// странная ошибочка
						case ShowResult.Failed:
							if( failed != null ){
								failed();
							}
							break;
					}
				} };
				// показ рекламы
				Advertisement.Show( "rewardedVideo", options );
				return true;
			}
			*/
			return false;
#else
			return false;
#endif
		}

	}

}