using Microsoft.Extensions.DependencyInjection;
using OfflineTest.Services.Contracts;
using OfflineTest.Services.ContractsImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineTest
{
    public static class InitialSevices
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IBookBus, BookBus>();
        }
    }
}
