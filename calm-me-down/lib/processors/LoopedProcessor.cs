using System.Collections.Generic;

namespace hello.lib.processors
{
    public abstract class LoopedProcessor<T>
    {
        private readonly List<T> items;
        private int nextItemPtr;

        protected LoopedProcessor(List<T> items)
        {
            this.items = items;
        }

        public T GetNext()
        {
            return items[nextItemPtr++ % items.Count];
        }
    }
}