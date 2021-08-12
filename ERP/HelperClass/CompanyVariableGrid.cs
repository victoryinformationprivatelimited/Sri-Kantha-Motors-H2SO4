using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HelperClass
{
    public class CompanyVariableGrid
    {

        private Guid employeeID;

        public Guid EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        private Guid variableID;

        public Guid VariableID
        {
            get { return variableID; }
            set { variableID = value; }
        }
       
        private string companyVariableName;
        public string CompanyVariableName
        {
            get { return companyVariableName; }
            set { companyVariableName = value; }
        }

        private bool iSActive;
        public bool ISActive
        {
            get { return iSActive; }
            set { iSActive = value; }
        }

    }
}
