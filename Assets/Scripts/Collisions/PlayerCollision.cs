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

    private void Start()
	{
		//find Player and get element
		elementManager = PlayerManager.Player.GetComponent<ElementManager>();
	}

    private void OnTriggerEnter(Collider other)
	{
        //if collide with water element
        #region colision with water
        if (other.CompareTag("enemyWater")) 
        {
            // if current element = lighning
           if(currentElement_ == Element.Lightning)
            {
				BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

				//healthController.TakeDamage(damage);  // damage = 0;
            }

            if (currentElement_ == Element.Fire)
            {
                BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

                healthController.TakeDamage(damage * 2);// Damage * 2
            }

            if (currentElement_ == Element.Water)
            {
                BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

                healthController.TakeDamage(damage/2); // Damage / 2
            }
        }
        #endregion

        #region colision with lighning
        if (other.CompareTag("enemyLightning"))
        {
            // if current element = lighning
            if (currentElement_ == Element.Lightning)
            {
                BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

                healthController.TakeDamage(damage / 2);  
            }

            if (currentElement_ == Element.Fire)
            {
                BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

                healthController.TakeDamage(0);
            }

            if (currentElement_ == Element.Water)
            {
                BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

                healthController.TakeDamage(damage * 2); 
            }
        }
        #endregion

        #region colision with fire
        if (other.CompareTag("enemyFire"))
        {
            // if current element = lighning
            if (currentElement_ == Element.Lightning)
            {
                BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

                healthController.TakeDamage(damage * 2);
            }

            if (currentElement_ == Element.Fire)
            {
                BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

                healthController.TakeDamage(damage / 2);
            }

            if (currentElement_ == Element.Water)
            {
                BulletBase bulletBase = other.gameObject.GetComponent<BulletBase>();
                bulletBase.DestroyBullet();

                healthController.TakeDamage(0);
            }
        }
        #endregion

        #region colision with power ups
        if (other.CompareTag("pUpHeal"))
        {
            PowerupBase pupb_ = other.gameObject.GetComponent<PowerupBase>();
            healthController.Heal(pupb_.heal_);
            pupb_.DestroyPowerup();
        }
        #endregion  
    }

    #region Public Methods
    #endregion

    #region Private Methods
    private void Update()
    {
        // buscar scripts no update é pesado pq roda em todo frame, melhor colocar no Awake ou Start e guardar em uma variavel
        //ElementManager elementManager = playerObj.GetComponent<ElementManager>();

        currentElement_ = elementManager.SelectedElement;
    }

    #endregion
}
