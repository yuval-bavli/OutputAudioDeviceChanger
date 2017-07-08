using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioChanger.AudioApi
{
    public class AudioException : Exception
    {
        public AudioException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
