using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;


public enum Element
{
    Lightning,
    Water,
    Fire

}
public class ElementManager : MonoBehaviour
{
    #region Fields
    [Header("Element")]
    [SerializeField] public Element selectedElement;
    [SerializeField] public Element unlockedElement;
    #endregion

    #region Properties
    #endregion

    #region Unity Messages

    public void Start()
    {
        selectedElement = Element.Lightning;
        unlockedElement = Element.Fire;
    }



    private void Update()
    {
        chageElement();

    }
    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void chageElement()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            selectedElement++;
            if(selectedElement > unlockedElement)
            {
                selectedElement = Element.Lightning;
            }
        }
    }

    #endregion
}
