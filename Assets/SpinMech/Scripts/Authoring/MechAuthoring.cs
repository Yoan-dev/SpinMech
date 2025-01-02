using Unity.Entities;
using UnityEngine;

namespace SpinMech
{
	[DisallowMultipleComponent]
	public class MechAuthoring : MonoBehaviour
	{
		public int Health = 100;

		private class Baker : Baker<MechAuthoring>
		{
			public override void Bake(MechAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, HealthComponent.New(authoring.Health));
			}
		}
	}
}