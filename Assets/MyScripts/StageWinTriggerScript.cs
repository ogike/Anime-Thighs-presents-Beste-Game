using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWinTriggerScript : MonoBehaviour
{
    bool alreadyWon;

    // Start is called before the first frame update
    void Start()
    {
        alreadyWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player" && !alreadyWon)
		{
            GameManagerScript.Instance.WinStage();
            alreadyWon = true;
		}
	}
}
