﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Configurations
{
    public interface IConfigManager
    {
        void ConfigureServices(IServiceCollection services);
    }
}
