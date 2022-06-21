using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace JudicialTest
{
    public class ModelDB : DbContext
    {
        public ModelDB(string connectionString) : base(connectionString) { }

        public DbSet<Apartment> Apartments_DB { get; set; }
        public DbSet<House> Houses_DB { get; set; }
        public DbSet<Debtor> Debtors_DB { get; set; }
        public DbSet<Arrear> Arrears_DB { get; set; }
        public DbSet<Extract> Extracts_DB { get; set; }
        public DbSet<EstateAddress> EstateAddresses_DB { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigProperties(modelBuilder);
            ConfigLinks(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void ConfigProperties(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<House>().Ignore(c => c.PickStatus).
                HasKey(c => c.HouseID).
                Property(c => c.HouseID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Apartment>().Ignore(c => c.PickStatus).
                HasKey(c => c.ApartmentID).
                Property(c => c.ApartmentID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Debtor>().Ignore(c => c.PickStatus).
                HasKey(c => c.DebtorID).
                Property(c => c.DebtorID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Arrear>().Ignore(c => c.PickStatus).
                HasKey(c => c.ArrearID).
                Property(c => c.ArrearID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Extract>().Ignore(c => c.PickStatus).
                HasKey(c => c.ExtractID).
                Property(c => c.ExtractID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EstateAddress>().
                HasKey(c => c.EstateAddressID).
                Property(c => c.EstateAddressID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
        private void ConfigLinks(DbModelBuilder modelBuilder)
        {
            //связь один-ко-многим House с Apartments
            modelBuilder.Entity<House>().
                HasMany(c => c.Apartments).
                WithRequired(c => c.House).WillCascadeOnDelete(false);
            //связь многие-ко-многим Apartments с Debtors
            modelBuilder.Entity<Apartment>().
                HasMany(apart => apart.Debtors).
                WithMany(debtor => debtor.Apartments).
                Map(keys =>
                {
                    keys.ToTable("Apart_Debtor");
                    keys.MapLeftKey("ApartmentId");
                    keys.MapRightKey("DebtorId");
                });
            //связь один-ко-многим Apartment с Arrears
            modelBuilder.Entity<Apartment>().
                HasMany(c => c.Arrears).
                WithRequired(c => c.Apartment).WillCascadeOnDelete(false);

            //связь один-ко-многим Debtor с Arrears
            modelBuilder.Entity<Debtor>().
                HasMany(debtor => debtor.Arrears).
                WithRequired(arrears => arrears.Debtor).WillCascadeOnDelete(false);

            //связь один-ко-многим Apartment с Extracts_EGRN (nullable)
            modelBuilder.Entity<Apartment>().
                HasMany(c => c.Extracts).
                WithOptional(c => c.Apartment).WillCascadeOnDelete(false);

            //связь один-ко-многим Debtor с Extracts_EGRN (nullable)
            modelBuilder.Entity<Debtor>().
                HasMany(c => c.Extracts).
                WithOptional(c => c.Debtor).WillCascadeOnDelete(false);

            //связь один-к-одному Debtor с EstateAddress (nullable)
            modelBuilder.Entity<Debtor>().
                HasOptional(c => c.LocationAddress).
                WithOptionalDependent(c => c.DebtorLoc);

            //связь один-к-одному Debtor с EstateAddress (nullable)
            modelBuilder.Entity<Debtor>().
                HasOptional(c => c.RegisterAddress).
                WithOptionalDependent(c => c.DebtorReg);

            //связь один-к-одному Debtor с EstateAddress (nullable)
            modelBuilder.Entity<Debtor>().
                HasOptional(c => c.SectorAddress).
                WithOptionalDependent(c => c.DebtorSec);
        }
    }
}