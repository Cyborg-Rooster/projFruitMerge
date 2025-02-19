using System;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Collections.Generic;

class SQLiteManager
{
    private static IDbConnection Database;
    private static string Connection = $@"{Application.persistentDataPath}\Database\Database.sqlite";

    public static bool Initialize()
    {
        Directory.CreateDirectory($@"{Application.persistentDataPath}\Database\");
        Database = new SqliteConnection("URI=file:" + Connection);

        SetDatabaseActive(true);

        bool databaseExist = int.Parse(ReturnValueAsString(CommonQuery.Select("COUNT(*)", "SQLITE_MASTER"))) > 0;

        if (!databaseExist) DatabaseSynchronizer.Synch();

        return databaseExist;
    }

    public static void RunQuery(string query)
    {
        IDbCommand cmd;

        cmd = Database.CreateCommand();
        cmd.CommandText = query;
        cmd.ExecuteReader();
    }

    public static string ReturnValueAsString(string query)
    {
        IDbCommand cmd = Database.CreateCommand();
        cmd.CommandText = query;

        string r;

        using (IDataReader reader = cmd.ExecuteReader())
        {
            r = reader[0].ToString();
        }

        return r;
    }

    public static object[] ReturnValues(string query)
    {
        IDbCommand cmd = Database.CreateCommand();
        cmd.CommandText = query;

        List<object> result = new List<object>();

        using (IDataReader reader = cmd.ExecuteReader())
        {
            for(int i = 0; i < reader.FieldCount; i++)
            {
                result.Add(reader.GetValue(i));
            }
        }

        var r = result.ToArray();
        return r;
    }

    public static int ReturnValueAsInt(string query)
    {
        IDbCommand cmd;
        cmd = Database.CreateCommand();
        cmd.CommandText = query;

        int r;

        using (IDataReader reader = cmd.ExecuteReader())
        {
            r = Convert.ToInt32(reader[0]);
        }

        return r;
    }

    public static void SetDatabaseActive(bool active)
    {
        if (active) Database.Open();
        else Database.Close();
    }
}
