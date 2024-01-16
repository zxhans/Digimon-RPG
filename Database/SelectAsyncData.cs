using MySql.Data.MySqlClient;
using System;

namespace Digimon_Project.Database
{
    public struct SelectAsyncData<T>
    {
        public readonly Func<T> Callback;
        public readonly MySqlCommand Command;

        public SelectAsyncData(Func<T> callback, MySqlCommand command)
        {
            this.Callback = callback;
            this.Command = command;
        }
    }
}
