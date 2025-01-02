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
}