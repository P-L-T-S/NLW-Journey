using Journey.Communication.Responses;
using Journey.Infrastructure;

namespace Journey.Application.UseCases.Trips.GetAll;

public class GetAllTripUseCase
{
    public ResponseTripsJson Execute()
    {
        var context = new JourneyDbContext();

        var trips = context.Trips
            .ToList();

        return new ResponseTripsJson
        {
            Trips = trips
                .Select(trip => new ResponseShortTripJson
                {
                    Id = trip.Id,
                    Name = trip.Name,
                    StartDate = trip.StartDate.ToLocalTime(),
                    EndDate = trip.EndDate.ToLocalTime()
                })
                .ToList()
        };
    }
}