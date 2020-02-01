using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace FornecedoresEmpresa.db
{
    public class Factory
    {
        private string connectionString;

        public Factory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ISessionFactory GetSessionFactory()
        {
            var sessionFactory = Fluently.Configure()
                                 .Database(SQLiteConfiguration.Standard.ConnectionString(this.connectionString))
                                 .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
                                 .BuildSessionFactory();

            return sessionFactory;
        }
    }
}
