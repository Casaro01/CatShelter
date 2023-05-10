using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : Item
{
	#region VARS

	public float objectiveTime;
	public float startTime;
	public float currentTime;
	public float reward;

	public enum ToyState { IDLE, INUSE, END };
	public ToyState state = ToyState.IDLE;
	ToyState prevState;
		
# endregion

	#region UPDATE
	private void Update()
	{
		switch (state)
		{
			case ToyState.IDLE:
				Update_IDLE();
				break;

			case ToyState.INUSE:
				Update_USED();
				break;

			case ToyState.END:
				Update_END();
				break;
		}
	}

	private void Update_IDLE()
	{
		// idle animation

		// wait for cat to be assigned

		// if cat is assigned go to used
	}
	private void Update_USED()
	{
		// used animation

		// count down timer

		// if timer runs out go to END
		// if interrupted go to IDLE
	}
	private void Update_END()
	{
		// wait to be collected, but cat is already sent home
	}

	#endregion

	#region CHANGESTATE

	private void ChangeState(ToyState newState)
	{
		if (state == newState) return;

		prevState = state;
		state = newState;

		switch (state)
		{
			case ToyState.IDLE:
				SetState_IDLE();
				break;

			case ToyState.INUSE:
				SetState_USED();
				break;

			case ToyState.END:
				SetState_END();
				break;
		}
	}
	private void SetState_IDLE()
	{

	}

	private void SetState_USED()
	{
		
	}

	private void SetState_END()
	{

	}
	#endregion
}
