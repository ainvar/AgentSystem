using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AgentsSys
{
    public class ActionAgent : IAgent<ActionAgent>
    {
        private readonly BufferBlock<Action> _inAct = new BufferBlock<Action>();
        private readonly BufferBlock<Tuple<Action, Exception>> _inFailedAct = new BufferBlock<Tuple<Action, Exception>>();

        private List<Task> _inTasks = new List<Task>();

        private bool _isPublishable = false;

        private bool _listening = false;

        private int _messagesReceived = 0;

        public bool IsListening()
        {
            return _listening;
        }

        public int MessagesReceived
        {
            get
            {
                return _messagesReceived;
            }
        }

        public static ActionAgent MakeAgent()
        {
            var actor = new ActionAgent();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("creato l'agent");
            Console.ResetColor();
            actor.handleIncomingMessages();
            return actor;
        }

        private ActionAgent()
        {
            _listening = true;
        }

        public void Post(Action message)
        {
            _inAct.Post(message);
        }

        public ActionAgent PostIn(Action message)
        {
            _inAct.Post(message);
            return this;
        }

        protected async void handleIncomingMessages()
        {
            while (_listening)
            {
                var message = await _inAct.ReceiveAsync();
                try
                {
                    _inTasks.Add(Task.Run(() => { message(); }));
                    _messagesReceived++;
                }
                catch (Exception ex)
                {
                    _inFailedAct.Post(new Tuple<Action, Exception>(message, ex));
                }
            }
        }

        public void WaitAllMsgProcessing()
        {
            //inTasks.ForEach(t => t.Wait());
            _listening = false;
            Task.WaitAll(_inTasks.ToArray(), Timeout.Infinite);
        }

        public void WaitAll()
        {
            _inAct.Completion.Wait();
            _listening = false;
        }

        public void ReceiveAndComplete()
        {
            _listening = false;
            var res = _inAct.Receive();

            _inAct.Complete();
        }
    }

}
