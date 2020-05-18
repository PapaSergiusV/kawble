using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5
{
    class Program
    {

        static void Main(string[] args)
        {
            User user = new User("root");
            List<File> files = new List<File>();
            List<User> users = new List<User>() { user };
            List<string> groups = new List<string>();

            while (true)
            {
                string command = Console.ReadLine();
                if (command == "exit" || command == "q")
                    break;

                string[] cmd = command.Split(" ")
                    .Where(s => s != string.Empty)
                    .ToArray();

                try
                {
                    if (cmd[0] == "adduser")
                    {
                        if (!users.Any(u => u.Name == cmd[1]))
                        {
                            users.Add(new User(cmd[1]));
                            Console.WriteLine($"User {cmd[1]} added");
                        }
                    }
                    else if (cmd[0] == "groupadd")
                    {
                        if (!groups.Contains(cmd[1]))
                        {
                            groups.Add(cmd[1]);
                            Console.WriteLine($"Group {cmd[1]} added");
                        }
                    }
                    else if (cmd[0] == "usermod")
                    {
                        if (groups.Contains(cmd[3]) && users.Any(u => u.Name == cmd[4]))
                        {
                            User u = users.Where(u => u.Name == cmd[4]).FirstOrDefault();
                            if (u == null)
                                Console.WriteLine("User not found");
                            else
                            {
                                u.addGroup(cmd[3]);
                                Console.WriteLine(u);
                            }
                        }
                        else
                            Console.WriteLine("Group or user not found");
                        
                    }
                    else if (cmd[0] == "su")
                    {
                        User found = users.Where(u => u.Name == cmd[1]).FirstOrDefault();
                        if (found != null)
                        {
                            user = found;
                            Console.WriteLine($"Set user {user.Name}");
                        }
                        else
                            Console.WriteLine("User not found");
                    }
                    else if (cmd[0] == "touch")
                    {
                        if (!files.Any(f => f.Name == cmd[1]))
                        {
                            File file = new File(cmd[1], user);
                            files.Add(file);
                            file.Group = user.GetFirstGroup();
                            Console.WriteLine($"File {file.Name} added");
                        }
                    }
                    else if (cmd[0] == "read")
                    {
                        File file = files.Where(f => f.Name == cmd[1]).FirstOrDefault();
                        if (file != null)
                            file.Read(user);
                        else
                            Console.WriteLine("File not found");
                    }
                    else if (cmd[0] == "write")
                    {
                        File file = files.Where(f => f.Name == cmd[1]).FirstOrDefault();
                        if (file != null)
                            file.Write(user);
                        else
                            Console.WriteLine("File not found");
                    }
                    else if (cmd[0] == "exec")
                    {
                        File file = files.Where(f => f.Name == cmd[1]).FirstOrDefault();
                        if (file != null)
                            file.Exec(user);
                        else
                            Console.WriteLine("File not found");
                    }
                    else if (cmd[0] == "chown")
                    {
                        string[] p = cmd[1].Split(":").ToArray();
                        User reqUser = users.Where(u => u.Name == p[0]).FirstOrDefault();
                        string group = groups.Where(g => g == p[1]).FirstOrDefault();
                        File file = files.Where(f => f.Name == cmd[2]).FirstOrDefault();
                        if (reqUser == null || group == null || file == null)
                        {
                            Console.WriteLine("args error");
                        }
                        else
                        {
                            if (reqUser.InGroup(group))
                            {
                                file.Group = group;
                                file.Owner = user;
                            }
                            else
                                Console.WriteLine("User doesnt belong to group");
                        }
                    }
                    else if (cmd[0] == "chmod")
                    {
                        File file = files.Where(f => f.Name == cmd[2]).FirstOrDefault();
                        if (file != null)
                        {
                            if (user.Name == "root" || user.InGroup(file.Group))
                                file.setMask(cmd[1]);
                            else
                                Console.WriteLine("no rights");
                        }
                        else
                            Console.WriteLine("File not found");
                    }
                    else if (cmd[0] == "users")
                    {
                        foreach (User u in users)
                            Console.WriteLine(u);
                    }
                    else if (cmd[0] == "ls")
                        foreach (File f in files)
                            Console.WriteLine(f);
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine("args error");
                }
                
                
            }
        }
    }
}
