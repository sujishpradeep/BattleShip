﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShipApi.Persistence;
using BattleShipApi.DataProcessing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BattleShipApi.Managers;
using BattleShipApi.Controllers;
using BattleShipApi.Strategies;

namespace BattleShipApi
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

            services.AddMemoryCache();

            services.AddScoped<IBoardManager, BoardManager>();
            services.AddScoped<IBattleShipManager, BattleShipManager>();

            services.AddScoped<IBattleShipAllignmentStrategy, HorizontalAllignmentStrategy>();
            services.AddScoped<IBattleShipAllignmentStrategy, VerticalAllignmentStrategy>();


            services.AddScoped<ICacheProvider, CacheProvider>();
            services.AddScoped<IBoardDataProcessing, BoardDataProcessing>();
            services.AddScoped<BoardStateCache>();

            services.AddAuthorization();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
