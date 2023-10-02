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
    [Header("Element")]
    [SerializeField] private Element currentElement_;
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	protected override void Update()
	{
		base.Update();

        //find Player and get element
        ElementManager elementManager = gameObject.GetComponent<ElementManager>();
        currentElement_ = elementManager.selectedElement;

		int elementIndex = (int)currentElement_;

		

        if (canHoldKey && Input.GetKey(shootKey))
		{
            Shoot(elementIndex);
        }
		else if (Input.GetKeyDown(shootKey))
		{

            Shoot(elementIndex);
        }
			
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
