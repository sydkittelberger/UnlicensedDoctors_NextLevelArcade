using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioController : MonoBehaviour
{
    //Private Variables
    private float musicSpeed = 1f;
    private float musicPitch = 1f;
    private float originalMusicVolume;
    private Coroutine musicVolumeCoroutine;
    private float breathingLevel = 0;

    //Public Variables
    [Header("Music")]
    public AudioSource musicSource;
    public AudioMixer musicMixer;
    public float speedIncrease = 0.1f;
    public float pitchDecrease = 0.1f;

    [Header("Music Volume Decrease")]
    public float firstLossMultiplier = 0.75f;
    public float additionalLossMusicReduction = 0.2f;
    public float musicDecreaseDuration = 0.4f;

    [Header("Breathing")]
    public AudioSource breathingSource;
    public float startBreathingVolume = 0.25f;
    public float breathingVolumeIncrease = 0.2f;
    public float startingBreathingSpeed = 1f;
    public float breathingSpeedIncrease = 0.15f;
    public float maxBreathingSpeed = 2f;

    void Awake()
    {
        //Keep Track of Original Volume of Background Music
        if (musicSource != null)
        {
            originalMusicVolume = musicSource.volume;
        }
    }
    
    void Start()
    {
        //Starting Values are Set
        ResetBreathing();
    }

    void Update()
    {
        
    }

    public void IntensifyMusic()
    {
        //Increase Speed and Lower Pitch
        musicSpeed += speedIncrease;
        musicPitch = Mathf.Max(0.1f, musicPitch - pitchDecrease);

        musicSource.pitch = musicSpeed;

        //Compensate for Pitch Increase From Faster Playback
        musicMixer.SetFloat("MusicPitch", musicPitch/musicSpeed);
    }

    public void IntensifyBreathing()
    {
        if (breathingSource == null)
        return;

        //Breathing Only Activates After First Heart is Lost, Hence the Need to Track Player's Stage
        breathingLevel++;

        //First Heart Lost Activates the Sound; Following Lost Lives increase the Volume + Pitch
        breathingSource.volume = Mathf.Clamp01(startBreathingVolume + breathingVolumeIncrease * (breathingLevel - 1));
        breathingSource.pitch = Mathf.Clamp(startingBreathingSpeed + breathingSpeedIncrease * (breathingLevel - 1), 0.1f, maxBreathingSpeed);

        //Keep the Loop Playing Without Restarting It
        if (!breathingSource.isPlaying)
        {
            breathingSource.Play();
        }

        //Lower Music as Breathing Becomes More Intense
        DecreaseMusic();
    }

    private void DecreaseMusic()
    {
        if (musicSource == null)
        return;

        float volumeMultiplier = Mathf.Clamp01(firstLossMultiplier - additionalLossMusicReduction * (breathingLevel - 1));

        float targetVolume = originalMusicVolume * volumeMultiplier;

        //Replace Any Previous Music Fades
        if (musicVolumeCoroutine != null)
        {
            StopCoroutine(musicVolumeCoroutine);
            musicVolumeCoroutine = null;
        }

        //Account for Any Interrupted Music Fades; Set Volume
        if (musicDecreaseDuration <= 0f)
        {
            musicSource.volume = targetVolume;
            return;
        }

        //Start Volume Fade
        musicVolumeCoroutine = StartCoroutine(FadeMusicVolume(targetVolume, musicDecreaseDuration));
    }

    public void ResetMusic()
    {
        //Reset Values
        musicSpeed = 1f;
        musicPitch = 1f;
        musicSource.pitch = musicSpeed;
        musicMixer.SetFloat("MusicPitch", musicPitch/musicSpeed);

        //Reset Breathing
        ResetBreathing();
    }

    public void ResetBreathing()
    {
        breathingLevel = 0;

        //Restore Original Volume
        if (musicVolumeCoroutine != null)
        {
            StopCoroutine(musicVolumeCoroutine);
            musicVolumeCoroutine = null;
        }
        if (musicSource != null)
        {
            musicSource.volume = originalMusicVolume;
        }
        
        if (breathingSource == null)
        return;

        breathingSource.Stop();
        breathingSource.loop = true;
        breathingSource.volume = startBreathingVolume;
        breathingSource.pitch = startingBreathingSpeed;
    }

    private IEnumerator FadeMusicVolume(float targetVolume, float duration)
    {
        float startingVolume = musicSource.volume;

        float elapsed = 0f;
        while(elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            //Slowly Fade Out Music
            musicSource.volume = Mathf.Lerp(startingVolume, targetVolume, Mathf.Clamp01(elapsed/duration));

            yield return null;
        }

        //Finalize Volume
        musicSource.volume = targetVolume;
        musicVolumeCoroutine = null;
    }

    public IEnumerator FadeOutBreathing(float duration)
    {
        if (breathingSource == null)
        yield break;

        float startingVolume = breathingSource.volume;
        
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed/duration);

            breathingSource.volume = Mathf.Lerp(startingVolume, 0f, progress);

            yield return null;
        }

        breathingSource.Stop();
        breathingSource.volume = 0f;
    }
}
