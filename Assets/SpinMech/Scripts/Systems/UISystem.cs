using Unity.Burst;
using Unity.Entities;

namespace SpinMech
{
	[UpdateAfter(typeof(GameSystemGroup))]
	[UpdateBefore(typeof(CleanupSystem))]
	public partial struct UISystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<BossCounterComponent>();
			state.RequireForUpdate<ScoreComponent>();
			state.RequireForUpdate<PlayerTag>();
		}

		//[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
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

			GameUI.Instance.UpdateUI(in bossCounter, in score, in playerHealth, in bossHealth);
		}
	}
}