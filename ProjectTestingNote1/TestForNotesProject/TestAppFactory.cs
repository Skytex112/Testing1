using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc.Testing;

using Microsoft.Data.Sqlite;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

using ProjectTestingNote.Data;

using System;

using System.Collections.Generic;

using System.Data.Common;

using System.Text;

namespace NoteTestApi

{

    public class TestAppFactory : WebApplicationFactory<Program>

    {

        private DbConnection? _dbConnection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)

        {

            builder.ConfigureServices(services =>

            {

                var descriptors = services.Where(d =>

                    d.ServiceType.Name.Contains("DbContextOptions") ||

                    d.ServiceType == typeof(DataBaseContext)).ToList();

                foreach (var descriptor in descriptors)

                {

                    services.Remove(descriptor);

                }



                _dbConnection = new SqliteConnection("DataSource=:memory:");

                _dbConnection.Open();



                services.AddDbContext<DataBaseContext>(options =>

                {

                    options.UseSqlite(_dbConnection);

                });



                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();



                db.Database.EnsureCreated();

            });

            base.ConfigureWebHost(builder);

        }


        protected override void Dispose(bool disposing)

        {

            if (disposing)

            {

                _dbConnection?.Dispose();

            }

            base.Dispose(disposing);

        }

    }

}

