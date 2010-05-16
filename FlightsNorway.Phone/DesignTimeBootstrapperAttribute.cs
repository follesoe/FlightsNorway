using System;
using System.Windows;
using FlightsNorway.Phone;
using FlightsNorway.Phone.Model;
using GalaSoft.MvvmLight;

[assembly: DesignTimeBootstrapper]

namespace FlightsNorway.Phone
{   
    [AttributeUsage(AttributeTargets.Assembly)]
    public class DesignTimeBootstrapperAttribute : Attribute
    {
        static DesignTimeBootstrapperAttribute()
        {
            if(ViewModelBase.IsInDesignModeStatic)
            {
                var viewModelLocator = (ViewModelLocator) Application.Current.Resources["ViewModelLocator"];

                viewModelLocator.FlightsViewModel.Arrivals.Add(new Flight());
                viewModelLocator.FlightsViewModel.Arrivals.Add(new Flight());
                viewModelLocator.FlightsViewModel.Arrivals.Add(new Flight());
            }
        }
    }
}
