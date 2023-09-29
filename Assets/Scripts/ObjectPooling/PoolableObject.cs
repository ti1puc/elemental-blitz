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
	public Action<PoolableObject> KillAction;
	#endregion

	#region Abstract & Virtual Methods
	/// <summary>
	/// Method called when object is spawned using pool. Override this on your class
	/// </summary>
	public virtual void OnSpawn() { }
	/// <summary>
	/// Method called when object is destroyed using pool. Override this on your class
	/// </summary>
	public virtual void OnDestroyed() { }
	#endregion

	#region Public Methods
	public void DestroyPoolable() => KillAction?.Invoke(this);
	public void SetKillAction(Action<PoolableObject> killAction) => KillAction = killAction;
	#endregion

}
