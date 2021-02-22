using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{

	public float updateInterval = 0.5f;
	public Text counterText;

	float accum = 0.0f; // FPS accumulated over the interval
	int frames = 0; // Frames drawn over the interval
	float timeleft; // Left time for current interval

	void Start()
	{
		timeleft = updateInterval;
	}

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0.0)
		{
			// display two fractional digits (f2 format)
			counterText.text = "" + accum / frames;
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}
	}
}