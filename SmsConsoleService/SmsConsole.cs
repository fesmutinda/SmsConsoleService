using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmsConsoleService
{
    partial class SmsConsole : ServiceBase
    {
        private ManualResetEvent _stop = new ManualResetEvent(false);

        public SmsConsole() => this.InitializeComponent();

        protected override void OnStart(string[] args)
        {
            this._stop.Reset();
            ThreadPool.RegisterWaitForSingleObject(_stop, new WaitOrTimerCallback(this.PeriodicProcess), null, 1000, true);
        }

        protected override void OnStop() => this._stop.Set();

        public void Start() => this.OnStart((string[])null);

        public new void Stop() => this.OnStop();

        private void PeriodicProcess(object state, bool timeout)
        {
            if (!timeout)
                return;
            new SmsEngine().RunSmses();
            ThreadPool.RegisterWaitForSingleObject((WaitHandle)this._stop, new WaitOrTimerCallback(this.PeriodicProcess), null, 1000, true);
        }
    }
}
