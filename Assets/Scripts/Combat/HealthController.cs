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
    [Header("References")]
    public HealthBar healthBar;
    [Header("Debug")]
    [SerializeField, ReadOnly] private int currentHealth;
    #endregion

    #region Properties
    public int CurrentHealth => currentHealth;
	#endregion

	#region Unity Messages
	private void OnEnable()
	{
        currentHealth = maxHealth;
        healthBar.ChangeMaxLife(maxHealth);
	}
	#endregion

	#region Public Methods
    public void Heal(int heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
            healthBar.ChangeLife(currentHealth);
        }

        // um fix pra caso a vida passar do máximo
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

	public void TakeDamage(int damage, Action onDeath = null)
    {
        currentHealth -= damage;

		if (currentHealth <= 0)
        {
			onDeath?.Invoke();
			healthBar.ChangeLife(0);
            return;
		}

		healthBar.ChangeLife(currentHealth);
    }
    #endregion

    #region Private Methods
    #endregion
}
