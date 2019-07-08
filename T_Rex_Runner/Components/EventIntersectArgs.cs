using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Rex_Runner.Components
{
    public class EventIntersectArgs : EventArgs
    {
        protected Unit first;
        protected Unit second;

        public Unit First { get => first; set => first = value; }
        public Unit Second { get => second; set => second = value; }

        public EventIntersectArgs(Unit first, Unit second)
        {
            this.first = first;
            this.second = second;
        }

    }
}
