using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Application.Handlers;
using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Application.Services.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Checkout.PaymentGateway.Domain.Repositories;
using Checkout.PaymentGateway.Infrastructure;
using Checkout.PaymentGateway.Infrastructure.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<PaymentGatewayDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Payments"));
            });

            ConfigureEncryption(services);
            ConfigureSwagger(services);

            services.Configure<BankingServiceOptions>(Configuration.GetSection("BankingServiceOptions"));

            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddHandlers();
            services.AddTransient<IMessageDispatcher, MessageDispatcher>();
            services.AddHttpClient<IBankingService, BankingService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["BankingServiceOptions:BaseAddress"]);
            });
            services.AddTransient<ICreatePaymentService, CreatePaymentService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureEncryption(IServiceCollection services)
        {
            var publicKey = Convert.FromBase64String(Configuration["RSA:PublicKey"]);
            var privateKey = Convert.FromBase64String(Configuration["RSA:PrivateKey"]);

            services.AddSingleton<RSA>(provider =>
            {
                var rsa = RSA.Create();
                rsa.ImportRSAPublicKey(publicKey, out _);
                rsa.ImportRSAPrivateKey(privateKey, out _);
                return rsa;
            });

            services.AddSingleton<IEncryptionService, EncryptionService>();
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen();
        }
    }
}
