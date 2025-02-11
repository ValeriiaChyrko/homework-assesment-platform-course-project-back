using HomeworkAssignment.Services;
using HomeworkAssignment.Services.Abstractions;
using RepoAnalisys.Grpc;

namespace HomeworkAssignment
{
    public static class DependencyInjection
    {
        public static void AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountGrpcService, AccountGrpcService>();
            services.AddScoped<ICompilationGrpcService, CompilationGrpcService>();
            services.AddScoped<IQualityGrpcService, QualityGrpcService>();
            services.AddScoped<ITestsGrpcService, TestsGrpcService>();

            var grpcBaseUrl = configuration
                .GetRequiredSection("GrpcSettings")
                .GetValue<string>("BaseUrl");

            services.AddGrpcClient<AccountsOperator.AccountsOperatorClient>(o =>
            {
                o.Address = new Uri(grpcBaseUrl!); 
            });
            services.AddGrpcClient<CompilationOperator.CompilationOperatorClient>(o =>
            {
                o.Address = new Uri(grpcBaseUrl!); 
            });
            services.AddGrpcClient<QualityOperator.QualityOperatorClient>(o =>
            {
                o.Address = new Uri(grpcBaseUrl!); 
            });
            services.AddGrpcClient<TestsOperator.TestsOperatorClient>(o =>
            {
                o.Address = new Uri(grpcBaseUrl!); 
            });
        }
    }
}