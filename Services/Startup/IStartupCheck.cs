using System;

namespace ModularWPFTemplate.Services.Startup
{
    public interface IStartupCheck
    {
        /// <summary>
        /// Status of process displayed to user in startup progress
        /// </summary>
        public IObservable<string> DisplayStatus { get; }

        /// <summary>
        /// Do startup check operation
        /// </summary>
        public void DoCheck();
    }
}
