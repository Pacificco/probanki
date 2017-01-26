using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain
{
    public enum VM_UserSex
    {
        Undefined = 0,
        Male,
        Female
    }
    public enum VM_ForecastTariff
    {
        Undefined = 0,
        Platinum = 1,
        Gold = 2,
        Silver = 3,
        Bronze = 4
    }
    public enum EnumBoolType
    {
        None = 0,
        True,
        False
    }
    public enum EnumSex
    { 
        Undefined = 0,
        Male,
        Female
    }
    public enum EnumConfirmStatus 
    {
        None = 0,        
        Confirmed,
        NotConfirmed
    }
    public enum EnumFirstDropDownItem
    { 
        None,
        All,
        NotSelected
    }
    public enum EnumOrgPointType 
    {
        None =  0,
        Office,
        ATM
    }
    public enum SQLDateTimeFormat { sdfDateTime, sdfDate, sdfTime, sdfDateTime_TimeMin, sdfDateTime_TimeMax };    
}