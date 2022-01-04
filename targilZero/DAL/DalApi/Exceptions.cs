using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DalApi
{
    public class ExistIdException : Exception //מחלקה ליצוג חריגה של מספר זהות קיים במערכת
    {
        public ExistIdException() : base() { }
        public ExistIdException(string message) : base(message) { }
        public ExistIdException(string message, Exception inner) : base(message, inner) { }

    }
    public class ObjectNotFoundException : Exception //מחלקה ליצוג חריגה של אובייקט שלא קיים במאגר
    {
        public ObjectNotFoundException() : base() { }
        public ObjectNotFoundException(string message) : base(message) { }
        public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }

    }
    public class WrongInputException : Exception //מחלקה ליצוג חריגה של קלט לא תקין
    {
        public WrongInputException() : base() { }
        public WrongInputException(string message) : base(message) { }
        public WrongInputException(string message, Exception inner) : base(message, inner) { }

    }

    public class LoadingException : Exception 
    {
        string filePath;
        public LoadingException() : base() { }
        public LoadingException(string message) : base(message) { }
        public LoadingException(string message, Exception inner) : base(message, inner) { }

        public LoadingException(string path, string messege, Exception inner) => filePath = path;
        protected LoadingException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }

}
