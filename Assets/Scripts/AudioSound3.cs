using UnityEngine;
using System.Collections;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class AudioSound3 : MonoBehaviour
{
	AudioSource audioSource;
	
	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}
	
	void Start()
	{
		audioSource.Play();
	}
	
	void Update()
	{
#if UNITY_5_3_OR_NEWER
        if (SceneManager.GetActiveScene().buildIndex >= 3)
#else
        if(Application.loadedLevel >= 3)
#endif
			audioSource.volume += 0.02f;
	}
}
