using FluentValidation.Results;

namespace NerdStore.Core.Messages
{
    public abstract class Command : Message
    {
        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; protected set; }
        public ValidationResult ValidationResult { get; protected set; }

        public abstract bool EhValido();
    }
}