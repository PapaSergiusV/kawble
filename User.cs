using System;
using System.Text;
using System.Collections.Generic;

namespace lab5
{
    public class User
    {
        public string Name { get; set; }
        private List<string> groups { get; set; }

        public User(string name)
        {
            Name = name;
            groups = new List<string>();
        }

        public override bool Equals(object obj)
        {
            return Name == ((User)obj).Name;
        }

        public bool InGroup(string group)
        {
            return group != null && groups.Contains(group);
        }

        public void addGroup(string group)
        {
            if (!groups.Contains(group))
                groups.Add(group);
        }

        public string GetFirstGroup()
        {
            if (groups.ToArray().Length == 0)
                return null;
            return groups[0];
        }

        public override string ToString()
        {
            StringBuilder gs = new StringBuilder();
            foreach (string g in groups)
                gs.Append($" {g}");
            return $"{Name}: {gs.ToString()}";
        }
    }
}
