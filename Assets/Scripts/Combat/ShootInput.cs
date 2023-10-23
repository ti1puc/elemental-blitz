using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInput : ShootBase
{
	#region Fields
	[Header("Input Settings")]
	[SerializeField] private bool canHoldKey;
	[SerializeField] private KeyCode shootKey = KeyCode.Z;
    //[Header("References")]
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	protected override void Update()
	{
		base.Update();

        if (canHoldKey && Input.GetKey(shootKey))
		{
            Shoot();
        }
		else if (Input.GetKeyDown(shootKey))
		{
            Shoot();
        }
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
