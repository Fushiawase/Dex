using System.Threading.Tasks;
using UnityEngine;

namespace InfoLoader
{
    public class WaitForTask : CustomYieldInstruction
    {
        private readonly Task task;

        public WaitForTask(Task task) => this.task = task;

        public override bool keepWaiting => !task.IsCompleted;
    }
}