using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManeger : MonoBehaviour
{
    AudioSource MyAS;

	private void Start()
	{
		MyAS = gameObject.GetComponent<AudioSource>();
	}
	public void PlaySE(AudioClip clip)
	{
		MyAS.PlayOneShot(clip);	
	}
}
