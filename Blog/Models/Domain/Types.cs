using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain
{
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