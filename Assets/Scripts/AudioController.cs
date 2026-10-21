using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    //Private Variables
    private float musicSpeed = 1f;
    private float musicPitch = 1f;

    //Public Variables
    public AudioSource musicSource;
    public AudioMixer musicMixer;
    public float speedIncrease = 0.1f;
    public float pitchDecrease = 0.1f;

    void Start()
    {
        
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

    public void ResetMusic()
    {
        //Reset Values
        musicSpeed = 1f;
        musicPitch = 1f;
        musicSource.pitch = musicSpeed;
        musicMixer.SetFloat("MusicPitch", musicPitch/musicSpeed);
    }
}
