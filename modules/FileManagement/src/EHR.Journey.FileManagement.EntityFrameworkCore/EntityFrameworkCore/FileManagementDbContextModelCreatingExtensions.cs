namespace EHR.Journey.FileManagement.EntityFrameworkCore;

public static class FileManagementDbContextModelCreatingExtensions
{
    public static void ConfigureFileManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));


        builder.Entity<EHR.Journey.FileManagement.Files.File>(b =>
        {
            b.ToTable(FileManagementDbProperties.DbTablePrefix + nameof(EHR.Journey.FileManagement.Files.File), FileManagementDbProperties.DbSchema);
            b.HasIndex(q => q.FileName);
            b.HasIndex(q => q.CreationTime);
            b.ConfigureByConvention();
        });
    }
}