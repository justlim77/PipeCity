using UnityEngine;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif
using System.Collections;

public class AudioSound3 : MonoBehaviour {

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
		if(SceneManager.GetActiveScene().buildIndex >= 3)
			audioSource.volume += 0.02f;
	}
}
