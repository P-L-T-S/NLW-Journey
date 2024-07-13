using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;

namespace Journey.Application.UseCases.Activities.Delete;

public class DeleteActivityUseCase
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

        context.Activities.Remove(activity);
        context.SaveChanges();
    }
}