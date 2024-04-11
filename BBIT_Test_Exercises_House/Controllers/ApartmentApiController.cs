using AutoMapper;
using BBIT_Test_Exercises_House.DTOs;
using BBIT_Test_Exercises_House.Storage;
using Microsoft.AspNetCore.Mvc;

namespace BBIT_Test_Exercises_House.Controllers;

[Route("apartment")]
[ApiController]
public class ApartmentApiController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IApartmentService _apartmentService;

    public ApartmentApiController(IMapper mapper, IApartmentService apartmentService)
    {
        _mapper = mapper;
        _apartmentService = apartmentService;
    }

    [HttpPost]
    [Route("add")]
    public IActionResult AddApartment(Apartment apartment)
    {
        if (_apartmentService.GetById(apartment.Id) != null)
        {
            return Conflict();
        }

        var apartmentViewModel = _mapper.Map<ApartmentDto>(apartment);
        _apartmentService.Add(apartment);
        return Created("", apartmentViewModel);
    }

    [HttpGet]
    [Route("apartment/{id}")]
    public IActionResult GetApartment(int id)
    {
        var apartment = _apartmentService.GetById(id);// trows null reference exception but i dont want it to do that...
        if (apartment == null)
        {
            return NotFound();
        }
        var apartmentViewModel = _mapper.Map<ApartmentDto>(apartment);

        return Ok(apartmentViewModel);
    }

    [HttpDelete]
    [Route("apartment/{id}")]
    public IActionResult DeleteApartment(int id)
    {
        var apartmentToDelete = _apartmentService.GetById(id);
        if (apartmentToDelete == null)
        {
            return NotFound();
        }

        _apartmentService.Delete(apartmentToDelete);
        return Ok();
    }
    
    [HttpPut]
    [Route("apartment/{number}")]
    public IActionResult EditApartment([FromBody] EditApartmentRequest request)
    {
        int id = request.id;
        Apartment updatedApartmentData = request.ApartmentData;
        
        var apartmentToEdit = _apartmentService.GetById(request.id);
        if (apartmentToEdit == null)
        {
            return NotFound();
        }
        _apartmentService.EditApartment(request.id, request.ApartmentData);

        return Ok();
    }
}