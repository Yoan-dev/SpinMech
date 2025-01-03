using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace SpinMech
{
	[UpdateInGroup(typeof(GameSystemGroup), OrderFirst = true)]
	public partial struct PhaseSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameConfig>();
			state.RequireForUpdate<GamePrefabs>();
			state.RequireForUpdate<PhaseComponent>();
		}

		//[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			GameConfig config = SystemAPI.GetSingleton<GameConfig>();
			GamePrefabs prefabs = SystemAPI.GetSingleton<GamePrefabs>();
			ref PhaseComponent phase = ref SystemAPI.GetSingletonRW<PhaseComponent>().ValueRW;

			phase.Time += SystemAPI.Time.DeltaTime;

			if (phase.IsTimedPhase() && phase.Time >= phase.Timer)
			{
				phase.GoToNext = true;
			}
			else if (!phase.GoToNext && phase.Current == GamePhase.Fight)
			{
				// check if there are remaining bosses
				foreach (var destroyed in SystemAPI.Query<EnabledRefRO<DestroyedComponent>>().WithAll<BossTag>())
				{
					if (destroyed.ValueRO)
					{
						phase.GoToNext = true;
						break;
					}
				}
			}

			if (phase.GoToNext)
			{
				phase.GoToNext = false;
				phase.Current = (GamePhase)math.max(1, ((int)phase.Current + 1) % (int)GamePhase.Count);
				phase.Time = 0f;
				phase.Timer = config.GetPhaseTimer(phase.Current);

				// trigger events according to phase
				if (phase.Current == GamePhase.Arrival)
				{
					ref BossCounterComponent bossCounter = ref SystemAPI.GetSingletonRW<BossCounterComponent>().ValueRW;
					bossCounter.Value++;
				}
				else if (phase.Current == GamePhase.Fight)
				{
					// spawn boss
					// TODO: spawn during Arrival (dropship/else), then activate combat when starting Fight
					state.EntityManager.Instantiate(prefabs.BossMech);
				}
				else if (phase.Current == GamePhase.End)
				{
					// TODO
				}
				else if (phase.Current == GamePhase.Scavenge)
				{
					Entity bossMech = SystemAPI.GetSingletonEntity<BossTag>();

					// open scavenge UI
					// TODO: send modules and loot parameters
					GameUI.Instance.OpenScavengeUI();

					// destroy boss
					state.EntityManager.DestroyEntity(bossMech);
				}
			}
		}
	}
}