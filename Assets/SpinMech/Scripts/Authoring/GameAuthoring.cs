using Unity.Entities;
using UnityEngine;

namespace SpinMech
{
	[DisallowMultipleComponent]
	public class GameAuthoring : MonoBehaviour
	{
		private class Baker : Baker<GameAuthoring>
		{
			public override void Bake(GameAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new LevelComponent());
				AddComponent(entity, new ScoreComponent());
			}
		}
	}
}