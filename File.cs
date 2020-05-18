using System;

namespace lab5
{
    public class AccessMatrix
    {
        public bool[] Owner { get; set; }
        public bool[] Group { get; set; }
        public bool[] Other { get; set; }

        public AccessMatrix()
        {
            Owner = new bool[3] { true, true, false };
            Group = new bool[3] { true, false, false };
            Other = new bool[3] { false, false, false };
        }
    }

    public class File
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public User Owner { get; set; }

        private AccessMatrix accessMatrix;

        public File(string name, User user)
        {
            Name = name;
            accessMatrix = new AccessMatrix();
            Owner = user;
        }

        private bool HavePermission(User user, int operationIndex)
        {
            bool isOwner = Owner.Equals(user);
            bool inGroup = user.InGroup(Group);
            if (isOwner && accessMatrix.Owner[operationIndex])
                return true;
            if (inGroup && accessMatrix.Group[operationIndex])
                return true;
            if (!inGroup && accessMatrix.Other[operationIndex])
                return true;
            return false;
        }

        private void Process(User user, int operationIndex)
        {
            if (HavePermission(user, operationIndex))
                Msg.Ok();
            else
                Msg.Forbid();
        }

        public void Read(User user)
        {
            Process(user, 0);
        }

        public void Write(User user)
        {
            Process(user, 1);
        }

        public void Exec(User user)
        {
            Process(user, 2);
        }

        public void setMask(string mask)
        {
            if (mask.Length != 3)
            {
                Console.WriteLine("Wrong mask");
                return;
            }
            int own = int.Parse(mask[0].ToString());
            int gro = int.Parse(mask[1].ToString());
            int oth = int.Parse(mask[2].ToString());
            if (own > 7 || gro > 7 || oth > 7)
            {
                Console.WriteLine("Wrong mask");
                return;
            }
            
            accessMatrix.Owner[2] = own % 2 == 1;
            accessMatrix.Group[2] = gro % 2 == 1;
            accessMatrix.Other[2] = oth % 2 == 1;

            accessMatrix.Owner[0] = own > 3;
            accessMatrix.Group[0] = gro > 3;
            accessMatrix.Other[0] = oth > 3;

            own /= 2;
            gro /= 2;
            oth /= 2;

            accessMatrix.Owner[1] = own % 2 == 1;
            accessMatrix.Group[1] = gro % 2 == 1;
            accessMatrix.Other[1] = oth % 2 == 1;
        }

        public override string ToString()
        {
            return $"{Name}\t{Owner.Name}:{Group}";
        }
    }
}
