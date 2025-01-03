using TMPro;
using UnityEngine;

namespace SpinMech
{
	public class DebugUI : MonoBehaviour
	{
		public static DebugUI Instance;

		[SerializeField] private TMP_Text _phaseText;

		private void Awake()
		{
			Instance = this;
			//Instance.gameObject.SetActive(false);
		}

		public void UpdateUI(in PhaseComponent phase)
		{
			_phaseText.text =
				"Phase: " + phase.Current +
				"\nTime: " + phase.Time.ToString("0.00") +
				"\nTimer: " + phase.Timer.ToString("0.00");
		}
	}
}