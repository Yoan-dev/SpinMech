using TMPro;
using UnityEngine;

namespace SpinMech
{
	public class DebugUI : MonoBehaviour
	{
		public static DebugUI Instance;

		public TMP_Text PhaseText;
		public TMP_Text GameText;
		public TMP_Text PlayerHealthText;
		public TMP_Text BossHealthText;

		private void Awake()
		{
			Instance = this;
			//Instance.gameObject.SetActive(false);
		}

		public void UpdateUI(in PhaseComponent phase, in BossCounterComponent bossCounter, in ScoreComponent score, in HealthComponent playerHealth, in HealthComponent bossHealth)
		{
			PhaseText.text =
				"Phase: " + phase.Current +
				"\nTime: " + phase.Time.ToString("0.00") +
				"\nTimer: " + phase.Timer.ToString("0.00");

			GameText.text =
				"Boss Count: " + bossCounter.Value.ToString() +
				"\nScore: " + score.Value.ToString();
			
			PlayerHealthText.text = "Player HP: " + playerHealth.Value.ToString() + "/" + playerHealth.Max.ToString();
			BossHealthText.text = "Boss HP: " + bossHealth.Value.ToString() + "/" + bossHealth.Max.ToString();
		}
	}
}