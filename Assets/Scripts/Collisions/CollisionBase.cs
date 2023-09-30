using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionBase : MonoBehaviour
{
	#region Fields
	//[Header("Settings")]
	//[Header("References")]
	[Header("Debug")]
	[SerializeField, ReadOnly] private Collider collider;
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	private void Awake()
	{
		collider = GetComponent<Collider>();
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (collider.isTrigger == false)
			return;
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if (collider.isTrigger == false)
			return;
	}

	protected virtual void OnCollisionEnter(Collision coll)
	{
		if (collider.isTrigger)
			return;
	}

	protected virtual void OnCollisionExit(Collision coll)
	{
		if (collider.isTrigger)
			return;
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
