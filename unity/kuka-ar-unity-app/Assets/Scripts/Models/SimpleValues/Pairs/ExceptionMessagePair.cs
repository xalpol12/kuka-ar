using System;

namespace Scenes.RotationAndScalingWithButtons.Scripts.Models.SimpleValues.Pairs
{
    public struct ExceptionMessagePair
    {
        public string ExceptionName { get; set; }
        public string ExceptionMessage { get; set; }

        public ExceptionMessagePair(string exceptionName, string exceptionMessage)
        {
            ExceptionName = exceptionName;
            ExceptionMessage = exceptionMessage;
        }

        public override bool Equals(object obj)
        {
            return obj is ExceptionMessagePair other && Equals(other);
        }

        public bool Equals(ExceptionMessagePair other)
        {
            return ExceptionName == other.ExceptionName && ExceptionMessage == other.ExceptionMessage;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ExceptionName, ExceptionMessage);
        }

        public override string ToString()
        {
            return $"{nameof(ExceptionName)}: {ExceptionName}, {nameof(ExceptionMessage)}: {ExceptionMessage}";
        }
    }
}
