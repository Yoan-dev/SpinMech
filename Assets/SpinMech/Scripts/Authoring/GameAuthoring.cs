using Unity.Entities;
using UnityEngine;

namespace SpinMech
{
	[DisallowMultipleComponent]
	public class GameAuthoring : MonoBehaviour
	{
		public GameConfig Config;

		private class Baker : Baker<GameAuthoring>
		{
			public override void Bake(GameAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, authoring.Config);
				AddComponent(entity, new PhaseComponent());
				AddComponent(entity, new BossCounterComponent());
				AddComponent(entity, new ScoreComponent());
			}
		}
	}
}