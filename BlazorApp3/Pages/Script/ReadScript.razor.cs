using Microsoft.AspNetCore.Components;

namespace BlazorApp3.Pages.Script;

public partial class ReadScript
{
    [Parameter] public long Id { get; set; }


    protected async Task OnParameterSetAsync()
    {
        await InvokeAsync(() =>
        {

        });
    }

    protected async override Task OnInitializedAsync()
    {
        var id = Id;
    }
}


