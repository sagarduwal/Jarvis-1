using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Jarvis.Core.Extensibility;
using NLog;

namespace Jarvis.Core.Infrastructure
{
    public class SimpleScheduler : IScheduler
    {
        IEnumerable<IDisposable> _triggers;
        Logger _logger = LogManager.GetCurrentClassLogger();

        public void Initialize(IEnumerable<IScheduledJob> jobs) {

            _triggers = jobs.Select(j => Observable.Timer(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1)).Subscribe(
                l => {
                    j.Execute();
                }, e => _logger.ErrorException("Unhandled Exception:", e))).ToList();
        }
    }
}