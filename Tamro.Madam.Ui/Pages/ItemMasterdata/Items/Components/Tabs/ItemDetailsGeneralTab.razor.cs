using AutoMapper;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Queries.Atcs.Clsf;
using Tamro.Madam.Application.Queries.Brands.Clsf;
using Tamro.Madam.Application.Queries.Forms.Clsf;
using Tamro.Madam.Application.Queries.MeasurementUnits.Clsf;
using Tamro.Madam.Application.Queries.Nicks.Clsf;
using Tamro.Madam.Application.Queries.Producers.Clsf;
using Tamro.Madam.Application.Queries.Requestors.Clsf;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Brands.Components;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Nicks.Components;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Producers.Components;
using Tamro.Madam.Ui.Pages.ItemMasterdata.Requestors.Components;
using Tamro.Madam.Ui.Store.Actions.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components.Tabs;

public partial class ItemDetailsGeneralTab
{
    [Inject]
    private IState<ItemState> _itemState { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IActionSubscriber _actionSubscriber { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IMapper _mapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _actionSubscriber?.SubscribeToAction<SetCurrentItemAction>(this, _ =>
        {
            StateHasChanged();
        });

        await base.OnInitializedAsync();
    }

    private async Task<IEnumerable<BrandClsfModel>> SearchBrands(string value, CancellationToken token)
    {
        try
        {
            var query = new BrandClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load brands", Severity.Error);
            return new List<BrandClsfModel>();
        }
    }

    private async Task<IEnumerable<ProducerClsfModel>> SearchProducers(string value, CancellationToken token)
    {
        try
        {
            var query = new ProducerClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load producers", Severity.Error);
            return new List<ProducerClsfModel>();
        }
    }

    private async Task<IEnumerable<NickClsfModel>> SearchNicks(string value, CancellationToken token)
    {
        try
        {
            var query = new NickClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load nicks", Severity.Error);
            return new List<NickClsfModel>();
        }
    }

    private async Task<IEnumerable<FormClsfModel>> SearchForms(string value, CancellationToken token)
    {
        try
        {
            var query = new FormClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load forms", Severity.Error);
            return new List<FormClsfModel>();
        }
    }

    private async Task<IEnumerable<AtcClsfModel>> SearchAtcs(string value, CancellationToken token)
    {
        try
        {
            var query = new AtcClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load ATCs", Severity.Error);
            return new List<AtcClsfModel>();
        }
    }

    private async Task<IEnumerable<MeasurementUnitClsfModel>> SearchMeasurementUnits(string value, CancellationToken token)
    {
        try
        {
            var query = new MeasurementUnitClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load measurement units", Severity.Error);
            return new List<MeasurementUnitClsfModel>();
        }
    }

    private void OnAtcChanged(AtcClsfModel atcChanged)
    {
        _itemState.Value.Item.Atc = atcChanged;
        _itemState.Value.Item.ActiveSubstance = atcChanged != null ? atcChanged.Name : null;
    }

    private async Task<IEnumerable<RequestorClsfModel>> SearchRequestors(string value, CancellationToken token)
    {
        try
        {
            var query = new RequestorClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load requestors", Severity.Error);
            return new List<RequestorClsfModel>();
        }
    }

    private async Task CreateBrand()
    {
        var parameters = new DialogParameters<BrandDetailsDialog>
        {
            { x => x.Model, new BrandModel() },
            { x => x.State, DialogState.Create },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<BrandDetailsDialog>(string.Empty, parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult.Data is BrandModel result)
        {
            _itemState.Value.Item.Brand = _mapper.Map<BrandClsfModel>(result);
        }
    }

    private async Task CreateNick()
    {
        var parameters = new DialogParameters<NickDetailsDialog>
        {
            { x => x.Model, new NickModel() },
            { x => x.State, DialogState.Create },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<NickDetailsDialog>(string.Empty, parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult.Data is NickModel result)
        {
            _itemState.Value.Item.SupplierNick = _mapper.Map<NickClsfModel>(result);
        }
    }

    private async Task CreateProducer()
    {
        var parameters = new DialogParameters<ProducerDetailsDialog>
        {
            { x => x.Model, new ProducerModel() },
            { x => x.State, DialogState.Create },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<ProducerDetailsDialog>(string.Empty, parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult.Data is ProducerModel result)
        {
            _itemState.Value.Item.Producer = _mapper.Map<ProducerClsfModel>(result);
        }
    }
    private async Task CreateRequestor()
    {
        var parameters = new DialogParameters<RequestorDetailsDialog>
        {
            { x => x.Model, new RequestorModel() },
            { x => x.State, DialogState.Create },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = true,
        };
        var dialog = DialogService.Show<RequestorDetailsDialog>(string.Empty, parameters, options);
        var dialogResult = await dialog.Result;

        if (dialogResult.Data is RequestorModel result)
        {
            _itemState.Value.Item.Requestor = _mapper.Map<RequestorClsfModel>(result);
        }
    }
}