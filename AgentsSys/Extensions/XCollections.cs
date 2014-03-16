using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentsSys.Extensions
{
    static public class XCollections
    {
        public static void RemoveWhere<T>(this ICollection<T> Coll,
                                            Func<T, bool> Criteria)
        {
            List<T> forRemoval = Coll.Where(Criteria).ToList();

            foreach (T obj in forRemoval)
            {
                Coll.Remove(obj);
            }
        }

    }
}
