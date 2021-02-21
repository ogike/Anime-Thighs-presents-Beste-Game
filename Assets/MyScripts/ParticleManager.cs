using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; } //basically make this a singleton

	private void Awake()
	{
        Instance = this;
	}

	//PLACEHOLDER
		//should be replaced with pooling later
	//should be called with StartCoroutine(), because its an ienumerator)
	public void PlayParticleEffect (GameObject particle, Vector3 position)
	{
		StartCoroutine( PlayParticleEffectCoroutine(particle, position) );
	}

	public IEnumerator PlayParticleEffectCoroutine (GameObject particle, Vector3 position)
	{
		GameObject particleObject = GameObject.Instantiate(particle, position, Quaternion.identity);
		ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();

		yield return new WaitForSeconds(particleSystem.main.duration);

		Destroy(particleObject, 0.0f);
	}
}
