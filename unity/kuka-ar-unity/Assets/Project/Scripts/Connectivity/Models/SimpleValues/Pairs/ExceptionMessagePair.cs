using System;

namespace Project.Scripts.Connectivity.Models.SimpleValues.Pairs
{
    public struct ExceptionMessagePair
    {
        public string ExceptionName { get; set; }
        public string ExceptionMessage { get; set; }

        public override string ToString()
        {
            return $"{nameof(ExceptionName)}: {ExceptionName}, {nameof(ExceptionMessage)}: {ExceptionMessage}; ";
        }
    }
}
