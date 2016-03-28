#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioManager : Singleton<AudioManager> {

    // Set to protected to prevent calling constructor
    protected AudioManager() { }

    AudioMixer m_MainMix;
    AudioSource m_BGM;
    AudioSource m_SFX;
    AudioSource m_SFXContinuous;

    void Awake()
    {
        PlayerPrefs.SetInt("bgmPref", 1);
        PlayerPrefs.SetInt("sfxPref", 1);

        m_MainMix = Resources.Load("AudioMixers/MainMix", typeof(AudioMixer)) as AudioMixer;

        m_BGM = gameObject.AddComponent<AudioSource>();
        m_BGM.outputAudioMixerGroup = m_MainMix.FindMatchingGroups("BGM")[0];
        m_BGM.mute = System.Convert.ToUInt16(PlayerPrefs.GetInt("bgmPref")) == 0 ? true : false;
        m_BGM.playOnAwake = true;
        m_BGM.loop = true;

        m_SFX = gameObject.AddComponent<AudioSource>();
        m_SFX.outputAudioMixerGroup = m_MainMix.FindMatchingGroups("SFX")[0];
        m_SFX.mute = System.Convert.ToUInt16(PlayerPrefs.GetInt("sfxPref")) == 0 ? true : false;
        m_SFX.playOnAwake = false;
        m_SFX.loop = false;

        m_SFXContinuous = gameObject.AddComponent<AudioSource>();
        m_SFXContinuous.outputAudioMixerGroup = m_MainMix.FindMatchingGroups("SFX")[0];
        m_SFXContinuous.mute = System.Convert.ToUInt16(PlayerPrefs.GetInt("sfxPref")) == 0 ? true : false;
        m_SFXContinuous.playOnAwake = false;
        m_SFXContinuous.loop = false;
    }

    /// <summary>
    /// [BGM] Plays a looping audio clip.
    /// </summary>
    /// <param name="clip">[AudioClip] Clip to be played.</param>
    /// <param name="volume">[float] Volume of SFX</param>
    public void PlayBGM(AudioClip clip, float volume = 1.0f)
    {
        m_BGM.mute = System.Convert.ToUInt16(PlayerPrefs.GetInt("bgmPref")) == 0 ? true : false;
        m_BGM.clip = clip;
        m_BGM.volume = volume;

        if (!m_BGM.isPlaying)
            m_BGM.Play();
    }

    /// <summary>
    /// [SFX] Plays a single audio clip.
    /// </summary>
    /// <param name="clip">[AudioClip] Clip to be played.</param>
    /// <param name="pitch">[float] Initial pitch of SFX (+- 0.5f variation)</param>
    /// <param name="volume">[float] Volume of SFX</param>
    public void PlaySFX(AudioClip clip = null, float pitch = 1.0f, float volume = 1.0f)
    {
        if (clip == null)
        {
            //Debug.Log("No AudioClip parameter, returning null...");
            return;
        }
 
        m_SFX.mute = System.Convert.ToUInt16(PlayerPrefs.GetInt("sfxPref")) == 0 ? true : false;
        m_SFX.pitch = UnityEngine.Random.Range(pitch - 0.05f, pitch + 0.05f);
        m_SFX.volume = volume;

        m_SFX.PlayOneShot(clip);       
    }

    /// <summary>
    /// [SFX] Plays continuous audio clip.
    /// </summary>
    /// <param name="clip">[AudioClip] Clip to be played.</param>
    /// <param name="pitch">[float] Initial pitch of SFX (+- 0.5f variation)</param>
    /// <param name="volume">[float] Volume of SFX</param>
    public void PlaySFXContinuous(AudioClip clip = null, float pitch = 1.0f, float volume = 1.0f)
    {
        if (clip == null)
            return;

        m_SFXContinuous.clip = clip;
        m_SFXContinuous.mute = System.Convert.ToUInt16(PlayerPrefs.GetInt("sfxPref")) == 0 ? true : false;
        m_SFXContinuous.pitch = UnityEngine.Random.Range(pitch - 0.05f, pitch + 0.05f);
        m_SFXContinuous.volume = volume;
        m_SFXContinuous.Play();
    }

    /// <summary>
    /// [SFX] Returns true if continuous SFX is playing
    /// </summary>
    public bool isContinuousPlaying
    {
        get { return m_SFXContinuous.isPlaying; }
    }

    /// <summary>
    /// [SFX] Stops continuous audio clip.
    /// </summary>
    public void StopSFXContinuous()
    {
        if(m_SFXContinuous.isPlaying)
            m_SFXContinuous.Stop();
    }

    /// <summary>
    /// [SFX] Plays a random audio clip.
    /// </summary>
    /// <param name="clips">[AudioClip[]] Array of sound effects</param>
    public void RandomizeSFX(params AudioClip[] clips)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        float randomPitch = UnityEngine.Random.Range(0.95f, 1.05f);

        m_SFX.pitch = randomPitch;
        m_SFX.clip = clips[randomIndex];
        m_SFX.Play();
    }

    public void ToggleBGM()
    {
        m_BGM.mute = !m_BGM.mute;
    }

    public void ToggleSFX()
    {
        m_SFX.mute = !m_SFX.mute;
    }

    public float BGMVolume
    {
        get { return m_BGM.volume; }
        set { m_BGM.volume = value; }
    }
}
