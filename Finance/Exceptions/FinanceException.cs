using System;

namespace Finance.Exceptions
{
    public class FinanceException : Exception
    {
        public FinanceException(string message) : base(message) { }
    }
}