using System;
using System.Collections.Generic;
using _Game.Scripts.Audio;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.ScriptableObjects.Predefined;
using _Game.Scripts.ScriptableObjects.Saveable;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Managers.Core
{
    public class AudioManagerBGM : SerializedMonoBehaviour
    {
        #region Public Variables

        public AudioSource audioSource;

        [ShowInInspector] public Dictionary<SoundType, AudioClipData> audioClipsData;

        public SettingsDataSO settingsData;
        
        private float initialVolume;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void Start()
        {
            audioSource.Play();
        }

        #endregion

        #region Private Methods

        private void SubscribeToEvents()
        {
            EventManager.AudioEvents.AudioEnabled += UpdateSettingsData;
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.AudioEvents.AudioEnabled -= UpdateSettingsData;
        }

        private void InitializeAudioSettings()
        {
            UpdateSettingsData(settingsData.IsSoundEnabled);
        }

        private void UpdateSettingsData(bool isEnabled)
        {
            audioSource.mute = !isEnabled;
        }

        private void StopAudio()
        {
            audioSource.Stop();
        }

        private void PlayAudio(SoundType audioClipId, bool isRandomPitch)
        {
            if (!audioClipsData.ContainsKey(audioClipId))
            {
                Debug.LogError($"Audio clip ID {audioClipId} not found.");
                return;
            }

            var clipData = audioClipsData[audioClipId];
            
            SetAudioPitch(clipData, isRandomPitch);
            SetAudioClipToSource(clipData.AudioClip);
            SetAudioLooping(clipData.ShouldLoop);
               

            audioSource.Play();
        }

        private void SetAudioVolume(float volume = 1f)
        {
            audioSource.volume = initialVolume;
        }
        
        private void SetAudioLooping(bool shouldLoop)
        {
            audioSource.loop = shouldLoop;
        }

        private void SetAudioPitch(AudioClipData clipData, bool isRandomPitch)
        {
            if (isRandomPitch) 
            {
                audioSource.pitch = Random.Range(clipData.Pitch - 0.2f, clipData.Pitch + 0.2f);
            }
            else
            {
                audioSource.pitch = clipData.Pitch;
            }
        }
        
        private AudioClip GetAudioClip(SoundType audioClipId)
        {
            return audioClipsData[audioClipId].AudioClip;
        }

        private void SetAudioClipToSource(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
        }

        private void FadeToNewClip(SoundType newAudioClipId, float fadeDuration)
        {
            StartCoroutine(CrossFadeAudioService.CrossFadeCoroutine(GetAudioClip(newAudioClipId), fadeDuration, audioSource));
        }

        private void ToggleLooping(bool shouldLoop)
        {
            audioSource.loop = shouldLoop;
        }

        #endregion
    }
}
