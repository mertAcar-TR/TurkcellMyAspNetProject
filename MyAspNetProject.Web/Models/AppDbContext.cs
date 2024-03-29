﻿using System;
using Microsoft.EntityFrameworkCore;

namespace MyAspNetProject.Web.Models
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Visitor> Visitors { get; set; }
		public DbSet<Category> Category { get; set; }
	}
}

