using Core.DisplayManagement.Entities;
using Core.DisplayManagement.Handlers;
using Core.DisplayManagement.Views;
using Core.Environment.Shell;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Summary.Bale
{
    public class BaleSettings
    {
        public string Token { get; set; }
        public string ApiAccessKey { get; set; }
        public long Bot_Id { get; set; }
    }

    public class BaleSettingsDisplayDriver : SectionDisplayDriver<ISite,
        BaleSettings>
    {
        private readonly IShellHost _host;
        private readonly ShellSettings _shell;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IAuthorizationService _authorize;

        public BaleSettingsDisplayDriver(IShellHost host,
            ShellSettings settings,
            IHttpContextAccessor httpContext,
            IAuthorizationService authorize)
        {
            _host = host;
            _shell = settings;
            _httpAccessor = httpContext;
            _authorize = authorize;
        }

        public override async Task<IDisplayResult> EditAsync(BaleSettings settings,
            BuildEditorContext context)
        {
            var user = _httpAccessor.HttpContext?.User;
            if (user is null || !await _authorize.AuthorizeAsync(user, Permissions.ManageBaleSettings))
            {
                return null;
            }

            var init = Initialize<BaleSettings>("BaleSettings_Edit", model =>
            {
                model.ApiAccessKey = settings.ApiAccessKey;
                model.Bot_Id = settings.Bot_Id;
                model.Token = settings.Token;
            });

            return init.Location("Content:5").OnGroup("Bale");
        }

        public override async Task<IDisplayResult> UpdateAsync(BaleSettings settings,
            BuildEditorContext context)
        {
            var user = _httpAccessor.HttpContext?.User;
            if (user is null || !await _authorize.AuthorizeAsync(user, Permissions.ManageBaleSettings))
            {
                return null;
            }
            if (context.GroupId == "Bale")
            {
                await context.Updater.TryUpdateModelAsync(settings, Prefix);
                await _host.ReloadShellContextAsync(_shell);
            }
            return await EditAsync(settings, context);
        }
    }

    public class BaleSettingsConfiguration : IConfigureOptions<BaleSettings>
    {
        private readonly ISiteService _site;

        public BaleSettingsConfiguration(ISiteService site)
        {
            _site = site;
        }

        public void Configure(BaleSettings options)
        {
            var settings = _site.GetSiteSettingsAsync().GetAwaiter().GetResult().As<BaleSettings>();
            options.ApiAccessKey = settings.ApiAccessKey;
            options.Bot_Id = settings.Bot_Id;
            options.Token = settings.Token;
        }
    }
}