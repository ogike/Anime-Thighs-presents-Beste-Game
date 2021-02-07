using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This handles what kinds of behaviours the enemy should do at what times
 * This is a very placeholder version, states should later be made with proper transitions, etc etc
 *      Also with custom state classes and lists
 * The different enemy behaviour scripts should be put in the proper 
 * Also needs an EnemyHandler on this GameObject for the distance
 */

//these are basically just kijelzésre van, meg hogy látjuk mikor milyenbe van
public enum StateNames {Sleeping, CloseRange, MediumRange, LongRange, VeryLongRange}

[System.Serializable]
public class StateDistanceBased
{
    public StateNames name;

    public float maxDist; //the distance until this state is active (ha kisebb távolságú state nem aktív obv)

    public List<MonoBehaviour> behaviours; //the list of behaviours to activate when in this state
}

public class EnemyStateMachineDistanceBased : MonoBehaviour
{
    public StateNames curStateName;
    [HideInInspector] int curStateIndex; //the index of the current state in the states list

    //the different states
    public List<StateDistanceBased> states;

    public List<MonoBehaviour> behavioursWhileAwake; //the behaviours that are always enabled while awake, regardless of state (pl.: rotation)

    public float timeToWakeUp = 0.3f; //the time it takes for the enemy to wake up

    bool isAwake;
    float timePassed; //for counting how much time til waking

    EnemyHandler myHandler;
    float distToPlayer; //set by the enemy handler


    // Start is called before the first frame update
    void Start()
    {
        if (states.Count == 0)
            Debug.Log("No states configured!");

        myHandler = GetComponent<EnemyHandler>();

        isAwake = false;
        timePassed = 0;
        curStateName = StateNames.Sleeping;
        curStateIndex = -1; //ezzel jelezzuk hogy még nincs beállítva a state/alszunk

		//disabling every state behaviour so we can enable stuff when we need to
		for (int i = 0; i < states.Count; i++)
		{
			for (int j = 0; j < states[i].behaviours.Count; j++)
			{
                states[i].behaviours[j].enabled = false;
            }
		}

        //disabling these until we are awake too
		for (int i = 0; i < behavioursWhileAwake.Count; i++)
		{
            behavioursWhileAwake[i].enabled = false;
		}
    }

    // Update is called once per frame
    void Update()
    {        
        //this is so when we get activated, theres a bit of time before the enemies start attacking
        if(!isAwake)
		{
            timePassed += Time.deltaTime;

            if (timePassed >= timeToWakeUp)
                WakeUp();

            if (!isAwake)
                return; //if we still arent awake, do nothing
		}

        distToPlayer = myHandler.GetDistToTarget();

        DecideState();
    }

    //deciding which state we are in (based on distance)
    void DecideState ()
	{
        //kereses tetel go brrrrrr
        int newIndex = -1;
		for (int i = 0; i < states.Count && newIndex == -1; i++)
		{
            if (distToPlayer <= states[i].maxDist)
                newIndex = i;
		}
        //if we didnt fin it, the distToPlayer is too high, so we should set the current state to be the most distant
        if (newIndex == -1)
		{
            newIndex = states.Count - 1;
		}

        if (newIndex != curStateIndex)
            SwitchState(newIndex);
    }

    void SwitchState (int newStateIndex)
	{
        //disabling the old states behaviours, only if we werent awake before
        if (curStateName != StateNames.Sleeping)
        {
            for (int i = 0; i < states[curStateIndex].behaviours.Count; i++)
            {
                states[curStateIndex].behaviours[i].enabled = false;
            }
        }

        //enabling the new state behaviours
        for (int i = 0; i < states[newStateIndex].behaviours.Count; i++)
        {
            states[newStateIndex].behaviours[i].enabled = true;
        }

        curStateIndex = newStateIndex;
        curStateName = states[curStateIndex].name;
    }

    void WakeUp()
    {
        isAwake = true;
		for (int i = 0; i < behavioursWhileAwake.Count; i++)
		{
            behavioursWhileAwake[i].enabled = true;
		}
    }
}
