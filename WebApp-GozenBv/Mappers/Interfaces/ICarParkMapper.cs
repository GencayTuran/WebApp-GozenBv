using System.Collections.Generic;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Mappers
{
    public interface ICarParkMapper
    {
        CarMaintenancesEditViewModel MapEditMaintenances(CarParkDTO dto);
        List<CarMaintenance> MapViewModelToModel(CarCreateViewModel viewModel);
    }
}