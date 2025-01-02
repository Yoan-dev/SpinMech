using Unity.Entities;
using UnityEngine;

namespace SpinMech
{
	[DisallowMultipleComponent]
	public class PlayerAuthoring : MonoBehaviour
	{
		private class Baker : Baker<PlayerAuthoring>
		{
			public override void Bake(PlayerAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new PlayerTag());
			}
		}
	}
}