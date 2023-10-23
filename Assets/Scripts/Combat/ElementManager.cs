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
    [SerializeField, ReadOnly] private Element selectedElement;
	[SerializeField, ReadOnly] private int selectedElementIndex = 0;

    public Element SelectedElement => selectedElement;

    #region Unity Messages
    public void Start()
    {
        selectedElement = unlockedElements[0];
		selectedElementIndex = 0;
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
            selectedElementIndex++;

            if (selectedElementIndex <= unlockedElements.Count - 1)
			    selectedElement = unlockedElements[selectedElementIndex];
            else
			{
				selectedElement = unlockedElements[0];
				selectedElementIndex = 0;
			}
		}
    }

    #endregion
}
