﻿namespace Entities;

public class Event
{
    public string Name { get; private set; }
    public int Id { get; set; } = 0;

    // Builder inner class
    public class Builder
    {
        private string _name = "Default Name";
        private int _id = 0;
        

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

        public Event Build()
        {
            return new Event
            {
                Name = _name,
                Id = _id
            };
        }
    }
}