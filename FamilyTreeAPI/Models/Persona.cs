using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyTreeAPI.DATA
{
    public partial class Persona
    {
        public const string FEMALE_GENDER_STRING = "FEMALE";
        public const string FEMALE_GENDER_VALUE = "F";
        public const string MALE_GENDER_STRING = "MALE";
        public const string MALE_GENDER_VALUE = "M";

        public string genderDesc
        {
            get
            {
                return this.Gender == FEMALE_GENDER_VALUE ? FEMALE_GENDER_STRING : MALE_GENDER_STRING;
            }
        }
    }
}