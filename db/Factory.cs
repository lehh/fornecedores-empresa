using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace FornecedoresEmpresa.db
{
    public class Factory
    {
        private readonly string connectionString;

        public Factory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ISessionFactory GetSessionFactory()
        {
            var sessionFactory = Fluently.Configure()
                                 .Database(SQLiteConfiguration.Standard.ConnectionString(this.connectionString))
                                 .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
                                 .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                                 .BuildSessionFactory();

            return sessionFactory;
        }
    }
}
