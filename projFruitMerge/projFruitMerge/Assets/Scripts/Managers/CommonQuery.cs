using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CommonQuery
{
    public static string Add(string table, string columns, string values)
    {
        return $"INSERT INTO {table}({columns}) VALUES({values})";
    }

    public static string Select(string columns, string table)
    {
        return $"SELECT {columns} FROM {table}";
    }

    public static string Select(string columns, string table, string where)
    {
        return $"SELECT {columns} FROM {table} WHERE {where}";
    }

    public static string Update(string table, string changes, string where)
    {
        return $"UPDATE {table} SET {changes} WHERE {where}";
    }

    public static string Create(string table, string columns)
    {
        return $"CREATE TABLE {table} ({columns})";
    }
}
