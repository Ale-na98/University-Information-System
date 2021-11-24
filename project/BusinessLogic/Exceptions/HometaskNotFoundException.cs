using System;

namespace BusinessLogic
{
    public class HometaskNotFoundException : BusinessException
    {
        public HometaskNotFoundException(string message) : base(message) { }
    }
}