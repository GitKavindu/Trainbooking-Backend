using Microsoft.JSInterop;
namespace BlazorComponents.Service
{
    public class DeviceService
    {
        private IJSObjectReference? _module;
        public event Action<int>? WidthChanged;

        private IJSRuntime _JS;

        public DeviceService(IJSRuntime JS)
        {
            _JS = JS;
        }

        [JSInvokable]
        public void OnScreenWidthChanged(int width)
        {
            try
            {
                WidthChanged?.Invoke(width);    
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
                
        }

        public async Task ListenOnScreenChanges(DotNetObjectReference<DeviceService> dotNetRef)
        {
            // Import JS module
             if (_module == null)
            {
                _module = await _JS.InvokeAsync<IJSObjectReference>(
                    "import", "./_content/BlazorComponents/CallDotnetInstance.js");
            }
            
            // Start JS listener, passing the object reference
            await _module.InvokeVoidAsync("startResizeListener", dotNetRef);
        }

        public async Task DisposeModuleAsync()
        {
            if (_module != null)
            {
                await  _module.DisposeAsync();
            }
        }
    }
}