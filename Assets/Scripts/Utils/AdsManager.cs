using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Utils
{
    public static class AdsManager
    {
        private static BannerView _bannerView;
        public static void InitializeAds()
        {
            MobileAds.Initialize(initStatus => { });
            RequestBanner();
        }
        private static void RequestBanner()
        {
#if UNITY_ANDROID
            const string adUnitId = "ca-app-pub-7736667654146587/3257057849";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

            // Create a 320x50 banner at the top of the screen.
            _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
            // Called when an ad request has successfully loaded.
            _bannerView.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            _bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            _bannerView.OnAdOpening += HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            _bannerView.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            _bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            _bannerView.LoadAd(request);
        }

        private static void HandleOnAdLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLoaded event received");
        }

        private static void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                                + args.Message);
        }

        private static void HandleOnAdOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdOpened event received");
        }

        private static void HandleOnAdClosed(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdClosed event received");
        }

        private static void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLeavingApplication event received");
        }
    }
}
