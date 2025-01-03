using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace SpinMech
{
	public partial struct PhaseSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameConfig>();
			state.RequireForUpdate<PhaseComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			GameConfig config = SystemAPI.GetSingleton<GameConfig>();
			ref PhaseComponent phase = ref SystemAPI.GetSingletonRW<PhaseComponent>().ValueRW;

			phase.Time += SystemAPI.Time.DeltaTime;

			if (phase.IsTimedPhase() && phase.Time >= phase.Timer)
			{
				phase.GoToNext = true;
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
				// else TODO
			}
		}
	}
}