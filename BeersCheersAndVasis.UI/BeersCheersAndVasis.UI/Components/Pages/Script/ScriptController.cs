using BeersCheersAndVasis.UI.ViewModels.Script;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.UI.ViewModels.UserManagement.Users;

namespace BeersCheersAndVasis.UI.Components.Pages.Script;

public class ScriptController
{
    private readonly IApiClient _api;

    public ScriptController(IApiClient api)
    {
        _api = api;
    }

    public async Task<ScriptPageViewModel> OnInitializeAsync()
    {
        var model = new ScriptPageViewModel();
        model = await RefreshScripts(model);
        return model;
    }

    public async Task<ScriptPageViewModel> RefreshScripts(ScriptPageViewModel model)
    {
        try
        {
            var scripts = await _api.ScriptApi.ListScriptsAsync();

            return model.SetScrpts(scripts.Select(s => new ScriptViewModel(
                s.Id,
                s.Title,
                s.Content,
                s.IsActive
            )).ToList());
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
