namespace Company.StateMachines
{
    using System;
    using MassTransit;

    public class QuoteFormattedState :
        SagaStateMachineInstance // Contrat de la SAGA
    {
        public int CurrentState { get; set; }

        public string Name { get; set; }
        public string Timestamp { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
