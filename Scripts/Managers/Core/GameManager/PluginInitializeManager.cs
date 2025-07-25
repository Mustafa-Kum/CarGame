using System;
using System.Collections;
using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Firebase.Extensions;
using GoogleMobileAds.Api;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.GameManager
{
    public class PluginInitializeManager : MonoBehaviour
    {
        #region Private Variables
        
        private bool _isAttAuthorized;
        private bool _isFirebaseInitialized;
        private bool _isFacebookInitialized;
        private bool _isFacebookActivated;
        private bool _isGoogleAdmobInitialized;
        public FirebaseApp App;
        private DependencyStatus _dependencyStatus = DependencyStatus.UnavailableOther;

        #endregion

        #region Unity Methods

        private void Start()
        {
            StartCoroutine(WaitForFirebaseInitialization());
            
            EnableMobileAds();
            
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                _dependencyStatus = task.Result;
                if (_dependencyStatus == DependencyStatus.Available)
                {
                    App = FirebaseApp.DefaultInstance;
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
                }
            });

        }

        #endregion

        #region Private Methods

        private void InitializeFirebase()
        {
            try
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                _isFirebaseInitialized = true;
                Debug.Log("Firebase - Enabling data collection.");

                InitializeCrashlytics();

            }
            catch (Exception e)
            {
                Debug.LogError("Failed to initialize Firebase: " + e.Message);
            }
        }

        private void EnableMobileAds()
        {
            MobileAds.Initialize(AdMobInitCallback);
        }
        
        private void AdMobInitCallback(InitializationStatus initStatus)
        {
            Debug.Log("AdMob initialization status: " + initStatus);
            
            _isGoogleAdmobInitialized = true;
            
            Debug.Log("Google Admob initialized.");
        }
        
        private void InitializeCrashlytics()
        {
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
            Debug.Log("Crashlytics initialized.");
        }
        
        private IEnumerator WaitForFirebaseInitialization()
        {
            yield return new WaitUntil(() => _isFirebaseInitialized && _isGoogleAdmobInitialized);
            EventManager.AnalyticsEvents.AnalyticsInitialized?.Invoke();
        }

        #endregion
    }
}