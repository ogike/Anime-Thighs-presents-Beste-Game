using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is what we use to store sound data for different occasions
 * 
 */

[System.Serializable]
public class SoundClass
{
    public string name; //this is just what shows up in the inspector in lists, doesnt have any real function

    public AudioClip clip; //the audio clip to play

    [Range(0f, 1f)] public float volume = 1f;
}
