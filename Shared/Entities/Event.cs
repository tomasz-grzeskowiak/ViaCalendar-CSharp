using System;

namespace Entities;

public class Event
{
    public string Name { get; private set; }
    public string Tag { get; private set; }
    public bool Recursive { get; private set; }
    public int CreatorId { get; private set; }
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }
    public string TypeOfRecursive { get; private set; }  
    
    public int Id { get; set; } = 0;

    // Builder class
    public class Builder
    {
        private string _name = "Default Name";
        private int _id = 0;
        private string _tag = "Default Tag";
        private bool _recursive;
        private int _creatorId = 0;
        private DateTime _start = DateTime.MinValue;
        private DateTime _end = DateTime.MinValue;
        private string _typeOfRecursive = "";  

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

        public Builder SetCreatorId(int creatorId)
        {
            if (creatorId < 0)
                throw new ArgumentException("CreatorId cannot be negative");
            _creatorId = creatorId;
            return this;
        }

        public Builder SetStart(DateTime start)
        {
            _start = start;
            return this;
        }

        public Builder SetEnd(DateTime end)
        {
            _end = end;
            return this;
        }

        public Builder SetTypeOfRecursive(string typeOfRecursive)  
        {
            _typeOfRecursive = typeOfRecursive ?? "";
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
                CreatorId = _creatorId,
                Start = _start,
                End = _end,
                TypeOfRecursive = _typeOfRecursive  
            };
        }
    }
}