using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ShootTimed : ShootBase
{
    #region Fields
    //[Header("Settings")]
    //[Header("References")]
    //[Header("Debug")]
    [Header("Element")]
    [SerializeField] public Element currentElement_;
    #endregion

    #region Properties
    #endregion

    #region Unity Messages
    protected override void Update()
	{
		base.Update();
        int elementIndex = (int)currentElement_;
        Shoot(elementIndex);
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
