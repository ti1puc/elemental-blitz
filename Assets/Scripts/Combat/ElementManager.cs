using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum Element
{
    Lightning,
    Water,
    Fire
}

public class ElementManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool canChangeElement;
    [Header("Element")]
    [SerializeField] public List<Element> unlockedElements = new List<Element>();
    [SerializeField, ReadOnly] private Element currentElement;
	[SerializeField, ReadOnly] private int currentElementIndex = 0;

    public Element CurrentElement => currentElement;

    #region Unity Messages
    public void Start()
    {
        currentElement = unlockedElements[0];
		currentElementIndex = 0;
	}

    private void Update()
    {
        ChangeElement();

    }
    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void ChangeElement()
    {
        if (canChangeElement == false) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentElementIndex++;

            if (currentElementIndex <= unlockedElements.Count - 1)
			    currentElement = unlockedElements[currentElementIndex];
            else
			{
				currentElement = unlockedElements[0];
				currentElementIndex = 0;
			}
		}
    }

    #endregion
}
