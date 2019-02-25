using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

/********************************************************************/
/*  Reference: https://github.com/Hoangpnse62077/CasptoneProject    */
/*  Gmail: hoangpnse62077@fpt.edu.vn                                */
/********************************************************************/

namespace eLTMS.DataAccess.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {

        public DbContext Context { get; set; }

        public string ConnectionString
        {
            get { return Context.Database.Connection.ConnectionString; }
            set { Context.Database.Connection.ConnectionString = value; }
        }

        public bool LazyLoadingEnabled { get; set; }

        public bool ProxyCreationEnabled { get; set; }

     
        public UnitOfWork(string connectionString = null)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                Context = new FinalProjectContext();
            else
                Context = new FinalProjectContext(connectionString);
        }

        public ICollection<ValidationResult> SaveChanges()
        {
            var validationResults = new List<ValidationResult>();
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbe)
            {
                foreach (DbEntityValidationResult validation in dbe.EntityValidationErrors)
                {
                    ICollection<ValidationResult> validations = validation.ValidationErrors.Select(
                        error => new ValidationResult(
                                     error.ErrorMessage,
                                     new[]
                                         {
                                             error.PropertyName
                                         })).ToList();

                    validationResults.AddRange(validations);

                    return validationResults;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("The records you attempted to edit or delete"
                                               + " were modified by another user after you got the original value. The"
                                               + " operation was canceled.", ex);
            }
            catch (DataException ex)
            {
                throw new DataException("Unable to save changes.", ex);
            }
            return validationResults;
        }

        public async Task<ICollection<ValidationResult>> SaveChangesAsync()
        {
            var validationResults = new List<ValidationResult>();
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbe)
            {
                foreach (DbEntityValidationResult validation in dbe.EntityValidationErrors)
                {
                    ICollection<ValidationResult> validations = validation.ValidationErrors.Select(
                        error => new ValidationResult(
                                     error.ErrorMessage,
                                     new[]
                                         {
                                             error.PropertyName
                                         })).ToList();

                    validationResults.AddRange(validations);

                    return validationResults;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = ex.Entries.Select(e => e.GetDatabaseValues()).ToList();
                throw new ConcurrencyException("The records you attempted to edit or delete"
                                               + " were modified by another user after you got the original value. The"
                                               + " operation was canceled.", databaseValues, ex);
            }
            catch (Exception ex)
            {
                throw new DataException("Unable to save changes.", ex);
            }
            return validationResults;
        }

        /// <summary>
        /// When we call begin transaction. Our proxy creates new Database.BeginTransaction and gives DbContextTransaction's control to proxy.
        /// We do this for unit test.
        /// </summary>
        /// <returns>Proxy which controls DbContextTransaction(Ef transaction class)</returns>
        public IDbContextTransactionProxy BeginTransaction()
        {
            return new DbContextTransactionProxy(this.Context);
        }

        public void Dispose()
        {
            if (Context.Database.Connection.State == ConnectionState.Open)
            {
                Context.Database.Connection.Close();
            }
            Context.Dispose();
            GC.WaitForPendingFinalizers();
        }


        private IEnumerable<DbEntityEntry> GetCreatedEntries()
        {
            var createdEntries = Context.ChangeTracker.Entries().Where(V =>
                         EntityState.Added.HasFlag(V.State)
                    );
            return createdEntries;
        }
    }
}