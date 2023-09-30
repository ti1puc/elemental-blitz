using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] private int maxHealth;
    //[Header("References")]
    [Header("Debug")]
    [SerializeField, ReadOnly] private int currentHealth;
    #endregion

    #region Properties
    public int CurrentHealth => currentHealth;
	#endregion

	#region Unity Messages
	private void Awake()
	{
        currentHealth = maxHealth;
	}
	#endregion

	#region Public Methods
    public void Heal(int heal)
    {
        if (currentHealth < maxHealth)
            currentHealth += heal;

        // um fix pra caso a vida passar do máximo
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

	public void TakeDamage(int damage, Action onDeath = null)
    {
        currentHealth -= damage;
        onDeath?.Invoke();
    }
    #endregion

    #region Private Methods
    #endregion
}
