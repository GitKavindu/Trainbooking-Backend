using Microsoft.AspNetCore.Components;
using Interfaces;
using Models;
using Models.Dtos;
using BlazorComponents.Service;
using System.Threading.Tasks;

namespace BlazorComponents.Components.StationComponent.AddEditStationComponent
{
    public partial class AddEditStationComponent : ComponentBase
    {
        private int? Stationid;
        private string? Stationname;

        [Parameter]
        public ReturnStationDto ?Station { get; set; }

        [Parameter]
        public EventCallback<ReturnStationDto?> StationChanged { get; set; }

        private SharedService service ;
        private IStationService _stationService;
        public AddEditStationComponent(SharedService sharedService,IStationService stationService)
        {
            service = sharedService;
            _stationService = stationService;
            this.Stationid = 0;
            
        }

        protected override void OnInitialized()
        {
            this.Stationid = this.Station.station_id;
            this.Stationname = this.Station.station_name;
                        
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await service.print("Hiii!!!");
            }
            else
            {
                
            }
        }

        private async Task addStation()
        {
            bool confirmDelete =await service.GetAlertBox("Are you sure ?");
            await service.print("station name ! "+this.Stationid);

            if(confirmDelete)
            {                   
                AddStationDto stationDto = new AddStationDto()
                {
                    station_id = "",
                    station_name = this.Stationname,
                    token_id = (await service._tokenService.ReturnToken())?.tokenId
                };
                
                try
                {
                    

                     ResponseModel res =await _stationService.AddStation(stationDto);
                    if(res.Success)
                    {
                        await service.GetAlertBox(res.Data.ToString());
                    
                    }
                    else
                    {
                        await service.print(res.ErrCode.ToString());
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

               

               

            }
        }

        private async Task editStation()
        {
            bool confirmDelete =await service.GetAlertBox("Are you sure ?");

            if(confirmDelete)
            {
                AddStationDto stationDto = new AddStationDto()
                {
                    station_id = this.Stationid.ToString(),
                    station_name = this.Stationname,
                    token_id = (await service._tokenService.ReturnToken())?.tokenId
                };

                ResponseModel res =await _stationService.UpdateStation(stationDto);
                if(res.Success)
                {
                    await service.GetAlertBox(res.Data.ToString());
                   
                }
                else
                {
                    await service.print(res.ErrCode.ToString());
                }

            }
        }
    }
}