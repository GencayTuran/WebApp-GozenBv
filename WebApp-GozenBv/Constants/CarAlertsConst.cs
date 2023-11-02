using Microsoft.AspNetCore.Mvc;

namespace WebApp_GozenBv.Constants
{
    public static class CarAlertsConst
    {
        public const int KeuringOneMonth = 1; //alerts when KeuringDate is within 1 month 
        public const int KeuringOutdated = 2; //alerts when KeuringDate passes
        public const int MaintenanceOneMonth = 3; //alerts when MaintenanceDate is within 1 month 
        public const int MaintenanceKm = 4; //alerts when less than 5000km left
    }
}
