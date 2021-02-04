using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	public SoundClass mainMusic; //temporary

	[Range(0f, 1f)] public float sfxVolume;
	[Range(0f, 1f)] public float musicVolume; 

	//these have to be manually drag and dropped
    AudioSource audioSourceSFX; //with mixer for sounds effects
	AudioSource audioSourceMusic; //same for music

    private void Awake()
    {
		Instance = this;

		audioSourceSFX   = gameObject.AddComponent<AudioSource>();
		audioSourceMusic = gameObject.AddComponent<AudioSource>();

		//making the sounds 2D (non-directional/flat)
		audioSourceSFX.spatialBlend   = 0;
		audioSourceMusic.spatialBlend = 0;

		audioSourceMusic.loop = true;

		VolumeChange(); //setting the starting volume
	}

	private void Start()
	{
		audioSourceMusic.clip = mainMusic.clip;
		PlayMusic();
	}

	//later the options should call this
	public void VolumeChange()
	{
		audioSourceSFX.volume   = sfxVolume;
		audioSourceMusic.volume = musicVolume;
	}

	//for effects
	public void PlayFXSound (SoundClass sound)
	{
		audioSourceSFX.PlayOneShot(sound.clip, sound.volume);
	}

	//for music/themes
	//we use seperate functions so different sounds can go to seperate mixers
    public void PlayMusic ()
	{
		audioSourceMusic.Play();
	}
}
