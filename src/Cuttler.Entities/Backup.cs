using System;

namespace Cuttler.Entities
{
    public class Backup
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string Path { get; set; }

        public Guid TenantId { get; set; }
    }
}