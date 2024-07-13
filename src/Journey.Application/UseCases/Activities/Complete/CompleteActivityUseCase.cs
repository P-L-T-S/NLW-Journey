using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Enums;

namespace Journey.Application.UseCases.Activities.Complete;

public class CompleteActivityUseCase
{
    public void Execute(Guid id)
    {
        var context = new JourneyDbContext();

        var activity = context
            .Activities
            .FirstOrDefault(activity => activity.Id == id);

        if (activity is null)
        {
            throw new NotFoundException(ResourceErrorMessages.ACTIVITY_NOT_FOUND);
        }

        activity.Status = ActivityStatus.Done;

        context.Activities.Update(activity);
        context.SaveChanges();
    }
}