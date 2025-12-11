namespace Entities;

public class Calendar
{
    public int Id { get; set; } = 0;
    public int UserId { get; private set; }
    public List<int> EventIds { get; private set; } = new List<int>();

    public class Builder
    {
        private int _id = 0;
        private int _userId = 0;
        private List<int> _eventIds = new List<int>();

        public Builder SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("Id cannot be negative");
            _id = id;
            return this;
        }

        public Builder SetUserId(int userId)
        {
            if (userId < 0)
                throw new ArgumentException("UserId cannot be negative");
            _userId = userId;
            return this;
        }

        public Builder AddEventId(int eventId)
        {
            if (eventId < 0)
                throw new ArgumentException("EventId cannot be negative");
            _eventIds.Add(eventId);
            return this;
        }

        public Builder SetEventIds(List<int> eventIds)
        {
            _eventIds = eventIds ?? new List<int>();
            return this;
        }

        public Builder SetEventIds(IEnumerable<int> eventIds)
        {
            _eventIds = eventIds?.ToList() ?? new List<int>();
            return this;
        }

        public Calendar Build()
        {
            return new Calendar
            {
                Id = _id,
                UserId = _userId,
                EventIds = new List<int>(_eventIds) // Create a copy to prevent external modification
            };
        }
    }
}