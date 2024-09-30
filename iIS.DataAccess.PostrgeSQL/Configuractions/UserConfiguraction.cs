using iIS.DataAccess.PostrgeSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iIS.DataAccess.PostrgeSQL.Configuractions
{
    public class UserConfiguraction : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(user => user.Id);
        }
    }
}