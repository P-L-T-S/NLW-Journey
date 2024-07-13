using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Entities;
using Journey.Infrastructure.Enums;

namespace Journey.Application.UseCases.Activities.Register;

public class RegisterActivityUseCase
{
    public ResponseActivityJson Execute(RequestRegisterActivityJson request)
    {
        var context = new JourneyDbContext();

        var trip = context.Trips.FirstOrDefault(trip => trip.Id == request.TripId);

        Validate(request, trip);

        var activity = new Activity
        {
            Name = request.Name,
            Date = request.Date,
            TripId = request.TripId,
        };


        context.Activities.Add(activity);
        context.SaveChanges();

        return new ResponseActivityJson
        {
            Id = activity.Id,
            Name = activity.Name,
            Date = activity.Date,
            Status = (Communication.Enums.ActivityStatus)activity.Status
        };
    }

    private void Validate(RequestRegisterActivityJson request, Trip? trip)
    {
        if (trip == null)
        {
            throw new NotFoundException(ResourceErrorMessages.TRIP_NOT_FOUND);
        }

        var validator = new RegisterActivityValidator();

        var validation = validator.Validate(request);


        var isActivityLaterThanStartTrip = request.Date >= trip.StartDate;
        var isActivityEarlyThanEndTrip = request.Date <= trip.EndDate;

        if (isActivityLaterThanStartTrip is false || isActivityEarlyThanEndTrip is false)
        {
            validation.Errors.Add(
                new ValidationFailure
                {
                    ErrorMessage = ResourceErrorMessages.DATE_NOT_WITHIN_TRAVEL_PERIOD,
                }
            );
        }

        if (validation.IsValid) return;

        var errorMessage = validation
            .Errors
            .Select(error => error.ErrorMessage)
            .ToList();

        throw new ErrorOnValidationException(errorMessage);
    }
}