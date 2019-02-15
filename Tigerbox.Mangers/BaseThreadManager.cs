using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tigerbox.Mangers
{
    public abstract class BaseThreadManager
    {
        protected Thread _thread;

        protected BaseThreadManager()
        {
            _thread = new Thread(new ThreadStart(this.ExecuteThread));
        }

        // Thread methods / properties
        public virtual void Start() => _thread.Start();
        public virtual void Join() => _thread.Join();
        public virtual bool IsAlive => _thread.IsAlive;
        public virtual void Stop() => _thread.Abort();
        public abstract void ExecuteThread();
    }
}
