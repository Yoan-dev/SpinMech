using Unity.Entities;

namespace SpinMech
{
	public struct LevelComponent : IComponentData
	{
		public int Value;
	}

	public struct ScoreComponent : IComponentData
	{
		public int Value;
	}

	public struct PlayerTag : IComponentData { }

	public struct BossTag : IComponentData { }
}