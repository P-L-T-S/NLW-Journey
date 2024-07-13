using FluentValidation;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Entities;

namespace Journey.Application.UseCases.Trips.Register;

public class RegisterTripUseCase
{
    public ResponseShortTripJson Execute(RequestRegisterTripJson request)
    {
        Validate(request);

        var trip = new Trip
        {
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        var context = new JourneyDbContext();

        context.Trips.Add(trip);
        context.SaveChanges();

        return new ResponseShortTripJson
        {
            Id = trip.Id,
            Name = trip.Name,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate
        };
    }

    private void Validate(RequestRegisterTripJson request)
    {
        var validator = new RegisterTripValidator();

        var validation = validator.Validate(request);

        if (validation.IsValid) return;
        
        var errors = validation
            .Errors
            .Select(error => error.ErrorMessage)
            .ToList();

        throw new ErrorOnValidationException(errors);
    }
}