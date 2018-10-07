// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections.Generic;
using UnityEngine;
using nRed.ActionUtil;
using nRed.CollectionUtil;
using nRed.ResultUtil;
#if NRED_FIREBASE_ADMOB_UTIL
using GoogleMobileAds.Api;
#endif
namespace nRed.FirebaseUtil
{
	//==============================================================================
	/**
	 *	@brief	Adob Helper class
	 */
	//==============================================================================
	// https://developers.google.com/admob/unity/
	public class AdmobComponent : MonoBehaviour, IFirebaseComponent
	{

// E N U M / S E T T I N G   C L A S S E S
//##########################################################################
		public enum COPPA
		{
			NO_SETTING_COPPA,
			COPPA_ENABLE,
			COPPA_DISABLE,
		}

		public enum MaxAdRating
		{
			None,
			G,
			PG,
			T,
			MA,
		}

		[System.Serializable]
		public class PlatformSettings
		{
			public string appId;
			public string[] bannerId;
			public string[] intersticialId;
			public string[] rewardId;
			public Targeting targeting;
			public MaxAdRating maxAdRating;

			public class Targeting
			{
				public COPPA coppa;
				public bool disablePersonalized;
			}

		}

// V A L U E S
//##########################################################################

		public PlatformSettings ios;
		public PlatformSettings android;
		public PlatformSettings editor;

		#if NRED_FIREBASE_ADMOB_UTIL
		private Dictionary<string,BannerView> bannerViewDic = new Dictionary<string,BannerView>(16);
		private InterstitialAd interstitialAd;
		private string lastInterstitialId;
		private RewardBasedVideoAd rewardBasedVideoAd;
		private string lastRewardVideoId;
		protected string appId;
		public string[] testDeviceId;
		#endif

// P R O P E R T I E S
//##########################################################################

		public bool isLoaded{get; private set;}

		#if NRED_FIREBASE_ADMOB_UTIL
		// Specifical test id for banner.
		// https://developers.google.com/admob/unity/test-ads
		public string[] testBannerId
		{
			get
			{
				#if UNITY_ANDROID
				return new string[]{"ca-app-pub-3940256099942544/6300978111"};
				#elif UNITY_IPHONE
				return new string[]{"ca-app-pub-3940256099942544/2934735716"};
				#else
				return new string[]{""};
				#endif
			}
		}

		// Specifical test id for Intersticial.
		public string[] testIntersticialId
		{
			get
			{
				#if UNITY_ANDROID
				return new string[]{"ca-app-pub-3940256099942544/1033173712"};
				#elif UNITY_IPHONE
				return new string[]{"ca-app-pub-3940256099942544/4411468910"};
				#else
				return new string[]{""};
				#endif
			}
		}

		// Specifical test id for rewards.
		public string[] testRewardId
		{
			get
			{
				#if UNITY_ANDROID
				return new string[]{"ca-app-pub-3940256099942544/5224354917"};
				#elif UNITY_IPHONE
				return new string[]{"ca-app-pub-3940256099942544/1712485313"};
				#else
				return new string[]{""};
				#endif
			}
		}

		// Make and get ad request instance.
		private AdRequest request
		{
			get
			{
				var b = new AdRequest.Builder();
				PlatformSettings settings = null;
				#if UNITY_IPHONE
					settings = ios;
				#elif UNITY_ANDROID
					settings = android;
				#else
					settings = editor;
				#endif

				#if DEBUG
					if( testDeviceId.IsAvailable() )
					{
						foreach( var testid in testDeviceId )
						{
							b.AddTestDevice(testid);
							settings.bannerId		= testBannerId;
							settings.intersticialId = testIntersticialId;
							settings.rewardId		= testRewardId;
						}
					}
				#endif

				// Advanced settings
				// https://developers.google.com/admob/unity/targeting
				if( settings!=null )
				{
					// COPPA
					if( settings.targeting.coppa==COPPA.COPPA_ENABLE )
					{
						b.TagForChildDirectedTreatment(true);
					}
					else if( settings.targeting.coppa==COPPA.COPPA_DISABLE )
					{
						b.TagForChildDirectedTreatment(false);
					}

					// Under age of consent
					if( settings.targeting.disablePersonalized )
					{
						//Note: This tag_for_under_age_of_consent parameter is currently NOT forwarded to ad network mediation adapters.
						//      It is your responsibility to ensure that each third-party ad network
						//      in your application serves ads that are appropriate for users under the age of consent per GDPR.
						b.AddExtra("tag_for_under_age_of_consent", "true");
					}

					// Ad content filtering
					if( settings.maxAdRating!=MaxAdRating.None )
					{
						b.AddExtra("max_ad_content_rating", settings.maxAdRating.ToString());
					}
				}

				return b.Build ();
				
			}
		}

		// Set below that you can custom callbacks on each events.
		// Must be set before called Initialize.
		public System.Action onBannerLoaded				{ set; private get; }
		public System.Action onBannerLoadFailed			{ set; private get; }
		public System.Action onBannerOpening			{ set; private get; }
		public System.Action onBannerClosed				{ set; private get; }
		public System.Action onBannerTouchedWithAppQuit	{ set; private get; }
		public System.Action onInterstitialLoaded		{ set; private get; }
		public System.Action onInterstitialLoadFailed	{ set; private get; }
		public System.Action onInterstitialOpening		{ set; private get; }
		public System.Action onInterstitialClosed		{ set; private get; }
		public System.Action onInterstitialTouchedWithAppQuit { set; private get; }
		public System.Action<Reward> onRewardedCallback	{ set; private get; }
		public System.Action onRewardLoaded				{ set; private get; }
		public System.Action onRewardLoadFailed			{ set; private get; }
		public System.Action onRewardStarted			{ set; private get; }
		public System.Action onRewardOpening			{ set; private get; }
		public System.Action onRewardVideoClosed		{ set; private get; }
		public System.Action onRewardTouchedWithAppQuit	{ set; private get; }
		
// M E T H O D S
//##########################################################################

		//===============================================================
		/**
		 *	@brief	Awake event
		 */
		//===============================================================
		private void Awake()
		{
			DontDestroyOnLoad (this);
		}
		#endif

		//===============================================================
		/**
		 *	@brief	Initialize this modele
		 *
		 *	@param	onSuccess	Callback on success.
		 *	@param	onFault		Callback on fault.
		 */
		//===============================================================
		public void Initialize( Action onSuccess=null, Action<IResultBase> onFault=null )
		{
			if( isLoaded )
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=ErrorCode.ALREADY_LOADED } );
				return;
			}

			#if NRED_FIREBASE_ADMOB_UTIL
			try
			{
				MobileAds.Initialize(appId);
				bannerViewDic.Clear();
				interstitialAd = null;
				InitializeRewardVideo();
				isLoaded = true;
				onSuccess.SafeCall();
			}
			catch(  System.Exception e )
			{
				DebugUtil.D.Log( string.Format("[Firebase][AdmobComponent]\n\tInitialize error:{0}", e.ToString()) );
				onFault.SafeCall( new Result{ ok=false, errorCode=ErrorCode.INIT_EXCEPTION } );
			}
			#endif
		}

		#if NRED_FIREBASE_ADMOB_UTIL
		// B A N N E R
		//##################################################################################
		// https://developers.google.com/admob/unity/banner

		//===============================================================
		/**
		 *	@brief	Rewuest banner ad
		 *
		 *	@param	adUnitId	Your ad unit id.
		 *	@param	position	Display banner position by AdPosition.
		 */
		//===============================================================
		public void RequestBanner(string adUnitId, AdPosition position)
		{
	        bannerViewDic[adUnitId] = new BannerView (adUnitId, AdSize.Banner, position);

			// Called when an ad request has successfully loaded.
			bannerViewDic[adUnitId].OnAdLoaded += OnBannerLoaded;
			// Called when an ad request failed to load.
			bannerViewDic[adUnitId].OnAdFailedToLoad += OnBannerLoadFailed;
			// Called when an ad is clicked.
			bannerViewDic[adUnitId].OnAdOpening += OnBannerOpening;
			// Called when the user returned from the app after an ad click.
			bannerViewDic[adUnitId].OnAdClosed += OnBannerClosed;
			// Called when the ad click caused the user to leave the application.
			bannerViewDic[adUnitId].OnAdLeavingApplication += OnBannerTouchedWithAppQuit;
			
			bannerViewDic[adUnitId].LoadAd (request);
		}

		//===============================================================
		/**
		 *	@brief	Show loaded banner.
		 *
		 *	@param	adUnitId	Your ad unit id.
		 */
		//===============================================================
		public void ShowBanner(string adUnitId)
		{
			if(bannerViewDic.ContainsKey(adUnitId) && bannerViewDic[adUnitId]!=null)
				bannerViewDic[adUnitId].Show();
		}
		
		//===============================================================
		/**
		 *	@brief	Hide loaded banner.
		 *
		 *	@param	adUnitId	Your ad unit id.
		 */
		//===============================================================
		public void HideBanner(string adUnitId)
		{
			if(bannerViewDic.ContainsKey(adUnitId) && bannerViewDic[adUnitId]!=null)
				bannerViewDic[adUnitId].Hide();
		}

		//===============================================================
		/**
		 *	@brief	Destroy loaded banner.
		 *
		 *	@param	adUnitId	Your ad unit id.
		 */
		//===============================================================
		public void DestroyBanner(string adUnitId)
		{
			if(bannerViewDic.ContainsKey(adUnitId) && bannerViewDic[adUnitId]!=null)
			{
				bannerViewDic[adUnitId].Destroy();
				bannerViewDic[adUnitId] = null;
				bannerViewDic.Remove(adUnitId);
			}
		}
		
		//===============================================================
		/**
		 *	@brief	Banner loaded event.
		 */
		//===============================================================
		private void OnBannerLoaded(object sender, System.EventArgs args)
		{
			onBannerLoaded.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Banner load failed event.
		 */
		//===============================================================
		protected void OnBannerLoadFailed(object sender, AdFailedToLoadEventArgs args)
		{
			onBannerLoadFailed.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Banner opening event.
		 */
		//===============================================================
		private void OnBannerOpening(object sender, System.EventArgs args)
		{
			onBannerOpening.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Banner closed event.
		 */
		//===============================================================
				
		private void OnBannerClosed (object sender, System.EventArgs e)
		{
			onBannerClosed.SafeCall();
		}
		
		//===============================================================
		/**
		 *	@brief	Banner touched and app quit event.
		 */
		//===============================================================
		protected void OnBannerTouchedWithAppQuit(object sender, System.EventArgs args)
		{
			onBannerTouchedWithAppQuit.SafeCall();
		}
		//##################################################################################



		// I N T E R S T I T I A L
		//##################################################################################
		// https://developers.google.com/admob/unity/interstitial

		//===============================================================
		/**
		 *	@brief	Request interstitial ad.
		 *
		 *	@param	adUnitId	Your ad unit id.
		 */
		//===============================================================
		public void RequestInterstitial (string adUnitId)
		{
			lastInterstitialId = adUnitId;
	        interstitialAd = new InterstitialAd(adUnitId);

			// Called when an ad request has successfully loaded.
			interstitialAd.OnAdLoaded += OnInterstitialLoaded;
			// Called when an ad request failed to load.
			interstitialAd.OnAdFailedToLoad += OnInterstitialLoadFailed;
			// Called when an ad is shown.
			interstitialAd.OnAdOpening += OnInterstitialOpening;
			// Called when the ad is closed.
			interstitialAd.OnAdClosed += OnInterstitialClosed;
			// Called when the ad click caused the user to leave the application.
			interstitialAd.OnAdLeavingApplication += OnInterstitialTouchedWithAppQuit;

			interstitialAd.LoadAd (request);
		}

		//===============================================================
		/**
		 *	@brief	Show loaded intersticial.
		 */
		//===============================================================
		public void ShowInterstitial()
		{
			if( interstitialAd!=null )
			{
				interstitialAd.Show();
			}
		}

		//===============================================================
		/**
		 *	@brief	Destroy loaded intersticial.
		 */
		//===============================================================
		public void DestroyInterstitial()
		{
			if(interstitialAd!=null)
			{
				interstitialAd.OnAdLoaded				-= OnInterstitialLoaded;
				interstitialAd.OnAdFailedToLoad			-= OnInterstitialLoadFailed;
				interstitialAd.OnAdOpening				-= OnInterstitialOpening;
				interstitialAd.OnAdClosed				-= OnInterstitialClosed;
				interstitialAd.OnAdLeavingApplication	-= OnInterstitialTouchedWithAppQuit;
				interstitialAd.Destroy ();
			}
		}

		//===============================================================
		/**
		 *	@brief	Interstitial loaded event.
		 */
		//===============================================================
		private void OnInterstitialLoaded(object sender, System.EventArgs args)
		{
			onInterstitialLoaded.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Intersticial load failed event.
		 */
		//===============================================================
		protected void OnInterstitialLoadFailed(object sender, AdFailedToLoadEventArgs args)
		{
			onInterstitialLoadFailed.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Intersticial opening event.
		 */
		//===============================================================
		private void OnInterstitialOpening(object sender, System.EventArgs args)
		{
			onInterstitialOpening.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Intersticial loaded event.
		 */
		//===============================================================
		private void OnInterstitialClosed (object sender, System.EventArgs e)
		{
			DestroyInterstitial ();
			RequestInterstitial (lastInterstitialId);
		}

		//===============================================================
		/**
		 *	@brief	Intersticial touched and app quit event.
		 */
		//===============================================================
		protected void OnInterstitialTouchedWithAppQuit(object sender, System.EventArgs args)
		{
			onInterstitialTouchedWithAppQuit.SafeCall();
		}
		//##################################################################################



		// V I D E O   R E W A R D
		//##################################################################################
		// https://developers.google.com/admob/unity/rewarded-video

		//===============================================================
		/**
		 *	@brief	Initialize reward video ad.
		 */
		//===============================================================
		private void InitializeRewardVideo()
		{
			rewardBasedVideoAd = RewardBasedVideoAd.Instance;			
			// Called when an ad request has successfully loaded.
			rewardBasedVideoAd.OnAdLoaded				+= OnRewardLoaded;
			// Called when an ad request failed to load.
			rewardBasedVideoAd.OnAdFailedToLoad 		+= OnRewardLoadFailed;
			// Called when an ad is shown.
			rewardBasedVideoAd.OnAdOpening				+= OnRewardOpening;
			// Called when the ad starts to play.
			rewardBasedVideoAd.OnAdStarted				+= OnRewardStarted;
			// Called when the user should be rewarded for watching a video.
			rewardBasedVideoAd.OnAdRewarded				+= OnVideoRewarded;
			// Called when the ad is closed.
			rewardBasedVideoAd.OnAdClosed				+= OnRewardVideoClosed;
			// Called when the ad click caused the user to leave the application.
			rewardBasedVideoAd.OnAdLeavingApplication	+= OnRewardTouchedWithAppQuit;
		}

		//===============================================================
		/**
		 *	@brief	Request reward video ad.
		 *
		 *	@brief	Your ad unit id.
		 */
		//===============================================================
		public void RequestRewardVideo (string adUnitId)
		{
			lastRewardVideoId = adUnitId;
	        rewardBasedVideoAd = RewardBasedVideoAd.Instance;
			
			rewardBasedVideoAd.LoadAd (request, adUnitId);
		}
		
		//===============================================================
		/**
		 *	@brief	Destroy reward video ad.
		 */
		//===============================================================
		public void DestroyRewardVideo()
		{
			rewardBasedVideoAd.OnAdLoaded				-= OnRewardLoaded;
			rewardBasedVideoAd.OnAdFailedToLoad 		-= OnRewardLoadFailed;
			rewardBasedVideoAd.OnAdOpening				-= OnRewardOpening;
			rewardBasedVideoAd.OnAdStarted				-= OnRewardStarted;
			rewardBasedVideoAd.OnAdRewarded				-= OnVideoRewarded;
			rewardBasedVideoAd.OnAdClosed				-= OnRewardVideoClosed;
			rewardBasedVideoAd.OnAdLeavingApplication	-= OnRewardTouchedWithAppQuit;
		}

		//===============================================================
		/**
		 *	@brief	Show reward video ad.
		 *
		 *	@param	onRewardedCallback			Rewarded callback event.
		 *	@param	onNotRewardedCallback		Not rewarded callback event.
		 *	@param	onRewardTouchedWithAppQuit	Touch rewarded and app quit callback.
		 */
		//===============================================================
		public void ShowRewardVideo( System.Action<Reward> onRewardedCallback, System.Action onNotRewardedCallback, System.Action onRewardTouchedWithAppQuit )
		{
			if( rewardBasedVideoAd!=null && rewardBasedVideoAd.IsLoaded() )
			{
				rewardBasedVideoAd.Show();
				this.onRewardedCallback			= onRewardedCallback;
				this.onRewardVideoClosed		= onNotRewardedCallback;
				this.onRewardTouchedWithAppQuit	= onRewardTouchedWithAppQuit;
			}
		}
		
		//===============================================================
		/**
		 *	@brief	Reward loaded callback.
		 */
		//===============================================================
		private void OnRewardLoaded(object sender, System.EventArgs args)
		{
			onRewardLoaded.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Reward load failed callback.
		 */
		//===============================================================
		protected void OnRewardLoadFailed(object sender, AdFailedToLoadEventArgs args)
		{
			onRewardLoadFailed.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Reward started callback.
		 */
		//===============================================================
		private void OnRewardStarted(object sender, System.EventArgs args)
		{
			onRewardStarted.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Reward opening callback.
		 */
		//===============================================================
		private void OnRewardOpening(object sender, System.EventArgs args)
		{
			onRewardOpening.SafeCall();
		}

		//===============================================================
		/**
		 *	@brief	Reward video closed callback.
		 */
		//===============================================================
		private void OnRewardVideoClosed (object sender, System.EventArgs e)
		{
			onRewardVideoClosed.SafeCall();
			RequestRewardVideo (lastRewardVideoId);
		}

		//===============================================================
		/**
		 *	@brief	Rewarded callback.
		 */
		//===============================================================
		private void OnVideoRewarded(object sender, Reward args)
		{
			string type = args.Type;
			double amount = args.Amount;

			DebugUtil.D.Log( string.Format("User rewarded with: {0} Type:{1}", amount.ToString(), type) );

			onRewardedCallback.SafeCall(args);
			RequestRewardVideo (lastRewardVideoId);
		}
		
		//===============================================================
		/**
		 *	@brief	Reward touched and app quit callback.
		 */
		//===============================================================
		protected void OnRewardTouchedWithAppQuit(object sender, System.EventArgs args)
		{
			onRewardTouchedWithAppQuit.SafeCall();
		}
		//##################################################################################
		#endif
	}

}