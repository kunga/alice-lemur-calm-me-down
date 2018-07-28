using System.Collections.Generic;

namespace hello.lib.processors
{
    public class FinishingQuestionsProcessor : LoopedProcessor<string>
    {
        private static readonly List<string> Questions = new List<string>
        {
            "Тебе л+учше?",
            "Я помогла тебе?"
        };

        public FinishingQuestionsProcessor() : base(Questions)
        {
        }
    }
}