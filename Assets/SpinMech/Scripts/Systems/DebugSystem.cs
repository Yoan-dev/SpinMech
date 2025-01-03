using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace SpinMech
{
	[UpdateAfter(typeof(PhaseSystem))]
	public partial struct DebugSystem : ISystem
	{
		private bool _showDebugUI;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameConfig>();
			state.RequireForUpdate<PhaseComponent>();

			_showDebugUI = true; // temp
		}

		//[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				if (Input.GetKeyDown(KeyCode.N)) // force next phase
				{
					SystemAPI.GetSingletonRW<PhaseComponent>().ValueRW.GoToNext = true;
				}
				else if (Input.GetKeyDown(KeyCode.B)) // damage boss
				{
					// TODO
				}
				else if (Input.GetKeyDown(KeyCode.P)) // damage player
				{
					// TODO
				}
				else if (Input.GetKeyDown(KeyCode.H)) // heal player
				{
					// TODO
				}
				else if (Input.GetKeyDown(KeyCode.U)) // hide/show debug UI
				{
					_showDebugUI = !_showDebugUI;
					DebugUI.Instance.gameObject.SetActive(_showDebugUI);
				}
			}

			if (_showDebugUI)
			{
				PhaseComponent phase = SystemAPI.GetSingleton<PhaseComponent>();
				BossCounterComponent bossCounter = SystemAPI.GetSingleton<BossCounterComponent>();
				ScoreComponent score = SystemAPI.GetSingleton<ScoreComponent>();

				Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
				HealthComponent playerHealth = SystemAPI.GetComponent<HealthComponent>(playerEntity);

				HealthComponent bossHealth;
				if (SystemAPI.HasSingleton<BossTag>())
				{
					Entity bossEntity = SystemAPI.GetSingletonEntity<BossTag>();
					bossHealth = SystemAPI.GetComponent<HealthComponent>(bossEntity);
				}
				else
				{
					bossHealth = new HealthComponent();
				}

				DebugUI.Instance.UpdateUI(in phase, in bossCounter, in score, in playerHealth, in bossHealth);
			}
		}
	}
}