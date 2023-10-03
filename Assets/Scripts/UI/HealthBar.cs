using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Fields
    //[Header("Settings")]
    [Header("References")]
    public Slider slider_;
    //[Header("Debug")]
    #endregion

    #region Properties
    #endregion

    #region Unity Messages

    #endregion

    #region Public Methods
    public void ChangeMaxLife(int maxLife)
    {
        slider_.maxValue = maxLife;
        slider_.value = maxLife;
        
    }
    public void ChangeLife(float life)
    {
        slider_.value = life;
    }
    #endregion

    #region Private Methods
    #endregion
}
