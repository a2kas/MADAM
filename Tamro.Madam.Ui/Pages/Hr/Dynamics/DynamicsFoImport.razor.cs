using BlazorDownloadFile;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Tamro.Madam.Application.Commands.Hr.Dynamics;

namespace Tamro.Madam.Ui.Pages.Hr.Dynamics;

public partial class DynamicsFoImport
{
    private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string _dragClass = DefaultDragClass;
    private IReadOnlyList<IBrowserFile> _files = new List<IBrowserFile>();
    private MudFileUpload<IReadOnlyList<IBrowserFile>>? _fileUpload;

    #region IoC
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    #endregion

    private Task OpenFilePickerAsync()
        => _fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

    private void OnInputFilesChanged(InputFileChangeEventArgs e)
    {
        ClearDragClass();
        var files = e.GetMultipleFiles(200);
        if (!files.Any())
        {
            return;
        }
        if (files.All(x => x.Name.EndsWith(".csv")))
        {
            _files = files;
        }
        else
        {
            Snackbar.Add($"Invalid file format: {files[0].Name}. Only .csv are allowed", Severity.Error);
        }
    }

    private async Task Upload()
    {
        var byteArrays = new List<byte[]>();
        foreach (var file in _files)
        {
            await using var stream = file.OpenReadStream();
            await using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byteArrays.Add(memoryStream.ToArray());
        }

        var result = await _mediator.Send(new TransformVikarinaToDynamicsCommand(byteArrays));
        if (result.Succeeded)
        {
            string extension = byteArrays.Count > 1 ? "zip" : "csv";
            await _blazorDownloadFileService.DownloadFile($"DynamicsImport.{extension}", result.Data, "application/octet-stream");
            Snackbar.Add("Dynamics FO import generated successfully", Severity.Success);
        }
        else
        {
            Snackbar.Add("Failed to generate Dynamics FO import", Severity.Error);
        }
    }

    private void SetDragClass()
        => _dragClass = $"{DefaultDragClass} mud-border-primary";

    private void ClearDragClass()
        => _dragClass = DefaultDragClass;
}
