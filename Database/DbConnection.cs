using System;
using System.Text;
using System.Collections.Generic;

using MySql.Data.MySqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Digimon_Project.Database
{
    public class DbConnection
    {
        private readonly string connectionString;

        public DbConnection(string server, uint port, string username, string password, string database)
        {
            MySqlConnectionStringBuilder cs = new MySqlConnectionStringBuilder()
            {
                Server = server,
                Port = port,
                UserID = username,
                Password = password,
                Database = database,
                Pooling = true,
                MinimumPoolSize = 20
            };

            connectionString = cs.GetConnectionString(true);
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public bool Check()
        {
            bool result = false;
            Console.WriteLine("Connecting to the MySQL database...");
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Successfully connected to the database, we are ready to boot.");
                    result = true;
                } // Test if we can connect to the database.
                catch { Console.WriteLine("Failed to connect to the database."); }
                finally
                {
                    try { conn.Close(); }  // Exception will be handeled by the parent.
                    catch { }
                }
            }

            return result;
        }

        private Func<T> Create<T>()
        {
            return Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
        }

        public T Select<T>(string select, string table) where T : ISelectResult
        {
            return this.Select<T>(select, table, "", null);
        }

        public T Select<T>(string select, string table, string extra, QueryParameters parameters = null) where T : ISelectResult
        {

            StringBuilder queryBuilder = new StringBuilder("SELECT ");
            queryBuilder.Append(select);
            queryBuilder.Append(" FROM ");
            queryBuilder.Append(table);

            if (extra.Length > 0)
            {
                queryBuilder.Append(" ");
                queryBuilder.Append(extra);
            }

            using (MySqlConnection conn = GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn))
                {
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> entry in parameters)
                        {
                            cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                        }
                    }

                    /**
                    if(Emulator.Enviroment.Teste)
                    Debug.Print(queryBuilder.ToString());
                    /**/

                    conn.Open();

                    try
                    {
                        T result = Create<T>()();// Create new instance w/ compiled lambda.
                        MySqlDataReader reader = cmd.ExecuteReader();
                        result.OnExecuted(reader);

                        // Clean-up.
                        if (!reader.IsClosed)
                            reader.Close();

                        reader.Dispose();

                        return result;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("MySQL Error: {0}.", e.ToString());
                    }
                    conn.Close();
                }
            }

            return default(T);
        }

        /// <summary>
        /// NOT IMPLEMENTED.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        public void SelectAsync<T>(Func<T> callback, string query, QueryParameters parameters)
        {
            throw new NotImplementedException();

            //using (MySqlConnection conn = GetConnection())
            //{
            //    using (MySqlCommand cmd = new MySqlCommand(query, conn))
            //    {
            //        foreach (KeyValuePair<string, object> entry in parameters)
            //        {
            //            cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
            //        }
            //        conn.Open();

            //        try
            //        {
            //            //cmd.BeginExecuteReader(new AsyncCallback(SelectAsyncCallback), null);
            //            //T result = Create<T>()();// Create new instance w/ compiled lambda.
            //            //result.OnExecuted(cmd.ExecuteReader());
            //            //return result;

            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine("MySQL Error: {0}.", e.ToString());
            //        }
            //        conn.Close();
            //    }
            //}

            //return default(T);
        }

        private void SelectAsyncCallback(IAsyncResult iAr)
        {
            //iAr.AsyncState
        }

        private string BuildInsertQuery(string table, IEnumerable<string> keys)
        {
            StringBuilder queryBuilder = new StringBuilder("INSERT INTO ");

            queryBuilder.Append(table);
            queryBuilder.Append("(`");
            queryBuilder.Append(string.Join("`,`", keys));
            queryBuilder.Append("`) VALUES (@");
            queryBuilder.Append(string.Join(",@", keys));
            queryBuilder.Append(");");

            return queryBuilder.ToString();
        }

        public T Insert<T>(string table, QueryParameters insertValues) where T : struct, IConvertible
        {
            // Insert
            T result = default(T);

            using (MySqlConnection conn = GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(BuildInsertQuery(table, insertValues.Keys), conn))
                {
                    foreach (KeyValuePair<string, object> entry in insertValues)
                    {
                        cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        result = (T)Convert.ChangeType(cmd.LastInsertedId, typeof(T));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("MySQL Error: {0}.", e.ToString());
                    }
                    finally
                    {
                        try { conn.Close(); }
                        catch { }
                    }

                }
            }

            return result;
        }

        public void InsertAsync(string table, QueryParameters insertValues, QueryCallback callback = null)
        {
            MySqlConnection conn = GetConnection();
            MySqlCommand cmd = new MySqlCommand(BuildInsertQuery(table, insertValues.Keys), conn);

            foreach (KeyValuePair<string, object> entry in insertValues)
            {
                cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
            }

            try
            {
                conn.Open();
                cmd.BeginExecuteNonQuery(new AsyncCallback(NonQueryCallback), new NonQueryData(callback, cmd));
            }
            catch (Exception e)
            {
                Console.WriteLine("MySQL Error: {0}.", e.ToString());
                try { conn.Close(); }
                catch { }
                finally { cmd.Dispose(); conn.Dispose(); }
            }
        }

        public void Insert(string table, QueryParameters[] insertValues)
        {
            // Multi Insert
            throw new NotImplementedException();
        }

        public int Update(string table, QueryParameters updateValues, string extra = "", QueryParameters extraParameters = null, string extraUpdateValues = "")
        {
            int result = 0;

            StringBuilder queryBuilder = new StringBuilder("UPDATE ");
            queryBuilder.Append(table);
            queryBuilder.Append(" SET ");

            string[] settingsJoin = new string[updateValues.Keys.Count];
            for (int i = 0; i < settingsJoin.Length; i++)
            {
                settingsJoin[i] = string.Format("{0}=@{0}", updateValues.ElementAt(i).Key);
            }

            queryBuilder.Append(string.Join(",", settingsJoin));

            if (extraUpdateValues.Length > 0 && settingsJoin.Length > 0)
                queryBuilder.Append(",");

            queryBuilder.Append(extraUpdateValues);

            if (extra.Length > 0)
            {
                queryBuilder.Append(" ");
                queryBuilder.Append(extra);
            }

            using (MySqlConnection conn = GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn))
                {
                    foreach (KeyValuePair<string, object> entry in updateValues)
                    {
                        cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    if (extraParameters != null)
                    {
                        foreach (KeyValuePair<string, object> entry in extraParameters)
                        {
                            cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                        }
                    }

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("MySQL Error: {0}.", e.ToString());
                    }
                    finally
                    {
                        try { conn.Close(); }
                        catch { }
                    }

                }
            }

            return result;
        }

        public int Delete(string table, string extra = "", QueryParameters extraParameters = null)
        {
            int result = 0;

            StringBuilder queryBuilder = new StringBuilder("DELETE FROM ");
            queryBuilder.Append(table);

            if (extra.Length > 0)
            {
                queryBuilder.Append(" ");
                queryBuilder.Append(extra);
            }

            using (MySqlConnection conn = GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn))
                {
                    if (extraParameters != null)
                    {
                        foreach (KeyValuePair<string, object> entry in extraParameters)
                        {
                            cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                        }
                    }

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("MySQL Error: {0}.", e.ToString());
                    }
                    finally
                    {
                        try { conn.Close(); }
                        catch { }
                    }

                }
            }

            return result;
        }

        public void UpdateAsync(QueryCallback callback, string table, QueryParameters updateValues, string extra = "", QueryParameters extraParameters = null, string extraUpdateValues = "")
        {
            StringBuilder queryBuilder = new StringBuilder("UPDATE ");
            queryBuilder.Append(table);
            queryBuilder.Append(" SET ");

            string[] settingsJoin = new string[updateValues.Keys.Count];
            for (int i = 0; i < settingsJoin.Length; i++)
            {
                settingsJoin[i] = string.Format("{0}=@{0}", updateValues.ElementAt(i).Key);
            }

            queryBuilder.Append(string.Join(",", settingsJoin));

            if (extraUpdateValues.Length > 0 && settingsJoin.Length > 0)
                queryBuilder.Append(",");

            queryBuilder.Append(extraUpdateValues);

            if (extra.Length > 0)
            {
                queryBuilder.Append(" ");
                queryBuilder.Append(extra);
            }

            MySqlConnection conn = GetConnection();
            MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn);

            foreach (KeyValuePair<string, object> entry in updateValues)
            {
                cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
            }

            if (extraParameters != null)
            {
                foreach (KeyValuePair<string, object> entry in extraParameters)
                {
                    cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }
            }

            try
            {
                conn.Open();
                cmd.BeginExecuteNonQuery(new AsyncCallback(NonQueryCallback), new NonQueryData(callback, cmd));
            }
            catch (Exception e)
            {
                Console.WriteLine("MySQL Error: {0}.", e.ToString());
                try { conn.Close(); }
                catch { }
                finally { cmd.Dispose(); conn.Dispose(); }
            }

        }

        private void NonQueryCallback(IAsyncResult iAr)
        {
            int result = 0;
            NonQueryData data = (NonQueryData)iAr.AsyncState;

            using (MySqlCommand cmd = data.Command)
            {
                try
                {
                    result = cmd.EndExecuteNonQuery(iAr);
                }
                catch (Exception e) { Console.WriteLine("MySQL Exception: {0}", e.ToString()); }
                finally
                {
                    try { cmd.Connection.Close(); }
                    catch { }
                    finally { cmd.Connection.Dispose(); }
                }
            }

            if (data.Callback != null && data.Callback.Callback != null)
            {
                data.Callback.Callback(result, data.Callback.Parameters);
            }
        }

        /// <summary>
        /// Executes a raw query with the provided parameters.
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="parameters">Parameters that need to be escaped</param>
        /// <returns></returns>
        public int Query(string query, QueryParameters parameters)
        {
            int result = 0;

            using (MySqlConnection conn = GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    foreach (KeyValuePair<string, object> entry in parameters)
                    {
                        cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("MySQL Error: {0}.", e.ToString());
                    }
                    finally
                    {
                        try { conn.Close(); }
                        catch { }
                    }

                }
            }

            return result;
        }

        public void BulkQuery(string query, QueryParameters[] parameters)
        {

        }

        public void MultiQuery(string[] query, QueryParameters[] parameters)
        {

        }
    }
}
