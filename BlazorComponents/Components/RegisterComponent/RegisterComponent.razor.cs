using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Interfaces;
using Models;
using Models.Dtos;

namespace BlazorComponents.Components.RegisterComponent
{
    public partial class RegisterComponent: ComponentBase
    {
        
        IUserService _userService;
        public RegisterComponent(IUserService userService)
        {
            _userService = userService;
            onClear();
        }


        //Form state
        private bool invalid = true;
        private string errMessege="";

        //username
        public string UserName { get; set; }
        private bool usernameTouched=false;
        private bool usernameBlured=false;

        private void OnUsernameFocus() =>  usernameTouched =true;

        private void OnUsernameBlur()
        {
            invalid=!ValidateFields();
            usernameBlured =true;
        }


        //userpassword
        public string UserPassword { get; set; }
        private bool passwordTouched=false;
        private bool passwordBlured=false;

        private void OnpasswordFocus() =>  passwordTouched =true;

        private void OnpasswordBlur()
        {
            invalid=!ValidateFields();
            passwordBlured =true;
        }

        //Retype Password
        public string RetypePassword {get;set;}
        private bool RetypePasswordTouched=false;
        private bool RetypePasswordBlured=false;

        private void OnRetypePasswordFocus() =>  RetypePasswordTouched =true;

        private void OnRetypePasswordBlur()
        {
            invalid=!ValidateFields();
            RetypePasswordBlured =true;
        }

        //Mobile No
        public string MobileNo {get;set;}
        private bool MobileNoTouched=false;
        private bool MobileNoBlured=false;

        private void OnMobileNoFocus() =>  MobileNoTouched =true;

        private void OnMobileNoBlur()
        {
            invalid=!ValidateFields();
            MobileNoBlured =true;
        }

        //National ID
        public string NationalId {get;set;}
        private bool NationalIdTouched=false;
        private bool NationalIdBlured=false;

        private void OnNationalIdFocus() =>  NationalIdTouched =true;

        private void OnNationalIdBlur()
        {
            invalid=!ValidateFields();
            NationalIdBlured =true;
        }

        //Email
        public string Email{get;set;}
        private bool emailTouched=false;
        private bool emailBlured=false;

        private void OnemailFocus() =>  emailTouched =true;

        private void OnemailBlur()
        {
            invalid=!ValidateFields();
            emailBlured =true;
        }

        //Fullname
        private string _fullName;
        public string Fullname 
        {
            get
            {
                return _fullName;
            }
            set
            {
                _fullName=value?.ToUpper();
               UpdateNameParts(_fullName);
            }
        }
        private bool FullnameTouched=false;
        private bool FullnameBlured=false;

        private void OnFullnameFocus() =>  FullnameTouched =true;

        private void OnFullnameBlur()
        {
            invalid=!ValidateFields();
            FullnameBlured =true;
        }

        //prefered name
        public string PreferedName{get;set;}
        private bool PreferedNameTouched=false;
        private bool PreferedNameBlured=false;

        private void OnPreferedNameFocus() =>  PreferedNameTouched =true;

        private void OnPreferedNameBlur()
        {
            invalid=!ValidateFields();
            PreferedNameBlured =true;
        }

        //
        private List<string> nameParts = new List<string>();

        public bool ValidateFields()
        {
            return IsUserNameValid() && IsUserPasswordValid() && IsRetypePasswordValid() && IsMobileNoValid() &&
                    IsNationalIdValid() && IsEmailValid() && IsFullnameValid();
        }

        public bool IsUserNameValid()
        {
            return !string.IsNullOrWhiteSpace(UserName);
        }

        public bool IsUserPasswordValid()
        {
            return this.UserPassword != "" && this.UserPassword.Length>=4;
        }

        public bool IsRetypePasswordValid()
        {
            return this.RetypePassword==this.UserPassword;
        }

        public bool IsMobileNoValid()
        {
            string pattern = @"^\d{10}$";
            return Regex.IsMatch(MobileNo ?? string.Empty, pattern);
        }

        public bool IsNationalIdValid()
        {
            string pattern = @"^(\d{11}|\d{9}V)$";
            return Regex.IsMatch(NationalId ?? string.Empty, pattern);
        }

        public bool IsEmailValid()
        {
            return !string.IsNullOrWhiteSpace(Email);
        }

        public bool IsFullnameValid()
        {
            string pattern = @"^[A-Z\s]*$";
            return Regex.IsMatch(Fullname ?? string.Empty, pattern);
        }

        private void UpdateNameParts(string Fullname)
        {
            if (string.IsNullOrWhiteSpace(_fullName))
            {
                nameParts.Clear();
                return;
            }

            // Split by whitespace, remove empty entries
            nameParts = _fullName
                .Trim()
                .Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        private void onClear()
        {
            usernameTouched=false;
            usernameBlured=false;

            passwordTouched=false;
            passwordBlured=false;

            RetypePasswordTouched = false;
            RetypePasswordBlured = false;

            emailTouched = false;
            emailBlured = false;

            MobileNoTouched = false;
            MobileNoBlured = false;

            NationalIdTouched = false;
            NationalIdBlured = false;

            FullnameTouched = false;
            FullnameBlured = false;

            PreferedNameTouched = false;
            PreferedNameBlured = false;
            
            this.UserName=string.Empty;
            this.UserPassword=string.Empty;
            this.RetypePassword = "";
            this.Email = "";
            this.MobileNo = "";
            this.NationalId = "";
            this.Fullname = "";
            this.PreferedName = "";

            errMessege="";
            invalid =true;
        }

        private async Task onSubmit()
        {
            ResponseModel responseModel=await _userService.RegisterUser(
                new RegisterUserDto()
                {
                    UserName=this.UserName,
                    Password= this.UserPassword,
                    Email=this.Email,
                    MobileNo=this.MobileNo,
                    NationalId=this.NationalId,
                    PreferedName=this.PreferedName,
                    Name=this.nameParts.ToArray()

                }
            );

            if(responseModel.ErrCode==200)
            {
                errMessege="Success";
            }
            else
            {
                errMessege=responseModel.ErrCode.ToString();
            
            }
        }
    }
}
        
