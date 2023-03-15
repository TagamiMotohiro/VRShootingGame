using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManeger : MonoBehaviour
{
	//制作担当　田上
	//オーディオ再生時に呼び出すクラス
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
