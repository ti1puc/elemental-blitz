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
    [SerializeField, ShowIf("canChangeElement")] private GameObject lightningRing;
    [SerializeField, ShowIf("canChangeElement")] private GameObject waterRing;
    [SerializeField, ShowIf("canChangeElement")] private GameObject fireRing;
	[Header("Element")]
    [SerializeField] public List<Element> unlockedElements = new List<Element>();
    [SerializeField, ReadOnly] private Element currentElement;
	[SerializeField, ReadOnly] private int currentElementIndex = 0;

    public Element CurrentElement => currentElement;

    #region Unity Messages
    private void Start()
    {
        if (canChangeElement)
        {
			lightningRing.SetActive(true);
			waterRing.SetActive(false);
			fireRing.SetActive(false);
		}

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

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Fire2"))
        {
            bool hasSkipped = false;
            currentElementIndex++;

            if (currentElementIndex <= unlockedElements.Count - 1)
			    currentElement = unlockedElements[currentElementIndex];
            else
			{
				currentElement = unlockedElements[0];
				currentElementIndex = 0;
                hasSkipped = true;
			}

            lightningRing.SetActive(false);
			waterRing.SetActive(false);
			fireRing.SetActive(false);
			switch (currentElement)
            {
                case Element.Lightning:
					lightningRing.SetActive(true);
					break;
                case Element.Water:
					waterRing.SetActive(true);
					break;
                case Element.Fire:
					fireRing.SetActive(true);
					break;
            }

            if (unlockedElements.Count > 1)
            {
                UIManager.RotateElement();
                if (unlockedElements.Count == 2 && hasSkipped)
				    StartCoroutine(RotateUIWithDelay());
            }
		}
	}

    private IEnumerator RotateUIWithDelay()
    {
        yield return new WaitForSeconds(.25f);
        UIManager.RotateElement();
	}
    #endregion
}
