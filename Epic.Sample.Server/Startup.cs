using Epic.Sample.Application.Handlers;
using Epic.Sample.Application.Models;
using Epic.Sample.Application.Queries;
using Epic.Sample.Domain.Repository;
using Epic.Sample.Infrastructure;
using GraphiQl;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Epic.Sample.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "Epic Demo", Version = "v1"}); });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<ProductQuery>();
            services.AddSingleton<ISchema>(
                new ProductSchema(new FuncDependencyResolver(type =>
                    services.BuildServiceProvider().GetService(type))));
            services.AddSingleton<ProductModel>();
            services.AddMediatR(typeof(NewProductHandler).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseGraphiQl();    
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}