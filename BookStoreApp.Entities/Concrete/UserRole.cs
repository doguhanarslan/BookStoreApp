﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.Entities;

namespace BookStoreApp.Entities.Concrete
{
    public class UserRole:IEntity
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }
}
