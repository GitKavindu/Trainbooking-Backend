using System.Threading.Tasks;
using Microsoft.JSInterop;
namespace BlazorComponents.Service
{
    public class DeviceService:IAsyncDisposable
    {
        private IJSObjectReference? _module;
        private event Action<int>? WidthChanged;
        private DotNetObjectReference<DeviceService> _dotNetRef;

        private IJSRuntime _JS;
        private int _width;

        public DeviceService(IJSRuntime JS)
        {
            _JS = JS;
        }

        [JSInvokable]
        public void OnScreenWidthChanged(int width)
        {
            try
            {
                _width = width;
                WidthChanged?.Invoke(width);    
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
                
        }

        public async Task ListenOnScreenChanges()
        {
            // Import JS module
            if (_module != null)
            {
                return;
            }

            _dotNetRef = DotNetObjectReference.Create(this);

            _module = await _JS.InvokeAsync<IJSObjectReference>(
                    "import", "./_content/BlazorComponents/CallDotnetInstance.js");
            
            // Start JS listener, passing the object reference
            await _module.InvokeVoidAsync("startResizeListener", _dotNetRef);
        }

        public async Task SubscribeToken(Action<int> handler)
        {
            WidthChanged += handler;
            await this.ListenOnScreenChanges();
            WidthChanged?.Invoke(_width);
        }

        public void UnsubscribeToken(Action<int> handler)
        {
            WidthChanged -= handler;
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
            {
                await  _module.DisposeAsync();
            }
        }
    }
}