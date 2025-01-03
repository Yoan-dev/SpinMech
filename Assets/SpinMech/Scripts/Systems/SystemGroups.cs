using Unity.Entities;

namespace SpinMech
{
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public partial class GameSystemGroup : ComponentSystemGroup
	{
	}
}