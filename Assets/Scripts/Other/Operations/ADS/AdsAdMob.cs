/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace Ads{

	public class AdsAdMob : IAds {

		private string AdID{
			get{
				#if UNITY_ANDROID
					// test ad
					//return "ca-app-pub-3940256099942544/5224354917";
					// real ad
					return "ca-app-pub-5179809256489539/7900027742";

				#elif UNITY_IPHONE
					return "unused";

				#else
					return "unexpected_platform";
				#endif
			}
		}


		
		private RewardedAd rewardedAd = null;


		private enum Status {
			None,
			InProgress,
			Success,
			Skip,
			Fail,
		}

		private Status status = Status.None;
		private System.Action	success = null,
						skipped = null,
						failed = null;
		private TimerExecutor.Item timerStatus = null;


		
		~AdsAdMob(){
			ClearInstance();
		}


		private void GetInstance(){
			if( rewardedAd != null ){
				return;
			}

			this.rewardedAd = new RewardedAd( AdID );

			// Called when an ad request has successfully loaded.
			rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
			// Called when an ad request failed to load.
			rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
			// Called when an ad is shown.
			rewardedAd.OnAdOpening += HandleRewardedAdOpening;
			// Called when an ad request failed to show.
			rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
			// Called when the user should be rewarded for interacting with the ad.
			rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
			// Called when the ad is closed.
			rewardedAd.OnAdClosed += HandleRewardedAdClosed;
		}

		private void ClearInstance(){
			if( rewardedAd != null ){
				rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
				rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
				rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
				rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
				rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
				rewardedAd.OnAdClosed -= HandleRewardedAdClosed;

				rewardedAd = null;
			}
		}

		

		public void HandleRewardedAdLoaded( object sender, EventArgs args ){
			ShowReward( success, skipped, failed );
		}

		public void HandleRewardedAdFailedToLoad( object sender, AdErrorEventArgs args ){
			if( status == Status.InProgress ){
				status = Status.Fail;
			}
		}

		public void HandleRewardedAdOpening( object sender, EventArgs args ){
			
		}

		public void HandleRewardedAdFailedToShow( object sender, AdErrorEventArgs args ){
			if( status == Status.InProgress ){
				status = Status.Fail;
			}
		}

		public void HandleRewardedAdClosed( object sender, EventArgs args ){
			if( status == Status.InProgress ){
				status = Status.Skip;
			}
		}

		public void HandleUserEarnedReward( object sender, Reward args ){
			if( status == Status.InProgress ){
				status = Status.Success;
			}
		}




		

		private void StartTimerStatus(){
			timerStatus = TimerExecutor.Add( 0.5f,
				() => {
					timerStatus = null;

					switch( status ){
						case Status.None:
							Complete( null );
							break;

						case Status.InProgress:
							StartTimerStatus();
							break;

						case Status.Success:
							Complete( success );
							break;

						case Status.Skip:
							Complete( skipped );
							break;

						case Status.Fail:
							Complete( failed );
							break;
					}
				}
			);
		}
		private void StopTimerStatus(){
			if( timerStatus != null ){
				timerStatus.ForceComplete();
				timerStatus = null;
			}
		}

		private void Complete( System.Action callback ){
			if( callback != null ){
				callback();
			}
			success = skipped = failed = null;
			ClearInstance();
			CyberPiggyApplication.NotifySingleton( MyOperationAds.NOTIFY_LOADING_COMPLETED );
		}




	
		public bool ShowSimple(){
			return false;
		}

		public bool ShowReward( System.Action success, System.Action skipped = null, System.Action failed = null ){
			
			this.success = success;
			this.skipped = skipped;
			this.failed = failed;

			GetInstance();

			if( rewardedAd.IsLoaded() == false ){
				CyberPiggyApplication.NotifySingleton( MyOperationAds.NOTIFY_LOADING_START );
				
				AdRequest request = new AdRequest.Builder()
									.AddTestDevice( "D088D941E4D98FE4A3E376A487B5EA6D" ) // this is for test ads on test device
									.TagForChildDirectedTreatment( true )
									.Build();
				rewardedAd.LoadAd( request );
				return false;

			}else{
				status = Status.InProgress;
				StartTimerStatus();
				rewardedAd.Show();
				return true;
			}
		}


	}

}*/