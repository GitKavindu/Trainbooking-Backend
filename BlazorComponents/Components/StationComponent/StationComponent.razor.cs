using Interfaces;
using BlazorComponents.Service;
using Microsoft.AspNetCore.Components;
using Models.Dtos;
using Models;
using Microsoft.JSInterop;


namespace BlazorComponents.Components.StationComponent
{
    public partial class StationComponent:ComponentBase
    {
        private List<ReturnStationDto>? stationList;
        private string? ModalTitle;

        private bool ActivateAddEditStationComp = false;
        private ReturnStationDto? Station;

        private string stationIdFilter = "";
        private string stationNameFilter = "";

        private List<ReturnStationDto>? stationListWithoutFilter;
        private DeviceService _deviceService;
        private Action<int> _widthHandler;
        private NavigationService<ReturnStationDto> _navigationService;

        private SharedService service ;
        private IStationService _stationService;

        private int _screenWidth;

        private bool asc=true; //represents ascending order
        private bool station_id = false;
        private bool station_name = false;

        private string selectedModel;
        private string _stationFilter = "";

        string StationFilter
        {
            get => _stationFilter;
            set
            {
                if (_stationFilter == value) 
                    return;

                _stationFilter = value;
                filterFn(); 
            }
        }



        public StationComponent(SharedService sharedService,IStationService stationService,DeviceService deviceService)
        {
            service = sharedService;
            _stationService = stationService;
            stationList = new List<ReturnStationDto>();
            stationListWithoutFilter=new List<ReturnStationDto>();

            _deviceService = deviceService;
            _navigationService =  new NavigationService<ReturnStationDto>(this.stationList);
            selectedModel = "id";
            stationIdFilter = "";
        }

        protected override async Task OnInitializedAsync()
        {
            await this.RefreshStationList();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //Subscribe First
                _widthHandler = OnScreenWidthChanged;
                await _deviceService.SubscribeToken(_widthHandler);

                //await this.RefreshStationList();
                
            }

            await service.print(firstRender.ToString());
        }

        public async Task Dispose()
        {
            _deviceService.UnsubscribeToken(_widthHandler);
          
        }

        private async void OnScreenWidthChanged(int width)
        {
            _screenWidth = width;
            await service.print("Screen width is "+width);
            StateHasChanged();
        }

        private void addClick() 
        {
            this.Station = new ReturnStationDto();
            this.ModalTitle = "Add station";
            this.ActivateAddEditStationComp = true;
        }

        private void editClick( ReturnStationDto item)  
        {
            this.Station = item;
            this.ModalTitle = "Edit station";
            this.ActivateAddEditStationComp = true;
        }

        private async Task deleteClick(int ?StationId) 
        {
            bool confirmDelete =await service.GetAlertBox("Are you sure ?");
            if(confirmDelete)
            {
                AddStationDto stationDto = new AddStationDto()
                {
                    station_id = StationId?.ToString(),
                    station_name = string.Empty,
                    token_id = (await service._tokenService.ReturnToken())?.tokenId
                };

                ResponseModel res =await _stationService.DeleteStation(stationDto);
                if(res.Success)
                {
                    await service.GetAlertBox(res.Data.ToString());
                    await this.RefreshStationList();
                }
                else
                {
                    await service.print(res.ErrCode.ToString());
                }

            }
        }
        
        private async Task closeClick()
        {
            this.ActivateAddEditStationComp = false;
            await this.RefreshStationList();
        }

        private async Task RefreshStationList()
        {
            this.stationList = new List<ReturnStationDto>();
            this._navigationService = new NavigationService<ReturnStationDto>(this.stationList);
            this.stationListWithoutFilter=new List<ReturnStationDto>();

            ResponseModel res =await _stationService.GetStations();

            if(res.ErrCode==200)
            {
                IEnumerable<ReturnStationDto> returnStationDto = (IEnumerable<ReturnStationDto>) res.Data;
                foreach (var station in returnStationDto)
                {
                    this.stationList.Add(station);
                    this.stationListWithoutFilter.Add(station);
                }
            }
            else
            {
                await service.print(res?.Data.ToString());
            }
        }

        private string GetStationId(int ?stationNum)
        {
            return "ST" + stationNum?.ToString("D6");
        }

        private void SortResult(string prop, bool asc)
        {
            var propertyInfo = typeof(ReturnStationDto).GetProperty(prop);

            if (propertyInfo != null)
            {
                if (asc)
                {
                    stationList = stationList.OrderBy(station => propertyInfo.GetValue(station)).ToList();
                }
                else
                {
                    stationList = stationList.OrderByDescending(station => propertyInfo.GetValue(station)).ToList();
                }
            }
            else
            {   
                Console.WriteLine($"Property {prop} does not exist.");
            }
            
        }

        private void toggleMoreDetails(int rowNo) {
            
            rowNo = this._navigationService.getRealRowNum(rowNo);            
            this.stationList[rowNo].showRow = !this.stationList[rowNo].showRow;
        }

        private List<ReturnStationDto> getVisibleRows(){
            List<ReturnStationDto> visibleRows=this._navigationService.GetVisibleRows();
            return visibleRows;
        }

        private void pageForward(){
            this._navigationService.PageForward();
        }

        private void pageBackward(){
            this._navigationService.PageBackward();
        }
        
        private void sortResult(string prop) 
        {
            this.stationList = new List<ReturnStationDto>();
            this._navigationService = new NavigationService<ReturnStationDto>(this.stationList);

            foreach(ReturnStationDto station in stationListWithoutFilter)
            {
                this.stationList.Add(station);
            }
           
            stationList.Sort((a, b) =>
            {
                var aValue = a.GetType().GetProperty(prop)?.GetValue(a) as IComparable;
                var bValue = b.GetType().GetProperty(prop)?.GetValue(b) as IComparable;
                

                if (aValue == null || bValue == null)
                    return 0;

                if (asc)
                    return aValue.CompareTo(bValue);
                else
                    return bValue.CompareTo(aValue);

            });

            this.asc = !this.asc;

            if(prop=="station_id"){
                this.station_id = true;
                this.station_name = false;
            }
            else{
                this.station_id = false;
                this.station_name = true;
            }
            
        }

        private void filterFn()
        {
            
            stationList = new List<ReturnStationDto>();
            this._navigationService = new NavigationService<ReturnStationDto>(this.stationList);

            List<ReturnStationDto> filteredStations;
            
            if (this.selectedModel == "id")
            {
                filteredStations = stationListWithoutFilter
                                    .Where(s => s.station_id
                                        .ToString()
                                        .Contains(StationFilter?.Trim() ?? string.Empty,
                                                StringComparison.OrdinalIgnoreCase))
                                    .ToList();
            }
            else
            {
                filteredStations = stationListWithoutFilter
                                    .Where(s => s.station_name
                                        .Contains(StationFilter?.Trim() ?? string.Empty,
                                                StringComparison.OrdinalIgnoreCase))
                                    .ToList();
            }

            // Use a for loop to add items (like your original TS code)
            for (int i = 0; i < filteredStations.Count; i++)
            {
                //Console.WriteLine(filteredStations[i].station_name);
                stationList.Add(filteredStations[i]);
            }
            
        }


    }
}

public class IntReference 
{
    public int Value { get; set; }
}