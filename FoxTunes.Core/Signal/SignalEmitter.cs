using FoxTunes.Interfaces;
using System.Threading.Tasks;

namespace FoxTunes
{
    public class SignalEmitter : StandardComponent, ISignalEmitter
    {
        public Task Send(ISignal signal)
        {
            return this.OnSignal(signal);
        }

        protected virtual Task OnSignal(ISignal signal)
        {
            if (this.Signal == null)
            {
                return TaskEx.FromResult(false);
            }
            return this.Signal(this, signal);
        }

        public event SignalEventHandler Signal;
    }
}
