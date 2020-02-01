using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using FornecedoresEmpresa.Data.Mapeamento;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NHibernateExtension
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString) 
        {
            var sessionFactory = Fluently.Configure()
                                 .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                                 .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PessoaMap>())
                                 .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                                 .BuildSessionFactory();
            
            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());

            return services;
        }
    }
}
