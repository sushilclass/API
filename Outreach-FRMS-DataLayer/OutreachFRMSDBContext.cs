using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_DataLayer
{
    public class OutreachFRMSDBContext : DbContext
    {
        public OutreachFRMSDBContext(DbContextOptions<OutreachFRMSDBContext> options) : base(options)
        {

        }
    }
}
