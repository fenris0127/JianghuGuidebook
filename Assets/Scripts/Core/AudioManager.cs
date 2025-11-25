using UnityEngine;
using System.Collections.Generic;

namespace JianghuGuidebook.Core
{
    /// <summary>
    /// 게임의 모든 오디오를 관리하는 싱글톤 매니저
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource ambienceSource;

        [Header("Music")]
        [SerializeField] private AudioClip combatMusic;
        [SerializeField] private AudioClip victoryMusic;
        [SerializeField] private AudioClip menuMusic;

        [Header("Sound Effects - Combat")]
        [SerializeField] private AudioClip cardPlaySound;
        [SerializeField] private AudioClip cardDrawSound;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip blockSound;
        [SerializeField] private AudioClip damageSound;
        [SerializeField] private AudioClip healSound;

        [Header("Sound Effects - UI")]
        [SerializeField] private AudioClip buttonClickSound;
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip errorSound;

        [Header("Sound Effects - Events")]
        [SerializeField] private AudioClip victorySound;
        [SerializeField] private AudioClip defeatSound;
        [SerializeField] private AudioClip enemyDeathSound;
        [SerializeField] private AudioClip turnStartSound;

        [Header("Settings")]
        [SerializeField][Range(0f, 1f)] private float masterVolume = 1f;
        [SerializeField][Range(0f, 1f)] private float musicVolume = 0.7f;
        [SerializeField][Range(0f, 1f)] private float sfxVolume = 1f;
        [SerializeField] private bool enableAudio = true;

        private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            // 싱글톤 패턴
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
                BuildSoundLibrary();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// AudioSource 초기화
        /// </summary>
        private void InitializeAudioSources()
        {
            // Music Source
            if (musicSource == null)
            {
                GameObject musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                musicSource = musicObj.AddComponent<AudioSource>();
            }

            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = musicVolume * masterVolume;

            // SFX Source
            if (sfxSource == null)
            {
                GameObject sfxObj = new GameObject("SFXSource");
                sfxObj.transform.SetParent(transform);
                sfxSource = sfxObj.AddComponent<AudioSource>();
            }

            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.volume = sfxVolume * masterVolume;

            // Ambience Source
            if (ambienceSource == null)
            {
                GameObject ambienceObj = new GameObject("AmbienceSource");
                ambienceObj.transform.SetParent(transform);
                ambienceSource = ambienceObj.AddComponent<AudioSource>();
            }

            ambienceSource.loop = true;
            ambienceSource.playOnAwake = false;
            ambienceSource.volume = 0.5f * masterVolume;
        }

        /// <summary>
        /// 사운드 라이브러리 구축
        /// </summary>
        private void BuildSoundLibrary()
        {
            // Combat sounds
            if (cardPlaySound != null) soundLibrary["card_play"] = cardPlaySound;
            if (cardDrawSound != null) soundLibrary["card_draw"] = cardDrawSound;
            if (attackSound != null) soundLibrary["attack"] = attackSound;
            if (blockSound != null) soundLibrary["block"] = blockSound;
            if (damageSound != null) soundLibrary["damage"] = damageSound;
            if (healSound != null) soundLibrary["heal"] = healSound;

            // UI sounds
            if (buttonClickSound != null) soundLibrary["button_click"] = buttonClickSound;
            if (hoverSound != null) soundLibrary["hover"] = hoverSound;
            if (errorSound != null) soundLibrary["error"] = errorSound;

            // Event sounds
            if (victorySound != null) soundLibrary["victory"] = victorySound;
            if (defeatSound != null) soundLibrary["defeat"] = defeatSound;
            if (enemyDeathSound != null) soundLibrary["enemy_death"] = enemyDeathSound;
            if (turnStartSound != null) soundLibrary["turn_start"] = turnStartSound;
        }

        #region Music Control

        /// <summary>
        /// 배경 음악 재생
        /// </summary>
        public void PlayMusic(string musicType)
        {
            if (!enableAudio || musicSource == null) return;

            AudioClip clip = null;

            switch (musicType.ToLower())
            {
                case "combat":
                    clip = combatMusic;
                    break;
                case "victory":
                    clip = victoryMusic;
                    break;
                case "menu":
                    clip = menuMusic;
                    break;
            }

            if (clip != null && musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.Play();
            }
        }

        /// <summary>
        /// 배경 음악 정지
        /// </summary>
        public void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }

        /// <summary>
        /// 배경 음악 일시정지/재개
        /// </summary>
        public void ToggleMusicPause()
        {
            if (musicSource == null) return;

            if (musicSource.isPlaying)
            {
                musicSource.Pause();
            }
            else
            {
                musicSource.UnPause();
            }
        }

        #endregion

        #region Sound Effects

        /// <summary>
        /// 효과음 재생
        /// </summary>
        public void PlaySFX(string soundName, float volumeScale = 1f)
        {
            if (!enableAudio || sfxSource == null) return;

            if (soundLibrary.TryGetValue(soundName, out AudioClip clip))
            {
                sfxSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
            }
            else
            {
                Debug.LogWarning($"AudioManager: Sound '{soundName}' not found in library");
            }
        }

        /// <summary>
        /// AudioClip으로 직접 효과음 재생
        /// </summary>
        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (!enableAudio || sfxSource == null || clip == null) return;

            sfxSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }

        /// <summary>
        /// 특정 위치에서 3D 사운드 재생
        /// </summary>
        public void PlaySFXAtPosition(string soundName, Vector3 position, float volumeScale = 1f)
        {
            if (!enableAudio) return;

            if (soundLibrary.TryGetValue(soundName, out AudioClip clip))
            {
                AudioSource.PlayClipAtPoint(clip, position, volumeScale * sfxVolume * masterVolume);
            }
        }

        #endregion

        #region Convenience Methods

        // Combat sounds
        public void PlayCardPlay() => PlaySFX("card_play");
        public void PlayCardDraw() => PlaySFX("card_draw");
        public void PlayAttack() => PlaySFX("attack");
        public void PlayBlock() => PlaySFX("block");
        public void PlayDamage() => PlaySFX("damage");
        public void PlayHeal() => PlaySFX("heal");

        // UI sounds
        public void PlayButtonClick() => PlaySFX("button_click");
        public void PlayHover() => PlaySFX("hover");
        public void PlayError() => PlaySFX("error");

        // Event sounds
        public void PlayVictory()
        {
            PlaySFX("victory");
            PlayMusic("victory");
        }

        public void PlayDefeat() => PlaySFX("defeat");
        public void PlayEnemyDeath() => PlaySFX("enemy_death");
        public void PlayTurnStart() => PlaySFX("turn_start", 0.5f);

        #endregion

        #region Volume Control

        /// <summary>
        /// 마스터 볼륨 설정
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateAllVolumes();
        }

        /// <summary>
        /// 음악 볼륨 설정
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            if (musicSource != null)
            {
                musicSource.volume = musicVolume * masterVolume;
            }
        }

        /// <summary>
        /// 효과음 볼륨 설정
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume * masterVolume;
            }
        }

        /// <summary>
        /// 모든 볼륨 업데이트
        /// </summary>
        private void UpdateAllVolumes()
        {
            if (musicSource != null)
            {
                musicSource.volume = musicVolume * masterVolume;
            }

            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume * masterVolume;
            }

            if (ambienceSource != null)
            {
                ambienceSource.volume = 0.5f * masterVolume;
            }
        }

        /// <summary>
        /// 오디오 활성화/비활성화
        /// </summary>
        public void SetAudioEnabled(bool enabled)
        {
            enableAudio = enabled;

            if (!enabled)
            {
                StopMusic();
                sfxSource?.Stop();
            }
        }

        #endregion

        #region Testing

        /// <summary>
        /// 모든 사운드 테스트
        /// </summary>
        [ContextMenu("Test All Sounds")]
        public void TestAllSounds()
        {
            StartCoroutine(TestSoundsCoroutine());
        }

        private System.Collections.IEnumerator TestSoundsCoroutine()
        {
            Debug.Log("Testing all sounds...");

            foreach (var sound in soundLibrary)
            {
                Debug.Log($"Playing: {sound.Key}");
                PlaySFX(sound.Key);
                yield return new WaitForSeconds(0.5f);
            }

            Debug.Log("Sound test complete!");
        }

        #endregion

        #region Placeholder Audio Generation

        /// <summary>
        /// 플레이스홀더 오디오 생성 (실제 오디오 없을 때)
        /// </summary>
        [ContextMenu("Generate Placeholder Audio")]
        private void GeneratePlaceholderAudio()
        {
            Debug.Log("플레이스홀더 오디오 생성:");
            Debug.Log("1. Resources 폴더에 Audio 폴더 생성");
            Debug.Log("2. 무료 사운드 사이트에서 다운로드:");
            Debug.Log("   - freesound.org");
            Debug.Log("   - OpenGameArt.org");
            Debug.Log("   - zapsplat.com");
            Debug.Log("3. 또는 Unity에서 AudioClip.Create()로 간단한 비프음 생성");
        }

        #endregion
    }
}
