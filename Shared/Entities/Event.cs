namespace Entities;

public class Event
{
    public string Name { get; private set; }
    public string Tag { get; private set; }
    public bool Recursive { get; private set; }
    public int CreatorId { get; private set; }  // Added CreatorId
    
    public int Id { get; set; } = 0;

    // Builder inner class
    public class Builder
    {
        private string _name = "Default Name";
        private int _id = 0;
        private string _tag = "Default Tag";
        private bool _recursive = true;
        private int _creatorId = 0;  // Added CreatorId to builder

        public Builder SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty");
            _name = name;
            return this;
        }

        public Builder SetId(int id)
        {
            if (id < 0) 
                throw new ArgumentException("Id cannot be negative");
            _id = id;
            return this;
        }

        public Builder SetRecursive(bool recursive)
        {
            _recursive = recursive;
            return this;
        }

        public Builder SetTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentException("Tag cannot be null or empty");
            _tag = tag;
            return this;
        }

        public Builder SetCreatorId(int creatorId)  // Added SetCreatorId method
        {
            if (creatorId < 0)
                throw new ArgumentException("CreatorId cannot be negative");
            _creatorId = creatorId;
            return this;
        }

        public Event Build()
        {
            return new Event
            {
                Name = _name,
                Id = _id,
                Tag = _tag,
                Recursive = _recursive,
                CreatorId = _creatorId  // Set CreatorId
            };
        }
    }
}