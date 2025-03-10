﻿using BeersCheersAndVasis.UI.Components.Pages.Script;
using BeersCheersAndVasis.UI.ViewModels.Script;
using Microsoft.AspNetCore.Components;

namespace BlazorApp3.Pages.Script
{
    public partial class Script
    {
        private ScriptPageViewModel _model { get; set; } = new();
        [Inject] private ScriptController Controller { get; set; } = default!;

        //[Inject] private IDialogService DialogService { get; set; } = default!;
        //[Inject] private ISnackbar Snackbar { get; set; } = default!;
        //[CascadingParameter] MudDialogInstance MudDialog { get; set; } = new();
        [Inject] private NavigationManager _navManager{get; set;} = default!;
        bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            await InitPage();
        }

        private async Task InitPage()
        {
            if (Controller == null)
            {
                throw new NullReferenceException(nameof(ScriptController));
            }

            isLoading = true;
            _model = await Controller.OnInitializeAsync();
            isLoading = false;

            //DisplayMessages();

            await InvokeAsync(StateHasChanged);
        }

        private async Task ReadMore_OnClick(ScriptViewModel script) 
        {
            if (script is not null) 
            {
                _navManager.NavigateTo($"/readscript/{script.Id}");
            }
        }
    }
}
