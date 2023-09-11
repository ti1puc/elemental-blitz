using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy01Movement : MonoBehaviour
{
    #region Fields
    public float moveSpeed = 3, horizontalSpd = 4, spawnX = 22, spawnZ = 42, maxZ = -30;
    int toRight;


    #endregion

    #region Properties
    #endregion

    #region Unity Messages
    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks); //tirar depois que criar o gameManager

        //ao ser criado ele pode vir da direita, ou da esquerda
        Direction();

    }

    private void Update()
    {
        float zPos = moveSpeed * Time.deltaTime;

        float xPos = (horizontalSpd * Time.deltaTime) * toRight;
        transform.Translate(xPos, 0, -zPos);

        Debug.Log(xPos);

        // caso o inimigo não for destruido pelo player

        if(transform.position.z <= maxZ)
        {
            AutoDestroy();
        }
    }
    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void AutoDestroy()
    {
        // Destroy(gameObject); 
        Direction();
    }

    private void Direction()
    {
        Vector3 currentPos = transform.position;
        toRight = Random.Range(0, 2);


        if (toRight == 1)
        {
            //toRight = true;
            currentPos.x = -spawnX;

        }
        else
        {
           // toRight = false;
            currentPos.x = spawnX;
            toRight = -1;

        }

        currentPos.z = spawnZ;
        transform.position = currentPos;
    }

    #endregion
}

