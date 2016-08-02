using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour
{
	public ParticleSystem RainParticles;
	public bool IsRaining
    {
        get;
        private set;
    }

	public AudioSource _AudioSource;
	public float RainInterval = 5f;

	float _RainCounter = 0f;
	float _RainDuration = 0f;

	
	void Start ()
	{
        RainParticles.loop = false;
        _AudioSource = GetComponent<AudioSource>();
	}

	void Update ()
	{
		RandomRain ();
	}

	void RandomRain ()
	{
		if (!RainParticles.isPlaying)
        {
			IsRaining = false;
			_RainCounter += 1 * Time.deltaTime;
		}

		if (_RainCounter > RainInterval)
        {
            // Increment rain duration
            _RainDuration += 1 * Time.deltaTime;

            // Play rain particles
            if (!RainParticles.isPlaying)
            {
                RainParticles.Play ();
			    IsRaining = true;
            }

            // Play rain sound
            if (!_AudioSource.isPlaying)
            {
				PlayRainSound();
            }
		}

		if (_RainDuration >= RainInterval)
        {
            if(RainParticles.isPlaying)
			    RainParticles.Stop();
            RainInterval = Random.Range(5f, 10f);
			_RainCounter = 0;
			_RainDuration = 0;
			StopPlayRainSound();
		}

        _AudioSource.mute = InputManager2._isPaused;
    }

	void PlayRainSound()
	{
		_AudioSource.Play();
	}

	void StopPlayRainSound()
	{
		_AudioSource.Stop();
	}    
}
