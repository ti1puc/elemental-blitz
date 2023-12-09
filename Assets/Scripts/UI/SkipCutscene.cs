using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipCutscene : MonoBehaviour
{
	[SerializeField] private Button skipButton;
	[SerializeField] private Animator skipAnimator;
	[SerializeField] private float deactiveSkipAfter = 3f;
	[SerializeField, ReadOnly] private bool hasShowed;

	private void Awake()
	{
		skipButton.gameObject.SetActive(false);
		skipButton.onClick.AddListener(Skip);

		hasShowed = false;
	}

	private void OnDestroy()
	{
		skipButton.onClick.RemoveListener(Skip);
	}

	private void Update()
	{
		if (Input.anyKeyDown && !hasShowed)
		{
			skipButton.gameObject.SetActive(true);
			hasShowed = true;
			StartCoroutine(DeactivateSkip());
		}
	}

	public void Skip()
	{
		GameManager.NextLevel();
	}

	private IEnumerator DeactivateSkip()
	{
		yield return new WaitForSeconds(deactiveSkipAfter);
		skipAnimator.SetTrigger("Hide");

		yield return new WaitForSeconds(.4f);
		skipButton.gameObject.SetActive(false);
		hasShowed = false;
	}
}
