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

    public Ixu _x;

    public TokenService(IJSRuntime JS,Ixu x)
    {
        _JS = JS;
        _x = x;
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
        Console.WriteLine("token changed ");
        OnChange?.Invoke();
    }

    private async Task EnsureJsSubscriptionAsync()
    {
        if (_module != null || _dotNetRef!=null)
            return;

        _module = await _JS.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorComponents/app.js");

        _dotNetRef = DotNetObjectReference.Create(this);

        await _module.InvokeVoidAsync("subscribe", _dotNetRef);
    }

    public async Task SubscribeToken(Action handler)
    {
        await EnsureJsSubscriptionAsync();
        OnChange += handler;
    }

    public void UnsubscribeToken(Action handler)
    {
        Console.WriteLine("delete now ");
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

    public Ixu ReturnXu()
    {
        return _x;
    }
    
}

public interface ITokenService
{
    Task<ReturnTokenDto?> ReturnToken();
    Task<bool?> GetIsUserAdmin();
    Task<ReturnTokenDto> SetToken(ReturnTokenDto token);
    Task<string> GetFromLocalStorage(string key);
    Task RemoveTokenAsync();
    Task SubscribeToken(Action handler);
    void UnsubscribeToken(Action handler);
    Ixu ReturnXu();
}

public interface Ixu
{
    bool GetState();
}

public class Xu:Ixu
{
    private bool _State;

    public Xu(bool State)
    {
        this._State = State;
    }

    public bool GetState()
    {
        return this._State;
    }
}
