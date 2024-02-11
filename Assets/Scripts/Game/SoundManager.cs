using UnityEngine;

namespace Game
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [SerializeField] private AudioClip levelCompleteSoundEffect;
        [SerializeField] private AudioSource levelCompleteAudioSource;
        [SerializeField] private AudioSource backgroundMusic;
        private bool _isMute;
        private void Awake()
        {
            if (Instance != null) Destroy(this);
            Instance = this;
        }

        public void PlayCompleteLevelSound()
        {
            levelCompleteAudioSource.PlayOneShot(levelCompleteSoundEffect);
        }

        public void StopMusic() => backgroundMusic.Stop();
        public void StartMusic() => backgroundMusic.Play();
        public void ToggleSound()
        {
            _isMute = !_isMute;
            // levelCompleteAudioSource.mute = _isMute; Can be commented back in, if levelComplete sound should also be muted.
            backgroundMusic.mute = _isMute;
        }
    }
}
