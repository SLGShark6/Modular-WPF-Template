using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ModularWPFTemplate.Services.Startup
{
    public abstract class BaseStartupCheck : IStartupCheck
    {
        /// <summary>
        /// Source subject for the current status of the check
        /// </summary>
        protected ISubject<string> _StatusSource;

        /// <summary>
        /// Observable stream for the current status of the check
        /// </summary>
        public IObservable<string> DisplayStatus { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public BaseStartupCheck()
        {
            _StatusSource = new ReplaySubject<string>();
            DisplayStatus = _StatusSource.AsObservable();
        }

        /// <summary>
        /// Do startup check operation
        /// </summary>
        public abstract void DoCheck();
    }
}
