using System;

namespace Digimon_Project.Database
{
    public class QueryCallback
    {
        public readonly Action<int, object[]> Callback;
        public readonly object[] Parameters;

        public QueryCallback(Action<int, object[]> callback, params object[] parameters)
        {
            Callback = callback;
            Parameters = parameters;
        }
    }
}
