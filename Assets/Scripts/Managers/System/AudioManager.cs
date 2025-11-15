using UnityEngine;

// 게임의 모든 사운드(BGM, SFX) 재생을 담당하는 중앙 관리자입니다.
[RequireComponent(typeof(AudioSource), typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Audio Clips - Music")]
    public AudioClip mainMenuTheme;
    public AudioClip mapTheme;
    public AudioClip battleTheme;
    public AudioClip victoryTheme;
    
    [Header("Audio Clips - SFX")]
    public AudioClip cardPlaySound;
    public AudioClip attackSound;
    public AudioClip takeDamageSound;
    public AudioClip defendSound;
    public AudioClip buttonClickSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            AudioSource[] sources = GetComponents<AudioSource>();
            musicSource = sources[0];
            sfxSource = sources[1];

            musicSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}