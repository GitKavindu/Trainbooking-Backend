using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorComponents.Service;

public class SharedService:ISharedService
{
    public ITokenService _tokenService;
    private IJSRuntime _JS;

    private NavigationManager _navigation { get; set; }
    public string currentUrl="";

    public SharedService(ITokenService tokenService,IJSRuntime JS,NavigationManager navigation)
    {
        this._tokenService = tokenService;
         _JS = JS;
        _navigation = navigation;

        currentUrl = new Uri(_navigation.Uri).AbsolutePath;
        if (string.IsNullOrEmpty(currentUrl))
        {
            currentUrl = "/";
        }

    }

    public bool showMessage = false;

    public ITokenService GetTokenService()
    {
        return _tokenService;
    }

    public async Task<bool> GetAlertBox(string messege)
    {
       return await _JS.InvokeAsync<bool>("confirm", messege);
    }

    public async Task print(string messege)
    {
         await _JS.InvokeVoidAsync("console.log", messege);
    }
}

public interface ISharedService
{
    ITokenService GetTokenService();
    Task<bool> GetAlertBox(string messege);

    Task print(string messege);
}