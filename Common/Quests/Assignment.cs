using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.Common.Quests
{
    /// <summary>
    /// Assignments are chains of multiple quests. Each assignment has a Queue of quests to complete in a given amount of time. Assignemnts refresh the morning after they are completed.
    /// </summary>
    public class Assignment
    {
        public Queue<Quest> Quests { get; set; }
        public int DaysLeft { get; set; }
    }
}
