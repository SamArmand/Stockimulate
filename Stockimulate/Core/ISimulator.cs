using System.Threading.Tasks;
using Stockimulate.Enums;

namespace Stockimulate.Core
{
    public interface ISimulator
    {
        SimulationState SimulationState { get; }

        SimulationMode SimulationMode { set; }

        Task PlayAsync();

        void Reset();
    }
}