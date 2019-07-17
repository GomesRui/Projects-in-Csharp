using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ValidatePhoto.Services.Face

{

    public enum PersonGroups
    {
        Security = 0,
        HR = 1,
        Development = 2,
        Janitors = 3
    };

    public class PersonGroupsID
    {

        private const string SecurityPGID = "employee_security_group1";
        private const string HRPGID = "employee_HR_group1";
        private const string DevelopmentPGID = "employee_development_group1";
        private const string JanitorsPGID = "employee_janitors_group1";

        private string[] PGIDs = new string[] {SecurityPGID,HRPGID,DevelopmentPGID,JanitorsPGID};

        Dictionary<PersonGroups, string> PersonGroupsIDs = new Dictionary<PersonGroups, string>();

        public PersonGroupsID(int group)
        {
            PersonGroupsIDs.Add((PersonGroups)group, PGIDs[group]);
        }

        public string pPersonGroupName
        {
            get
            {
                return PersonGroupsIDs.First().Value;
            }
        }
        public PersonGroups pPersonGroupId
        {
            get
            {
                return PersonGroupsIDs.First().Key;
            }
        }


    }
}
