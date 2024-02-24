using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [SerializeField] private AudioClip levelCompleteSoundEffect;
        [SerializeField] private AudioClip buttonClickSoundEffect;
        [SerializeField] private AudioSource soundEffects;
        [SerializeField] private AudioSource turningNoiseAudio;
        [SerializeField] private AudioSource backgroundMusic;
        [SerializeField] private GameObject[] canvases;
        private bool _isMute;
        private void Awake()
        {
            if (Instance != null) Destroy(this);
            Instance = this;
        }

        private void Start()
        {
            var buttons = canvases.SelectMany(canvas => canvas.GetComponentsInChildren<Button>()).ToList();
            buttons.ForEach(button =>
            {
                button.onClick.AddListener(PlayButtonClickSound);
            });
        }

        public void PlayCompleteLevelSound()
        {
            soundEffects.PlayOneShot(levelCompleteSoundEffect);
        }

        public void PlayButtonClickSound()
        {
            soundEffects.PlayOneShot(buttonClickSoundEffect);
        }

        public void StopMusic() => backgroundMusic.Stop();
        public void StartMusic() => backgroundMusic.Play();
        public void ToggleSound()
        {
            _isMute = !_isMute;
            // soundEffects.mute = _isMute; Can be commented back in, if sound effects sound should also be muted.
            backgroundMusic.mute = _isMute;
        }

        public void PlayTurningNoise(bool isStill)
        {
            turningNoiseAudio.mute = isStill;
        }
    }
}
