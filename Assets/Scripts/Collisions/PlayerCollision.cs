using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] public int damage =  5;
    [Header("References")]
     private GameObject playerObj;
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

        if (CompareTag("enemyFire")) 
        {
           if(currentElement_ == Element.Lightning)
            {

             //   HealthController.TakeDamage(damage);

            }
            else
            {
                // nulo
            }
 
        }
        else if (CompareTag("enemyWater")) 
        {
            if (currentElement_ == Element.Fire)
            {
                //dano 
            }
            else
            {
                // nulo
            }
        }
        else // (CompareTag("enemyLightning")) 
        {
            if (currentElement_ == Element.Water)
            {
                //dano 
            }
            else
            {
                // nulo
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
