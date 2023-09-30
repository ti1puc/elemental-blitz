using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : CollisionBase
{
	#region Fields
	//[Header("Settings")]
	//[Header("References")]
	//[Header("Debug")]
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);

		// tomar dano e tal
	}

	protected override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
