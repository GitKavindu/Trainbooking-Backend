using Microsoft.AspNetCore.Components;
using BlazorComponents.Service;
using Models.Dtos;

namespace BlazorComponents.Components.HomeComponent
{
    public partial class HomeComponent:ComponentBase
    {
        private NavigationManager _navigation;
        private SharedService service;
        private ReturnTokenDto?  _token; 
        public HomeComponent(NavigationManager navigation,SharedService sharedService)
        {
            _navigation = navigation;
            service = sharedService;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                await UpdateToken();
                await this.service._tokenService.SubscribeToken(HandleTokenChanged);
                await this.service.print("first admin is "+_token?.isAdmin.ToString());
            }
            else
            {
                 await this.service.print("admin is "+_token?.isAdmin.ToString());
            }

            await this.service.print(firstRender.ToString());
           

        }

        private void HandleTokenChanged()
        {
            this.UpdateToken();
            
        }

        public void Dispose()
        {
            this.service._tokenService.UnsubscribeToken(HandleTokenChanged) ;
        }

        private async Task UpdateToken()
        {
            this._token=await service._tokenService.ReturnToken();
            StateHasChanged();
        }

        private void goToBooking(){
            this._navigation.NavigateTo("/booking");
        }

        private void goToMyBookings(){
            this._navigation.NavigateTo("/mybookings");
        }

        private void goToSchedule(){
            this._navigation.NavigateTo("/schedule");
        }

        private void goToLogin(){
            this._navigation.NavigateTo("/login");
        }

        private void goToRegister(){
            this._navigation.NavigateTo("/register");
        }
    }
}