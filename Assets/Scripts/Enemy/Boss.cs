using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
	[Header("Name")]
	[SerializeField] private string bossName;

	public string BossName => bossName;
}
