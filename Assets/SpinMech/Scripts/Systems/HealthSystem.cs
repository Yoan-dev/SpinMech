using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace SpinMech
{
	[UpdateAfter(typeof(PhaseSystem))]
	public partial struct HealthSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<PhaseComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency = new DamageEventJob
			{
				LookupEntity = SystemAPI.GetSingletonEntity<DamageEvent>(), // game singleton
				DamageEventBufferLookup = SystemAPI.GetBufferLookup<DamageEvent>(true),
				DestroyEventBufferLookup = SystemAPI.GetBufferLookup<DestroyEvent>(),
				HealthLookup = SystemAPI.GetComponentLookup<HealthComponent>(),
				DestroyedLookup = SystemAPI.GetComponentLookup<DestroyedComponent>(),
			}.Schedule(state.Dependency);
		}

		[BurstCompile]
		public partial struct DamageEventJob : IJob
		{
			public Entity LookupEntity;
			[ReadOnly] public BufferLookup<DamageEvent> DamageEventBufferLookup;
			public BufferLookup<DestroyEvent> DestroyEventBufferLookup;
			public ComponentLookup<HealthComponent> HealthLookup;
			public ComponentLookup<DestroyedComponent> DestroyedLookup;

			public void Execute()
			{
				DynamicBuffer<DamageEvent> damageEventBuffer = DamageEventBufferLookup[LookupEntity];
				DynamicBuffer<DestroyEvent> destroyEventBuffer = DestroyEventBufferLookup[LookupEntity];
				for (int i = 0; i < damageEventBuffer.Length; i++)
				{
					DamageEvent damageEvent = damageEventBuffer[i];

					// we assume target is still valid and has a health component
					ref HealthComponent health = ref HealthLookup.GetRefRW(damageEvent.Target).ValueRW;

					bool isAlreadyDestroyed = health.Value <= 0f;

					health.Value = math.clamp(health.Value - damageEvent.Value, 0f, health.Max);

					if (!isAlreadyDestroyed && health.Value <= 0f)
					{
						// add destroy (= death) event
						destroyEventBuffer.Add(new DestroyEvent { Entity = damageEvent.Target });
						DestroyedLookup.SetComponentEnabled(damageEvent.Target, true);
					}
				}
			}
		}
	}
}