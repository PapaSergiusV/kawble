using System;

namespace lab5
{
    class Msg
    {
        public static void Ok() => Console.WriteLine("Permission confirmed");

        public static void Forbid() => Console.WriteLine("Permission denied");
    }
}
