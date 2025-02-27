using BeersCheersAndVasis.UI.ViewModels.Script;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Models.Script;
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
            var scripts = await _api.ScriptApi.ListAsync();

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

    public async Task<string> CreateScript(CreateScriptRequest request)
    {
        var response = await _api.ScriptApi.CreateAsync(request);
        return response;
    }



    //TODO: will need this eventually, of course
    //public async Task<string> UpdateScript(UpdateScriptRequest request)
    //{
    //    var response = await _api.ScriptApi.UpdateAsync(request);
    //    return response;
    //}
}
