using System;
using System.Collections.Generic;
using System.Text;

namespace AOC.Common
{
    public interface ISignalListener
    {
        public void AddSignal(long signal);
        public List<long> Inputs { get; }
    }
    public class SignalListener : ISignalListener
    {
        private Action<SignalListener> _signalled = null;

        public List<long> Inputs { get; protected set; } = new List<long>();
        protected virtual void Signalled() { _signalled?.Invoke(this); }
        public void AddSignal(long signal)
        {
            Inputs.Add(signal);
            Signalled();
        }

        public SignalListener() { }

        public SignalListener(Action<SignalListener> signalled)
        {
            _signalled = signalled;
        }
    }
}
