using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace SpinMech
{
	public class ScavengeUI : MonoBehaviour
	{
		[SerializeField] private Button _validateButton;

		public void OnEnable()
		{
			_validateButton.onClick.AddListener(OnValidateClicked);
		}

		public void OnDisable()
		{
			_validateButton.onClick.RemoveAllListeners();
		}

		public void OpenScavengeUI()
		{
			gameObject.SetActive(true);
		}

		public void OnValidateClicked()
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.Unmanaged.EntityManager;
			Entity gameEntity = new EntityQueryBuilder(Allocator.Temp).WithAll<PhaseComponent>().Build(entityManager).GetSingletonEntity();
			PhaseComponent phase = entityManager.GetComponentData<PhaseComponent>(gameEntity);
			
			if (phase.Current == GamePhase.Scavenge)
			{
				phase.GoToNext = true;
				entityManager.SetComponentData(gameEntity, phase);
			}

			gameObject.SetActive(false);
		}
	}
}