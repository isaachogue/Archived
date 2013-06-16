using System;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
namespace SabreExcelImport
{
    public interface IMeetingReport
    {
        System.Collections.Generic.List<Conference> Meetings { get; }
    }
}
