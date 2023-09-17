using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds an action for kill behaviour (to be released from Pool) and common methods
/// </summary>
public abstract class PoolableObject : MonoBehaviour
{
	#region Fields
	public Action<PoolableObject> killAction;
	#endregion

	#region Abstract Methods
	#endregion

	#region Public Methods
	public void SetKillAction(Action<PoolableObject> killAction) => this.killAction = killAction;
	#endregion

}
