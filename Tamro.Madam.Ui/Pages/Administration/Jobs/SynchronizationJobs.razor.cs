using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;
using Tamro.Madam.Application.Jobs;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Ui.Pages.Administration.Jobs;

public partial class SynchronizationJobs
{
    private IEnumerable<IOneTimeJob> Jobs = new List<IOneTimeJob>();

    [Inject]
    private IServiceProvider _serviceProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(IOneTimeJob));

        var jobTypes = assembly.GetTypes()
            .Where(t => t.IsClass && typeof(IOneTimeJob).IsAssignableFrom(t));

        var jobs = new List<IOneTimeJob>();
        foreach (var type in jobTypes)
        {
            IOneTimeJob jobInstance = (IOneTimeJob)Activator.CreateInstance(type);
            jobs.Add(jobInstance);
        }

        Jobs = jobs;
    }

    private async Task OnExecute(IOneTimeJob? job)
    {
        job.Processing = true;

        var service = (IOneTimeJob)_serviceProvider.GetRequiredService(job.GetType());
        
        var result = await service.Execute();

        if (result.Succeeded)
        {
            Snackbar.Add($"'{job.Name}' completed successfully", Severity.Success);
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }

        job.Processing = false;

    }
}
