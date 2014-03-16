using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentsSys
{
    public interface IAgent<TAgent>
    {
        TAgent PostIn(Action message);
        void Post(Action message);
    }
}
