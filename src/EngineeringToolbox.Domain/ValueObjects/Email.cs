namespace EngineeringToolbox.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; }

        //EF
        protected Email() { }
        public Email(string value)
        {
            Value = value;
        }
    }
}
