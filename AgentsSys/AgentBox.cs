using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentsSys
{
    public class AgentBox<TAgent> where TAgent : IAgent<TAgent>
    {
        private readonly TAgent _agent;

        public TAgent GetAgent
        {
            get
            {
                return _agent;
            }
        }
        public int NumPost { get; set; }

        public AgentBox(TAgent agent)
        {
            _agent = agent;
        }

        public AgentBox(TAgent agent, int numPost)
            : this(agent)
        {
            NumPost = numPost;
        }
    }
}
