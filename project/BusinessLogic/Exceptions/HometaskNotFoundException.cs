using System;
using BusinessLogic.Exceptions;

namespace BusinessLogic
{
    public class HometaskNotFoundException : BusinessException
    {
        public HometaskNotFoundException(string message) : base(message) { }
    }
}