using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace Servicing.Components.Pages
{
    public partial class Index
    {
        protected string ChangelogVersion { get; } = "v1.6.0";
        protected DateTime ChangelogUpdatedAt { get; } = new DateTime(2026, 3, 22);
        protected string ChangelogVersionLabel => $"Versão atual: {ChangelogVersion}";
        protected string ChangelogUpdatedLabel => $"Última atualização: {ChangelogUpdatedAt:dd/MM/yyyy}";

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
    }
}