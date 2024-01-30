using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace EHR.Journey.Core
{
    public abstract class BaseDbContext<T> : AbpDbContext<T> where T : DbContext
    {
        private readonly byte[] _encryptionKey = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        private readonly byte[] _encryptionIV = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        private readonly IEncryptionProvider _provider;
        protected bool? CustomDataFilter;
        public BaseDbContext(DbContextOptions<T> options) : base(options)
        {
            this._provider = new AesProvider(this._encryptionKey, this._encryptionIV);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(this._provider);
            base.OnModelCreating(modelBuilder);

        }
    }
}
