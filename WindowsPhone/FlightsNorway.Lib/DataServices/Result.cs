using System;

namespace FlightsNorway.Lib.DataServices
{
    public delegate void ResultCallback<T>(Result<T> result);

    public class Result<T>
    {
        public Exception Error { get; private set; }
        public T Value { get; private set; }

        public Result(T value)
        {
            Value = value;
        }

        public Result(Exception error)
        {
            Error = error;
        }

        public bool HasError()
        {
            return Error != null;
        }
    }
}
