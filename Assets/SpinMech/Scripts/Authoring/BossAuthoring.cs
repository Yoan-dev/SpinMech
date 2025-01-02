using Unity.Entities;
using UnityEngine;

namespace SpinMech
{
	[DisallowMultipleComponent]
	public class BossAuthoring : MonoBehaviour
	{
		private class Baker : Baker<BossAuthoring>
		{
			public override void Bake(BossAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new BossTag());
			}
		}
	}
}