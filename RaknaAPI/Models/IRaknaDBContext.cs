﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;

namespace RaknaAPI.Models
{
    public interface IRaknaDBContext
    {
        DbSet<User> User { get; set; }
        DbSet<UserAccount> UserAccount { get; set; }
    }
}