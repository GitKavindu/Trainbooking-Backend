using BlazorComponents.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorComponents.Components.TrainComponent
{
    public partial class TrainComponent:ComponentBase
    {
        private DotNetObjectReference<DeviceService>? _ref;
        private int _screenWidth;

        private DeviceService _deviceService;

        private SharedService service;

        public TrainComponent(SharedService sharedService,DeviceService deviceService)
        {
            _deviceService = deviceService;
            service = sharedService;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //Subscribe First
                _deviceService.WidthChanged += OnScreenWidthChanged;

                _ref = DotNetObjectReference.Create(_deviceService);
                await _deviceService.ListenOnScreenChanges(_ref);

            }
        }
        
        public async Task Dispose()
        {
            _deviceService.WidthChanged -= OnScreenWidthChanged;
            _ref?.Dispose();
            await _deviceService.DisposeModuleAsync();
        }

        private async void OnScreenWidthChanged(int width)
        {
            _screenWidth = width;
            //await service.print("Screen width is "+width);
            StateHasChanged();
        }
    }
}
    

    

