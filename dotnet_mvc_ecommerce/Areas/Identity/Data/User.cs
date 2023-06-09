﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_mvc_ecommerce.Models;
using Microsoft.AspNetCore.Identity;

namespace dotnet_mvc_ecommerce.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{

    //Relationship
    public virtual ICollection<Order> Orders { get; set; }

    public virtual ICollection<ShoppingBasket> ShoppingBaskets { get; set; }

    public virtual ICollection<CustomerDetail>? CustomerDetail { get; set; }
}

