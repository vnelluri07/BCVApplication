using BeersCheersVasis.UI.ViewModels.UserManagement.Permissions;
using BeersCheersVasis.UI.ViewModels.UserManagement.Users;
using System.Collections.Immutable;

namespace BeersCheersAndVasis.UI.ViewModels.Script;

public record ScriptPageViewModel
{
    public ScriptViewModel? SelectedPermission => Scripts.FirstOrDefault(p => p.IsSelected);

    // can this be extracted into an ErrorViewModel?
    public ImmutableList<string> ErrorMessages { get; init; } = ImmutableList<string>.Empty;
    public ImmutableList<string> SuccessMessages { get; init; } = ImmutableList<string>.Empty;

    public ImmutableList<ScriptViewModel> Scripts { get; init; } = ImmutableList<ScriptViewModel>.Empty;

    public ScriptPageViewModel SetScrpts(List<ScriptViewModel> scripts)
    {
        return this with
        {
            Scripts = scripts.ToImmutableList()
        };
    }


}
