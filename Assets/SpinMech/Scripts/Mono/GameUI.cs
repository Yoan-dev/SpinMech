using TMPro;
using UnityEngine;

namespace SpinMech
{
	public class GameUI : MonoBehaviour
	{
		public static GameUI Instance;

		[SerializeField] private TMP_Text _gameText;
		[SerializeField] private TMP_Text _playerHealthText;
		[SerializeField] private TMP_Text _bossHealthText;
		[SerializeField] private ScavengeUI _scavengeUI;

		private void Awake()
		{
			Instance = this;
		}

		public void UpdateUI(in BossCounterComponent bossCounter, in ScoreComponent score, in HealthComponent playerHealth, in HealthComponent bossHealth)
		{
			_gameText.text =
				"Boss Count: " + bossCounter.Value.ToString() +
				"\nScore: " + score.Value.ToString();

			_playerHealthText.text = "Player HP: " + playerHealth.Value.ToString() + "/" + playerHealth.Max.ToString();
			_bossHealthText.text = bossHealth.Max == 0f ? "" : "Boss HP: " + bossHealth.Value.ToString() + "/" + bossHealth.Max.ToString();
		}

		public void OpenScavengeUI()
		{
			_scavengeUI.OpenScavengeUI();
		}
	}
}