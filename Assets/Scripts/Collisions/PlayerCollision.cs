using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerCollision : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] public int damage =  2;
    [Header("References")]
     private GameObject playerObj;
    public HealthController healthController;
    public BulletBase bulletBase;   
    //[Header("Debug")]
    [Header("Element")]
    [SerializeField] private Element currentElement_;
    #endregion

    #region Properties
    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion

    #region Unity Messages
    private void OnTriggerEnter(Collider other)
	{


        if (other.CompareTag("enemyWater")) 
        {
           if(currentElement_ == Element.Lightning)
            {
                bulletBase = other.gameObject.GetComponent<BulletBase>();
                healthController.TakeDamage(damage, bulletBase.DestroyBullet);

            }
 
        }
       
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Update()
    {
        //find Player and get element
        ElementManager elementManager = playerObj.GetComponent<ElementManager>();
        currentElement_ = elementManager.selectedElement;
    }

    #endregion
}
