using System;
namespace WebApp_GozenBv.CustomExceptions
{
	public class UserInputException : Exception
	{
        public UserInputException(string message) : base(message)
        {
        }
    }
}

