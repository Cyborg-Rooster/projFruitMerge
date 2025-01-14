using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DatabaseSynchronizer
{
    private static readonly int Version = 1;
    public static void Synch()
    {
        #region "Create"
        SQLiteManager.RunQuery
        (
            CommonQuery.Create
            (
                "DATABASE", 
                "DATABASE_VERSION INTEGER"
            )
        );

        SQLiteManager.RunQuery
        (
            CommonQuery.Create
            (
                "PLAYER",
                "BEST_SCORE INTEGER, " +
                "SOUNDS INTEGER, " +
                "MUSICS INTEGER, " +
                "LANGUAGE INTEGER"
            )
        );

        #endregion

        #region "Add"
        SQLiteManager.RunQuery
        (
            CommonQuery.Add
            (
                "DATABASE",
                "DATABASE_VERSION",
                Version.ToString()
            )
        );
        SQLiteManager.RunQuery
        (
            CommonQuery.Add
            (
                "PLAYER",
                "BEST_SCORE, SOUNDS, MUSICS, LANGUAGE",
                $"0, 1, 1, 0"
            )
        );
        #endregion
    }
}
