using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; } //basically make this a singleton

	public float particlesZLevel = 1.0f;

	private void Awake()
	{
        Instance = this;
	}

	//PLACEHOLDER
		//should be replaced with pooling later
	//should be called with StartCoroutine(), because its an ienumerator)
	public void PlayParticleEffect (GameObject particle, Vector3 position)
	{
		position.z = particlesZLevel;
		StartCoroutine( PlayParticleEffectCoroutine(particle, position) );
	}

	private IEnumerator PlayParticleEffectCoroutine (GameObject particle, Vector3 position)
	{
		GameObject particleObject = GameObject.Instantiate(particle, position, Quaternion.identity);
		ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

		//duration for particle should be set to the max particle lifetime in editor!!
		yield return new WaitForSeconds(particleSystem.main.duration);

		Destroy(particleObject, 0.0f);
	}
}
