using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentsSys.Extensions;

namespace AgentsSys
{
    public class AAgentRunner
    {
        #region private fields

        private int _maxAgentsInUse = 1;
        private int _maxPostsPerAgent = 1;
        private int _currentPost = 0;
        private int _currentAgentPosition = 0;

        private List<AgentBox<ActionAgent>> _agts = new List<AgentBox<ActionAgent>>();

        private bool _isPublishable = false;

        #endregion

        #region ctor

        public AAgentRunner(int maxAgentsInUse)
        {
            _maxAgentsInUse = maxAgentsInUse;
        }

        public AAgentRunner(int maxAgentsInUse, int maxPostsPerAgent)
            : this(maxAgentsInUse)
        {
            _maxPostsPerAgent = maxPostsPerAgent;
        }

        #endregion

        public void Run(Action actionToPost)
        {
            if (_agts.Count == 0)
            {
                //_agents.Add((ActionAgent)ActionAgent.MakeAgent().PostIn(actionToPost));
                _agts.Add(new AgentBox<ActionAgent>(ActionAgent.MakeAgent().PostIn(actionToPost), 1));
                _currentAgentPosition = 0;
                _currentPost = 1;
            }
            else
            {
                if (_currentPost == _maxPostsPerAgent)
                {
                    // _agents.Add((ActionAgent)ActionAgent.MakeAgent().PostIn(actionToPost));
                    _agts.Add(new AgentBox<ActionAgent>(ActionAgent.MakeAgent().PostIn(actionToPost), 1));
                    _currentAgentPosition++;
                    _currentPost = 1;
                }
                else
                {
                    _currentPost++;
                    //_agents[_currentAgentPosition].PostIn(actionToPost);
                    _agts[_currentAgentPosition].GetAgent.PostIn(actionToPost);

                    _agts[_currentAgentPosition].NumPost = _agts[_currentAgentPosition].NumPost + 1;
                }
            }

            //_agents.Add(ActionAgent.MakeAgent().PostIn(actionToPost));

            if ((_agts.Count() % _maxAgentsInUse == 0) && (_agts[_currentAgentPosition].NumPost == _maxPostsPerAgent))
            {
                WaitAllAgentsCompletion();
                //_agents.ForEach(a => a.WaitAllMsgProcessing());
                //_agents.ForEach(a => a.IsListening() = false);
                //_agents.RemoveWhere(a => !a.IsListening());
                _agts.RemoveWhere(a => !a.GetAgent.IsListening());
            }
        }

        public void WaitAllAgentsCompletion()
        {
            foreach (var agent in _agts)
            {
                while (agent.GetAgent.MessagesReceived < agent.NumPost) { }
                agent.GetAgent.WaitAllMsgProcessing();
                //agent.ReceiveAndComplete();
                //agent.WaitAll();
            }
        }
    }
}
