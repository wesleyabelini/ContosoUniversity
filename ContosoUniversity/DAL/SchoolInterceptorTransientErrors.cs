using ContosoUniversity.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace ContosoUniversity.DAL
{
    public class SchoolInterceptorTransientErrors: DbCommandInterceptor
    {
        private int counter = 0;
        private ILogger logger = new Logger();

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            bool throwTransientErrors = false;

            if(command.Parameters.Count > 0 && command.Parameters[0].Value.ToString() == "Throw")
            {
                throwTransientErrors = true;
                command.Parameters[0].Value = "an";
                command.Parameters[1].Value = "an";
            }

            if(throwTransientErrors && counter < 4)
            {
                logger.Information("Returning transient error for command: {0}",
                    command.CommandText);
                counter++;
                interceptionContext.Exception = CreateDummySqlException();
            }
        }

        private SqlException CreateDummySqlException()
        {
            /*
             * The instance of SQL Server you attempted to connect to does not 
             * support encryption*/

            var sqlErrorNumber = 20;

            var sqlErrorCtor = typeof(SqlError).GetConstructors(BindingFlags.Instance |
                BindingFlags.NonPublic).Where(c => c.GetParameters().Count() == 7).Single();

            var sqlError = sqlErrorCtor.Invoke(new object[]
            {
                sqlErrorNumber, (byte)0, (byte)0, "", "", "", 1
            });

            var erroCollection = Activator.CreateInstance(typeof(SqlErrorCollection), true);
            var AddMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.Instance |
                BindingFlags.NonPublic);
            AddMethod.Invoke(erroCollection, new[] { sqlError });

            var sqlExceptionCtor = typeof(SqlException).GetConstructors(BindingFlags.Instance |
                BindingFlags.NonPublic).Where(c => c.GetParameters().Count() == 4).Single();

            var sqlException = (SqlException)sqlExceptionCtor.Invoke(new object[] { "Dummy",
            erroCollection, null, Guid.NewGuid()});

            return sqlException;
        }
    }
}