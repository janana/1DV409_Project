using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeatherApp.ViewModels
{
    public class LocationString
    {
        [DisplayName("Stad")] 
        [Required(ErrorMessage="Du måste ange en stad.")]
        public string Location
        {
            get;
            set;
        }
    }
}