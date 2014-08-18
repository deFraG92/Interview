using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Interview.InterviewWorker
{
    public class Question 
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            var question = obj as Question;
            if ( (Name == null) | (question.Name == null) )
                return false;
            return Name == question.Name;
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

    }
}
