using Unity.Burst;
using Unity.Entities;

namespace SpinMech
{
	[UpdateBefore(typeof(PhaseSystem))]
	public partial struct CleanupSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<DamageEvent>();
			state.RequireForUpdate<DestroyEvent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			SystemAPI.GetSingletonBuffer<DamageEvent>().Clear();
			SystemAPI.GetSingletonBuffer<DestroyEvent>().Clear();
		}
	}

	[UpdateAfter(typeof(CleanupSystem))]
	[UpdateBefore(typeof(PhaseSystem))]
	public partial struct GameOverSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<PhaseComponent>();
			state.RequireForUpdate<PlayerTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			ref PhaseComponent phase = ref SystemAPI.GetSingletonRW<PhaseComponent>().ValueRW;

			if (phase.Current != GamePhase.None && SystemAPI.IsComponentEnabled<DestroyedComponent>(SystemAPI.GetSingletonEntity<PlayerTag>()))
			{
				phase.GoToNext = false;
				phase.Current = GamePhase.None;
				phase.Time = 0f;
				phase.Timer = 0f;

				// TODO: process game over
			}
		}
	}
}