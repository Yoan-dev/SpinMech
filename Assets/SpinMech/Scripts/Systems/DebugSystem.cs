using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace SpinMech
{
	[UpdateAfter(typeof(PhaseSystem))]
	[UpdateBefore(typeof(HealthSystem))]
	public partial struct DebugSystem : ISystem
	{
		private bool _showDebugUI;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			//state.Enabled = false;

			state.RequireForUpdate<GameConfig>();
			state.RequireForUpdate<PhaseComponent>();
			state.RequireForUpdate<BossCounterComponent>();
			state.RequireForUpdate<ScoreComponent>();
			state.RequireForUpdate<DamageEvent>();

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
					AddDamageEvent<BossTag>(SystemAPI.GetSingletonEntity<BossTag>(), 50f);
				}
				else if (Input.GetKeyDown(KeyCode.P)) // damage player
				{
					AddDamageEvent<PlayerTag>(SystemAPI.GetSingletonEntity<PlayerTag>(), 50f);
				}
				else if (Input.GetKeyDown(KeyCode.H)) // heal player
				{
					AddDamageEvent<PlayerTag>(SystemAPI.GetSingletonEntity<PlayerTag>(), -100f);
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

		private void AddDamageEvent<T>(Entity entity, float value)
		{
			SystemAPI.GetSingletonBuffer<DamageEvent>().Add(new DamageEvent
			{
				Value = value,
				Target = entity,
			});
		}
	}
}