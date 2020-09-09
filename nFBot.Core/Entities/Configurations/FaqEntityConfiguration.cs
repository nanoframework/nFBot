//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace nanoFramework.Tools.nFBot.Core.Entities.Configurations
{
    public class FaqEntityConfiguration : IEntityTypeConfiguration<FaqEntity>
    {
        public void Configure(EntityTypeBuilder<FaqEntity> builder)
        {
            builder.HasKey(faq => faq.Id);
            builder.HasIndex(faq => faq.Tag).IsUnique();

            builder.Property(faq => faq.Tag).HasMaxLength(512).IsRequired();
            builder.Property(faq => faq.Content).IsRequired();
            builder.Property(faq => faq.Creator).IsRequired();
            builder.Property(faq => faq.CreatedDate).IsRequired();
        }
    }
}