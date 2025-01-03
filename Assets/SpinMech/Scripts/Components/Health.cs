using Unity.Entities;

namespace SpinMech
{
	public struct HealthComponent : IComponentData
	{
		public float Value;
		public float Max;

		public float Percentage => Value / Max;

		public static HealthComponent New(float value)
		{
			return new HealthComponent
			{
				Value = value,
				Max = value,
			};
		}
	}

	public struct DestroyedComponent : IComponentData, IEnableableComponent { }

	[InternalBufferCapacity(0)]
	public struct DamageEvent : IBufferElementData
	{
		public float Value;
		public Entity Target;
	}

	[InternalBufferCapacity(0)]
	public struct DestroyEvent : IBufferElementData
	{
		public Entity Entity;
	}
}