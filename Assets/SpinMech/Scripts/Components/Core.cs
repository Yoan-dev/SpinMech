using System;
using Unity.Entities;

namespace SpinMech
{
	public enum GamePhase
	{
		None = 0,
		Arrival,
		Fight,
		End,
		Scavenge,
		Count = 5,
	}

	[Serializable]
	public struct GameConfig : IComponentData
	{
		public float ArrivalTimer;
		public float EndTimer;

		public float GetPhaseTimer(GamePhase phase)
		{
			return phase == GamePhase.Arrival ? ArrivalTimer : phase == GamePhase.End ? EndTimer : 0f;
		}
	}

	public struct GamePrefabs : IComponentData
	{
		public Entity BossMech;
	}

	public struct PhaseComponent : IComponentData
	{
		public float Time;
		public float Timer;
		public GamePhase Current;
		public bool GoToNext;

		public bool IsTimedPhase()
		{
			return Current == GamePhase.End || Current == GamePhase.Arrival;
		}
	}

	public struct BossCounterComponent : IComponentData
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