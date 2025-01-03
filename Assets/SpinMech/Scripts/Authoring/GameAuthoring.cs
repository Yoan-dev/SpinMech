using Unity.Entities;
using UnityEngine;

namespace SpinMech
{
	[DisallowMultipleComponent]
	public class GameAuthoring : MonoBehaviour
	{
		public GameConfig Config;
		public GameObject BossMechPrefab;

		private class Baker : Baker<GameAuthoring>
		{
			public override void Bake(GameAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				
				// config
				AddComponent(entity, authoring.Config);
				AddComponent(entity, new GamePrefabs
				{
					BossMech = GetEntity(authoring.BossMechPrefab, TransformUsageFlags.Dynamic),
				});
				
				// game
				AddComponent(entity, new PhaseComponent());
				AddComponent(entity, new BossCounterComponent());
				AddComponent(entity, new ScoreComponent());

				// events
				AddBuffer<DamageEvent>(entity);
				AddBuffer<DestroyEvent>(entity);
			}
		}
	}
}