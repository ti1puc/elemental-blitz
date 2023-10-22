using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerCollision : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public int damage =  2;
    [Header("References")]
    public HealthController healthController;
    [Header("Element")]
    [SerializeField] private Element currentElement_;
    [Header("Debug")]
    [SerializeField, ReadOnly] private ElementManager elementManager;

    private void Awake()
	{
		//find Player and get element
		elementManager = PlayerManager.Player.GetComponent<ElementManager>();
	}

    private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("enemyWater")) 
        {
           if(currentElement_ == Element.Lightning)
            {
				BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

				healthController.TakeDamage(damage);
            }
 
        }

        if (other.CompareTag("pUpHeal"))
        {
            PowerupBase pupb_ = other.gameObject.GetComponent<PowerupBase>();
            healthController.Heal(pupb_.heal_);
            pupb_.DestroyPowerup();
        }


    }

    #region Public Methods
    #endregion

    #region Private Methods
    private void Update()
    {
        // buscar scripts no update é pesado pq roda em todo frame, melhor colocar no Awake ou Start e guardar em uma variavel
        //ElementManager elementManager = playerObj.GetComponent<ElementManager>();

        currentElement_ = elementManager.selectedElement;
    }

    #endregion
}
