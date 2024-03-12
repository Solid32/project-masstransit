namespace Company.StateMachines
{
  using Contracts;
  using MassTransit;

  public class StateMachStateMachine :
      MassTransitStateMachine<QuoteFormattedState>
  {
    public StateMachStateMachine()
    {
      InstanceState(x => x.CurrentState, Created);

      Event(() => QuoteFormatted, x => x.CorrelateById(context => context.Message.CorrelationId));
      Event(() => QuoteFormattedCount , x => x.CorrelateById(context => context.Message.CorrelationId));

      Initially(
          When(QuoteFormatted)
              .Then(context =>
      {
        context.Saga.Name = context.Message.Name;
        QuoteFormattedQuotes qfq = new()
        {
          CorrelationId = context.Saga.CorrelationId,
          Name = context.Saga.Name,
          Timestamp = context.Saga.Timestamp
        };
        context.Publish(qfq);
      }
        )
              .TransitionTo(Continue)
      );
      During(Continue,
      When(QuoteFormattedCount)
        .Then(context =>
              {
        context.Saga.Name = context.Message.Name;
        QuoteFormattedCount qfc = new()
        {
          CorrelationId = context.Saga.CorrelationId,
          Name = context.Saga.Name
        };
        context.Publish(qfc);
      }
        )
        .TransitionTo(Complete)
      );
      SetCompletedWhenFinalized();
    }

    public State Created { get; private set; }
    public State Continue { get; private set; }
    public State Complete {get; private set; }

    public Event<IQuoteFormatted> QuoteFormatted { get; private set; }
    public Event<QuoteFormattedQuotes> QuoteFormattedCount { get; private set; }
  }
  }
