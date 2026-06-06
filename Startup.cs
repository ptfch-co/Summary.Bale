namespace Summary.Bale
{
    using Core.DisplayManagement.Handlers;
    using Core.Modules;
    using Core.Navigation;
    using Core.Security.Permissions;
    using Core.Settings;
    using Core.Workflows.Helpers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Summary.Bale.Services;
    using Summary.Bale.Workflows.Task.Message.Send;

    [Feature(Bale.Feature.Bale)]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, Menu>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<IDisplayDriver<ISite>, BaleSettingsDisplayDriver>();

            services.AddTransient<IConfigureOptions<BaleSettings>, BaleSettingsConfiguration>();

            services.AddActivity<SendMessageInBaleTask, SendMessageInBaleTaskDisplay>();
        }
    }
}