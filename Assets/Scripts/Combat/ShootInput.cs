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
	[SerializeField] private bool canUseMouse;
	//[Header("References")]
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	protected override void Update()
	{
		if (DisableShoot) return;
		base.Update();

        if (canHoldKey && (Input.GetKey(shootKey) || (canUseMouse && Input.GetButton("Fire1"))))
		{
            Shoot();
        }
		else if (Input.GetKeyDown(shootKey) || (canUseMouse && Input.GetButtonDown("Fire1")))
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
