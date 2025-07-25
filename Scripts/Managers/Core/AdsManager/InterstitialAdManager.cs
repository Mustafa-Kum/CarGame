using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using FortuneWheel;
using UnityEngine;

public class InterstitialAdManager : MonoBehaviour
{
    #region PUBLIC VARIABLES

    [SerializeField] private IAPBundleValuesSO _bundleValuesSo;
    [SerializeField] private PlayerSavableData _playerSavableData;

    public static InterstitialAdManager Instance;
    public RewardedAdData RewardedAdData;

    #endregion

    #region PRIVATE VARIABLES

    private float _timeSinceLastLevelEnd = 0f;
    private bool _timerActive = false;
    private float _requiredTime = 30f;
    private bool _canInterstialShow;

    #endregion


    #region Unity Lifecycle

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make the instance persistent across scenes
            Debug.Log("DontDestroyOnLoad: InterstitialAdManager");
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }

        _timerActive = true;
        _timeSinceLastLevelEnd = 0f;
    }

    private void OnEnable()
    {
        EventManager.InGameEvents.LevelFinish += OnLevelEnd;
        EventManager.InGameEvents.LevelFail += OnLevelEnd;
    }

    private void OnDisable()
    {
        EventManager.InGameEvents.LevelFinish -= OnLevelEnd;
        EventManager.InGameEvents.LevelFail -= OnLevelEnd;
    }

    private void Update()
    {
        if (!_timerActive) return;
        _timeSinceLastLevelEnd += Time.deltaTime;
        
        if (_timeSinceLastLevelEnd >= _requiredTime)
        {
            _canInterstialShow = true;
        }
        else
        {
            _canInterstialShow = false;
        }
    }

    #endregion

    #region Event Responses

    private void OnLevelEnd()
    {
        if (!_timerActive) // Start timer after the first level end
        {
            _timeSinceLastLevelEnd = 0f;
            _timerActive = true;
        }
        else
        {
            CheckAndShowInterstitial();
        }
    }

    #endregion
    
    private void CheckAndShowInterstitial()
    {
        if (_bundleValuesSo.NoAdsPurchased)
        {
            return; 
        }

        if (_playerSavableData.LevelIndex == 0) return;

        if (!_canInterstialShow) return;

        if (_timeSinceLastLevelEnd >= _requiredTime)
        {
            EventManager.AdEvents.ShowRewardedInterstitial.Invoke(OnAdShow);
            Debug.Log("Showing Interstitial Ad");
            ResetAdTimer();
        }
    }
    
    private void OnAdShow()
    {
        EventManager.CurrencySystem.RewardedAd.Invoke(RewardedAdData);
    }

    private void ResetAdTimer()
    {
        _timeSinceLastLevelEnd = 0f;
    }
}