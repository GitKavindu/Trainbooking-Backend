using BlazorComponents.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorComponents.Components.TrainComponent
{
    public partial class TrainComponent:ComponentBase
    {   
        private Action<int> _widthHandler;
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
                _widthHandler = OnScreenWidthChanged;
                await _deviceService.SubscribeToken(_widthHandler);

            }
        }
        
        public async Task Dispose()
        {
            _deviceService.UnsubscribeToken(_widthHandler);
        }

        private async void OnScreenWidthChanged(int width)
        {
            _screenWidth = width;
            //await service.print("Screen width is "+width);
            StateHasChanged();
        }
    }
}
    

    

