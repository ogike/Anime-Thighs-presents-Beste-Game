using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

    AudioSource myAudioSource;

    private void Awake()
    {
		Instance = this;
        myAudioSource = GetComponent<AudioSource>();
    }

	private void Start()
	{
		PlayMusic();
	}

	//for effects
	public void PlayFXSound (SoundClass sound)
	{
        myAudioSource.PlayOneShot(sound.clip, sound.volume);
	}

	//for music/themes
	//we use seperate functions so different sounds can go to seperate mixers
    public void PlayMusic ()
	{
		//play song on seperate aduiosource with seperate mixer channel
	}
}
