﻿namespace Shop.UIForms.View_Models
{
    public class MainViewModel
    {
        public LoginViewModel Login { get; set; }
        public MainViewModel()
        {
            this.Login = new LoginViewModel();
        }
    }
}