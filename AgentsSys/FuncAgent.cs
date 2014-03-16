using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AgentsSys
{
    public class FuncAgent<TResult>
    {
        private readonly BufferBlock<Func<TResult>> _inMsg = new BufferBlock<Func<TResult>>();
        private readonly BufferBlock<TResult> _outMsg = new BufferBlock<TResult>();

        private bool _listening = false;

        private FuncAgent()
        {
            _listening = true;
        }

        public bool IsListening()
        {
            return _listening;
        }

        public static FuncAgent<TResult> MakeAgent()
        {
            var agent = new FuncAgent<TResult>();
            agent.processMessages();
            return agent;
        }

        protected async void processMessages()
        {
            while (_listening)
            {
                var message = await _inMsg.ReceiveAsync();
                try
                {
                    _outMsg.Post(message());
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Post(Func<TResult> message)
        {
            _inMsg.Post(message);
        }

        public FuncAgent<TResult> PostIn(Func<TResult> message)
        {
            _inMsg.Post(message);
            return this;
        }

        public TResult Receive()
        {
            return _outMsg.Receive();
        }

        public TResult ReceiveAndComplete()
        {
            var res = _outMsg.Receive();
            _listening = false;
            _outMsg.Complete();
            return res;
        }

        public IEnumerable<TResult> ReceiveAllOutMsgsLazy()
        {
            TResult res;
            while ((res = _outMsg.Receive()) != null)
            {
                yield return res;
            }
        }
    }
}
