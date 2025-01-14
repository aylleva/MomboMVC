using MambaMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MambaMVC.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(a => a.Name).IsRequired().HasColumnType("varchar(30)");
            builder.Property(a => a.Surname).IsRequired().HasColumnType("varchar(50)");
        }
    }
}
