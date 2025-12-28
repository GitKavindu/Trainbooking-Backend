using Microsoft.AspNetCore.Components;
using Interfaces;
using Models;
using Models.Dtos;
using BlazorComponents.Service;

namespace BlazorComponents.Components.LoginComponent
{
    public partial class LoginComponent:ComponentBase
    {

        private IUserService _userService;
        private ITokenService _tokenService;

        private NavigationManager _navigation;
        public LoginComponent(IUserService userService,ITokenService tokenService,NavigationManager navigation)
        {
            _userService = userService;
            _tokenService = tokenService;
            _navigation = navigation;
        }

        private string errMessege="";
        private UserSubmitModel userSubmitModel=new UserSubmitModel();

        //Form state
        private bool invalid = true;

        //username
        public string UserName { get; set; }
        private bool usernameTouched=false;
        private bool usernameBlured=false;

        private void OnUsernameFocus() =>  usernameTouched =true;

        private void OnUsernameBlur()
        {
            invalid=ValidateFields();
            usernameBlured =true;
        }

        //userpassword
        public string UserPassword { get; set; }
        private bool passwordTouched=false;
        private bool passwordBlured=false;

        private void OnpasswordFocus() =>  passwordTouched =true;

        private void OnpasswordBlur()
        {
            invalid=ValidateFields();
            passwordBlured =true;
        }


        private void onClear()
        {
            usernameTouched=false;
            usernameBlured=false;

            passwordTouched=false;
            passwordBlured=false;

            this.UserName="";
            this.UserPassword="";

            errMessege="";
            invalid =this.ValidateFields();
        }

        private async Task onSubmit()
        {
            ResponseModel responseModel=await _userService.GetUserToken(
            new UserSubmitModel()
            {
                UserName=this.UserName,
                UserPassword=this.UserPassword
            }
            );

            if(responseModel.ErrCode==200)
            {
                errMessege="Success";

                // Save to local storage
                await _tokenService.SetToken((ReturnTokenDto)responseModel.Data);

                _navigation.NavigateTo("/station");
            }
            else
            {
                errMessege=responseModel.ErrCode.ToString();
            
            }
        }


        public bool ValidateFields()
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(UserPassword) && UserPassword.Length>=4)
            {
                return false;
            }

            return true;
        }
    }
}