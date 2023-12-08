using Convey.CQRS.Events;
using Menchul.Core.Events;
using Menchul.MCode.Application.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Application.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> Map(params IDomainEvent[] events) => events.Select(Map);

        // TODO this class should be used to map internal domain events to external message bus events
        private static IEvent Map(IDomainEvent @event)
            => @event switch
            {
                 // StoryRatingChanged e => new StoryRated(e.Rating.Id.StoryId, e.Rating.Id.UserId, e.Rating.Rate,
                //     e.TotalRate),
                // StoryCreated e => new StorySent(e.Story.Id,
                //     new StorySent.AuthorModel(e.Story.Author.Id, e.Story.Author.Name),
                //     e.Story.Title, e.Story.Tags, e.Story.CreatedAt,
                //     new StorySent.VisibilityModel(e.Story.Visibility.From, e.Story.Visibility.To,
                //         e.Story.Visibility.Highlighted)),
                _ => null
            };
    }
}