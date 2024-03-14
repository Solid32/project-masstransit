namespace Company.StateMachines
{
  using Contracts;
  using MassTransit;

  public class QuotesStateMachine :
      MassTransitStateMachine<QuoteFormattedState> // Création de la SagaStateMachine
  {
    public QuotesStateMachine()
    {
      InstanceState(x => x.CurrentState, Created); // Instance de départ

      Event(() => QuoteFormatted, x => x.CorrelateById(context => context.Message.CorrelationId));
      Event(() => QuoteFormattedQuote , x => x.CorrelateById(context => context.Message.CorrelationId));

      Initially(
          When(QuoteFormatted) //Saga déclenché par event QuoteFormatted reçu
              .Then(context =>
      {
        context.Saga.Name = context.Message.Name; // Update l'objet Saga
        QuoteFormattedQuotes qfq = new()
        {
          CorrelationId = context.Saga.CorrelationId,
          Name = context.Saga.Name,
          Timestamp = context.Saga.Timestamp
        };
        context.Publish(qfq); // Publie un nouvel Event
      }
        )
              .TransitionTo(Continue)
      );
      During(Continue,
      When(QuoteFormattedQuote) // Etape déclenchée par QuoteFormattedQuote reçu
        .Then(context =>
              {
        context.Saga.Name = context.Message.Name;
        // Crée un nouvel objet
        QuoteFormattedCount qfc = new()
        {
          CorrelationId = context.Saga.CorrelationId,
          Name = context.Saga.Name
        };
        context.Publish(qfc); // Publie un nouvel Event
      }
        )
        .TransitionTo(Complete)
      );
      SetCompletedWhenFinalized();
    }
    // Description des états de la Saga
    public State Created { get; private set; }
    public State Continue { get; private set; }
    public State Complete {get; private set; }

    // Description des Events de la Saga
    public Event<IQuoteFormatted> QuoteFormatted { get; private set; }
    public Event<QuoteFormattedQuotes> QuoteFormattedQuote { get; private set; }
  }
  }
