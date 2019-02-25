using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Enums
{
    public enum RoleEnum: int
    {
        Patient = 1,
        Receptionist = 2,
        Nurse = 3,
        LabTechnician = 4,
        Doctor = 5,
        WarehouseKeeper = 6,
        Manager = 7,
        Cashier = 8
    }
}
