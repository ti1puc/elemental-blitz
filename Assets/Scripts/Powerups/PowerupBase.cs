using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBase : PoolableObject
{
    #region Fields
    //[Header("Settings")]
    //[Header("References")]
    //[Header("Debug")]
    #endregion
    
    #region Properties
    #endregion
    
    #region Unity Messages
    #endregion
    
    #region Public Methods
    public void DestroyPowerup()
    {
        DestroyPoolable();
    }
    #endregion
    
    #region Private Methods
    #endregion
}
