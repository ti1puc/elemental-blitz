using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBase : MonoBehaviour
{
    #region Fields
    //[Header("Settings")]
    //[Header("References")]
    //[Header("Debug")]
    [Header("Power Up Heal")]
    public int heal_ = 5;
    #endregion

    #region Properties
    #endregion

    #region Unity Messages

    #endregion

    #region Public Methods
    public void DestroyPowerup()
    {
        ObjectPoolManager.DespawnGameObject(gameObject);
    }
    #endregion
    
    #region Private Methods
    #endregion
}
