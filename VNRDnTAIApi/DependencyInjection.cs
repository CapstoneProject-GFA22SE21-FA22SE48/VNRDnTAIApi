using BusinessObjectLibrary;
using DataAccessLibrary.Implementations;
using DataAccessLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VNRDnTAIApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            #region DbContext
            services.AddDbContext<vnrdntaiContext>(options =>
            {
                options.UseSqlServer(VNRDnTAILibrary.VNRDnTAIConfiguration.ConnectionString);
            });
            #endregion

            #region Repository
            services.AddScoped<IGenericRepository<Answer>, GenericRepository<Answer>>();
            services.AddScoped<IGenericRepository<Column>, GenericRepository<Column>>();
            services.AddScoped<IGenericRepository<Decree>, GenericRepository<Decree>>();
            services.AddScoped<IGenericRepository<Gpssign>, GenericRepository<Gpssign>>();
            services.AddScoped<IGenericRepository<Keyword>, GenericRepository<Keyword>>();
            services.AddScoped<IGenericRepository<Paragraph>, GenericRepository<Paragraph>>();
            services.AddScoped<IGenericRepository<ParagraphModificationRequest>, GenericRepository<ParagraphModificationRequest>>();
            services.AddScoped<IGenericRepository<Question>, GenericRepository<Question>>();
            services.AddScoped<IGenericRepository<QuestionModificationRequest>, GenericRepository<QuestionModificationRequest>>();
            services.AddScoped<IGenericRepository<Reference>, GenericRepository<Reference>>();
            services.AddScoped<IGenericRepository<Section>, GenericRepository<Section>>();
            services.AddScoped<IGenericRepository<Sign>, GenericRepository<Sign>>();
            services.AddScoped<IGenericRepository<SignCategory>, GenericRepository<SignCategory>>();
            services.AddScoped<IGenericRepository<SignModificationRequest>, GenericRepository<SignModificationRequest>>();
            services.AddScoped<IGenericRepository<SignParagraph>, GenericRepository<SignParagraph>>();
            services.AddScoped<IGenericRepository<Statue>, GenericRepository<Statue>>();
            services.AddScoped<IGenericRepository<TestCategory>, GenericRepository<TestCategory>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<UserModificationRequest>, GenericRepository<UserModificationRequest>>();
            services.AddScoped<IGenericRepository<VehicleCategory>, GenericRepository<VehicleCategory>>();
            #endregion

            #region UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            return services;
        }

    }
}
