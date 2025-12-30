using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models.Dtos;
using System.Text.Json;

namespace BlazorComponents.Service;
public class TokenService:ITokenService, IAsyncDisposable
{
    private IJSRuntime _JS;
    private IJSObjectReference? _module;
    private DotNetObjectReference<TokenService>? _dotNetRef;

    private event Action? OnChange;

    public TokenService(IJSRuntime JS)
    {
        _JS = JS;
    }
    
    public async Task<ReturnTokenDto?> ReturnToken()
    {
        ReturnTokenDto userToken = null;

        string tokenString=await this.GetFromLocalStorage("userToken");
        if(tokenString!="")
        {
            if(this.ValidateToken(tokenString))
            {
                userToken = JsonSerializer.Deserialize<ReturnTokenDto>(tokenString);
                return userToken;
            }
            else
            {
                await this.RemoveTokenAsync();
            }

            return userToken;
        }

        return null;
    }

    public async Task<bool?> GetIsUserAdmin()
    {
       return (await ReturnToken())?.isAdmin;
    }

    public async Task<ReturnTokenDto> SetToken(ReturnTokenDto token)
    {
        await EnsureJsSubscriptionAsync();
        await _JS.InvokeVoidAsync(
            "localStorage.setItem",
            "userToken",
            JsonSerializer.Serialize(token)
        );
        
        await _module!.InvokeVoidAsync("notify");
        return token;
    }


    public async Task<string> GetFromLocalStorage(string key)
    {
        return await _JS.InvokeAsync<string?>("localStorage.getItem", key) ?? string.Empty;
    }

    private bool ValidateToken(string tokenString)
    {
        ReturnTokenDto userToken = JsonSerializer.Deserialize<ReturnTokenDto>(tokenString);

        if (userToken == null)
            return false;

        DateTime utcNow = DateTime.UtcNow;
        DateTime tokenDate = userToken.endTime;

        return utcNow <= tokenDate && userToken.isActive;
    }

    public async Task RemoveTokenAsync()
    {
        await EnsureJsSubscriptionAsync();
        await _JS.InvokeVoidAsync("localStorage.removeItem", "userToken");
        await _module!.InvokeVoidAsync("notify");
    }

    public Action? GetOnChange()
    {
        return OnChange;
    }

    [JSInvokable]
    public void OnTokenChanged()
    {
        OnChange?.Invoke();
    }

    private async Task EnsureJsSubscriptionAsync()
    {
        if (_module != null)
            return;

        _module = await _JS.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorComponents/app.js");

        _dotNetRef = DotNetObjectReference.Create(this);

        await _module.InvokeVoidAsync("subscribe", _dotNetRef);
    }

    public void SubscribeToken(Action handler)
    {
        OnChange += handler;
    }

    public void UnsubscribeToken(Action handler)
    {
        OnChange -= handler;
    }

    public async ValueTask DisposeAsync()
    {
        if (_module != null && _dotNetRef != null)
        {
            await _module.InvokeVoidAsync("unsubscribe", _dotNetRef);
            await _module.DisposeAsync();
        }

        _dotNetRef?.Dispose();
    }
    
}

public interface ITokenService
{
    Task<ReturnTokenDto?> ReturnToken();
    Task<bool?> GetIsUserAdmin();
    Task<ReturnTokenDto> SetToken(ReturnTokenDto token);
    Task<string> GetFromLocalStorage(string key);
    Task RemoveTokenAsync();
    void SubscribeToken(Action handler);
    void UnsubscribeToken(Action handler);
}
