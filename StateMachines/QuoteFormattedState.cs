namespace Company.StateMachines
{
    using System;
    using MassTransit;

    public class QuoteFormattedState :
        SagaStateMachineInstance
    {
        public int CurrentState { get; set; }

        public string Name { get; set; }
        public string Timestamp { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
