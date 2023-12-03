using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
	[SerializeField] private GolemCutscene golemCutscene;

	public void PassSceneOnAnimation()
	{
		GameManager.NextLevel();
	}

	public void PlayerShipSound()
	{
		AudioManager.Instance.PlaySFXEnemy("snd_PlayerPass");
	}

	public void EnemyShipSound()
	{
		AudioManager.Instance.PlaySFXEnemy("snd_EnemyPass");
	}

	public void GetGemSound()
	{
		AudioManager.Instance.PlaySFXEnemy("snd_PowerUp03");
	}

	public void WakeUpGolem()
	{
		if (golemCutscene != null)
		{
			golemCutscene.WakeUpAnim();
		}
	}
}
