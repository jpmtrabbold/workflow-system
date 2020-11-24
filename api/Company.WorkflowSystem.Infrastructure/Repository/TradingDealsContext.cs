
using EFCore.BulkExtensions;
using InversionRepo.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Entities.Integrations;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Domain.Services;
using Company.WorkflowSystem.Domain.Util;
using Company.WorkflowSystem.Infrastructure.Extensions;

namespace Company.WorkflowSystem.Infrastructure.Context
{
    public class TradingDealsContext : DbContext
    {
        public readonly ScopedDataService scopedDataService;
        public TradingDealsContext(DbContextOptions<TradingDealsContext> options) : base(options)
        {
            //Database.EnsureCreated(); // this should be used only with in-memory database for unit testing
        }

        public TradingDealsContext(DbContextOptions<TradingDealsContext> options, ScopedDataService scopedDataService)
          : base(options)
        {
            this.scopedDataService = scopedDataService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder
            //    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }

        internal DbSet<AuditEntry> AuditEntries { get; set; }
        internal DbSet<AuditEntryTable> AuditEntryTables { get; set; }
        internal DbSet<AuditEntryField> AuditEntryFields { get; set; }

        internal DbSet<AttachmentType> AttachmentTypes { get; set; }
        internal DbSet<Counterparty> Counterparties { get; set; }
        internal DbSet<Country> Countries { get; set; }
        internal DbSet<Deal> Deals { get; set; }
        internal DbSet<DealAttachment> DealAttachments { get; set; }
        internal DbSet<DealAttachmentVersion> DealAttachmentVersions { get; set; }
        internal DbSet<DealCodeSequence> DealCodeSequences { get; set; }
        internal DbSet<DealItem> DealItems { get; set; }
        internal DbSet<DealItemSourceData> DealItemSourceData { get; set; }
        internal DbSet<DealItemField> DealItemFields { get; set; }
        internal DbSet<DealItemFieldset> DealItemFieldsets { get; set; }

        internal DbSet<DealNote> DealNotes { get; set; }

        internal DbSet<DealType> DealTypes { get; set; }

        internal DbSet<DealWorkflowActionListener> DealWorkflowActionListeners { get; set; }
        internal DbSet<DealWorkflowStatus> DealWorkflowStatuses { get; set; }
        internal DbSet<DealWorkflowTask> DealWorkflowTasks { get; set; }

        internal DbSet<Functionality> Functionalities { get; set; }
        internal DbSet<FunctionalityInUserRole> FunctionalitiesInUserRoles { get; set; }
        internal DbSet<SubFunctionality> SubFunctionalities { get; set; }
        internal DbSet<SubFunctionalityInUserRole> SubFunctionalitiesInUserRoles { get; set; }

        internal DbSet<IntegrationRun> IntegrationRuns { get; set; }
        internal DbSet<IntegrationRunEntry> IntegrationRunEntries { get; set; }

        internal DbSet<TraderAuthorityPolicy> TraderAuthorityPolicies { get; set; }
        internal DbSet<TraderAuthorityPolicyCriteria> TraderAuthorityPoliciesCriteria { get; set; }

        internal DbSet<Product> Products { get; set; }
        internal DbSet<DealCategory> DealCategories { get; set; }
        internal DbSet<CounterpartyInDealCategory> CounterpartiesInDealCategories { get; set; }
        internal DbSet<DealTypeInDealCategory> DealTypesInDealCategories { get; set; }

        internal DbSet<SalesForecast> SalesForecasts { get; set; }

        internal DbSet<User> Users { get; set; }
        internal DbSet<UserRole> UserRoles { get; set; }
        internal DbSet<UserInWorkflowRole> UsersInWorkflowRoles { get; set; }
        internal DbSet<UserIntegrationData> UserIntegrationData { get; set; }


        internal DbSet<WorkflowRole> WorkflowRoles { get; set; }
        internal DbSet<WorkflowSet> WorkflowSets { get; set; }
        internal DbSet<WorkflowStatus> WorkflowStatuses { get; set; }
        internal DbSet<WorkflowAction> WorkflowActions { get; set; }
        internal DbSet<WorkflowTask> WorkflowTasks { get; set; }
        internal DbSet<WorkflowTaskAnswer> WorkflowTaskAnswers { get; set; }
        internal DbSet<WorkflowTaskInDealType> WorkflowTaskInDealTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<Deal>()
                .HasMany(d => d.DealWorkflowStatuses)
                .WithOne(wf => wf.Deal)
                .HasForeignKey(wf => wf.DealId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<DealWorkflowStatus>()
                .HasOne(s => s.DealHasThisAsCurrent)
                .WithOne(d => d.CurrentDealWorkflowStatus)
                .HasForeignKey<Deal>(d => d.CurrentDealWorkflowStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<DealWorkflowStatus>()
                .HasOne(s => s.DealHasThisAsNext)
                .WithOne(d => d.NextDealWorkflowStatus)
                .HasForeignKey<Deal>(d => d.NextDealWorkflowStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<DealWorkflowStatus>()
                .HasOne(s => s.DealHasThisAsPrevious)
                .WithOne(d => d.PreviousDealWorkflowStatus)
                .HasForeignKey<Deal>(d => d.PreviousDealWorkflowStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<WorkflowAction>()
                .HasOne(wa => wa.SourceWorkflowStatus)
                .WithMany(sw => sw.ActionsFromThisSource)
                .HasForeignKey(x => x.SourceWorkflowStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<WorkflowAction>()
                .HasOne(wa => wa.TargetWorkflowStatus)
                .WithMany(sw => sw.ActionsToThisTarget)
                .HasForeignKey(x => x.TargetWorkflowStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<WorkflowTaskInDealType>()
                .HasKey(wcdt => new { wcdt.WorkflowTaskId, wcdt.DealTypeId });

            mb.Entity<WorkflowTask>()
                .HasOne(wt => wt.PrecedingAnswer)
                .WithMany(pa => pa.SubsequentTasks)
                .HasForeignKey(wt => wt.PrecedingAnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<WorkflowTask>()
                .HasOne(wt => wt.DependingUponAnswer)
                .WithMany(pa => pa.DependentTasks)
                .HasForeignKey(wt => wt.DependingUponAnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<WorkflowTask>()
                .HasMany(wt => wt.PossibleAnswers)
                .WithOne(pa => pa.WorkflowTask)
                .HasForeignKey(pa => pa.WorkflowTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<FunctionalityInUserRole>()
            //    .HasKey(cp => new { cp.UserRoleId, cp.FunctionalityId });

            mb.Entity<UserRole>()
                .HasMany(ur => ur.FunctionalitiesInUserRole)
                .WithOne(fr => fr.UserRole)
                .HasForeignKey(fr => fr.UserRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<FunctionalityInUserRole>()
                .HasOne(fr => fr.Functionality)
                .WithMany(ur => ur.FunctionalitiesInUserRole)
                .HasForeignKey(fr => fr.FunctionalityId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<FunctionalityInUserRole>()
                .HasMany(fr => fr.SubFunctionalitiesInUserRole)
                .WithOne(s => s.FunctionalityInUserRole)
                .HasForeignKey(s => s.FunctionalityInUserRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<SubFunctionalityInUserRole>()
                .HasOne(sr => sr.SubFunctionality)
                .WithMany(sf => sf.SubFunctionalitiesInUserRole)
                .HasForeignKey(sr => sr.SubFunctionalityId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<CounterpartyInDealCategory>()
                .HasKey(cp => new { cp.DealCategoryId, cp.CounterpartyId });

            mb.Entity<DealTypeInDealCategory>()
                .HasKey(cp => new { cp.DealCategoryId, cp.DealTypeId });

            mb.Entity<UserInWorkflowRole>()
                .HasKey(cp => new { cp.UserId, cp.WorkflowRoleId });

            mb.Entity<DealNote>()
                .HasOne(dn => dn.Deal)
                .WithMany(d => d.Notes)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<Deal>()
                .HasOne(d => d.CreationUser)
                .WithMany(u => u.Deals)
                .HasForeignKey(d => d.CreationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<Deal>()
                .HasOne(d => d.ExecutionUser)
                .WithMany(u => u.DealsExecutedByUser)
                .HasForeignKey(d => d.ExecutionUserId)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<Deal>()
                .HasOne(d => d.SubmissionUser)
                .WithMany(u => u.DealsSubmittedByUser)
                .HasForeignKey(d => d.SubmissionUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // log index, to speed up search for creation/update dates on any entity
            mb.Entity<AuditEntry>()
                .HasIndex(ae => new { ae.FunctionalityId, ae.EntityId, ae.DateTime });

            // to speed up trading policy criteria verifications
            mb.Entity<Deal>()
                .HasIndex(d => new { d.SubmissionUserId, d.DealCategoryId, d.DealTypeId, d.CounterpartyId });
            mb.Entity<DealItemSourceData>()
                .HasIndex(d => d.SourceId);

            DataSeeding.Seed(mb);
        }

        public async Task<int> SaveEntityWithAudit<T>(T entity, FunctionalityEnum functionalityEnum, int? auditEntryId = null) where T : class, IEntity
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 20, 0), TransactionScopeAsyncFlowOption.Enabled))
            {
                if (entity.Id == 0)
                    Add(entity);

                int? userId = scopedDataService.GetUserId();
                if (!userId.HasValue)
                {
                    var userName = scopedDataService.GetUsername();
                    if (userName == null)
                        throw new Exception("User is not logged in.");

                    userId = (await Users.FirstAsync(u => u.Username == userName && u.Active))?.Id;
                    if (!userId.HasValue)
                        throw new Exception($"User {userName} is not registered in WorkflowSystem or it is not active.");
                }

                var auditEntry = GetOrCreateAuditEntry(userId.Value, functionalityEnum, entity.Id, auditEntryId);

                var temoraryAuditEntities = PrepareAuditData(auditEntry);

                await base.SaveChangesAsync();

                await RecordAuditData(temoraryAuditEntities, entity.Id, auditEntry);

                scope.Complete();

                return auditEntry.Id;
            }
            
        }

        private AuditEntry GetOrCreateAuditEntry(int userId, FunctionalityEnum functionalityEnum, int entityId, int? auditEntryId)
        {
            //auditEntryId = 0; // while there is no better solution, this method will always create a new audit entry
            return (auditEntryId > 0 ? AuditEntries.First(a => a.Id == auditEntryId) : new AuditEntry
            {
                DateTime = DateUtils.GetDateTimeOffsetNow(),
                UserId = userId,
                FunctionalityId = (int)functionalityEnum,
                EntityId = entityId,
                Type = entityId > 0 ? AuditEntryTypeEnum.Modified : AuditEntryTypeEnum.Added,
            });
        }

        IEnumerable<Tuple<EntityEntry, AuditEntryTable>> PrepareAuditData(AuditEntry auditEntry)
        {
            if (auditEntry.Type == AuditEntryTypeEnum.Added)
                return new List<Tuple<EntityEntry, AuditEntryTable>>();

            ChangeTracker.DetectChanges();
            var entitiesToTrack = ChangeTracker.Entries().Where(e => !(e.Entity is AuditEntry) && !(e.Entity is AuditEntryField) && e.State != EntityState.Detached && e.State != EntityState.Unchanged);

            //Return list of pairs of EntityEntry and Table  
            return entitiesToTrack
                 .Select(e => new Tuple<EntityEntry, AuditEntryTable>(
                     e,
                 new AuditEntryTable()
                 {
                     TableName = e.Metadata.GetTableName(),//.Relational().TableName,
                     Action = Enum.GetName(typeof(EntityState), e.State),
                     KeyValues = string.Join("; ", e.Properties
                        .Where(p => p.Metadata.IsPrimaryKey())
                        .Select(p => p.CurrentValue)),
                     Fields = e.Properties
                        .Where(p => e.State == EntityState.Deleted || /*e.State == EntityState.Added ||*/ (e.State == EntityState.Modified && p.IsModified))
                        .Where(p => !p.IsTemporary && (p.OriginalValue != null || p.CurrentValue != null))
                        .Select(p => new AuditEntryField
                        {
                            FieldName = p.Metadata.Name,
                            OldValue = ((e.State == EntityState.Deleted || e.State == EntityState.Modified) ? JsonConvert.SerializeObject(p.OriginalValue) : null),
                            NewValue = (e.State == EntityState.Added || e.State == EntityState.Modified ? JsonConvert.SerializeObject(p.CurrentValue) : null),
                        }).ToList()
                 })).ToList();
        }

        async Task RecordAuditData(IEnumerable<Tuple<EntityEntry, AuditEntryTable>> temporaryEntities, int entityId, AuditEntry auditEntry)
        {
            if (auditEntry.Id == 0)
            {
                auditEntry.EntityId = entityId;
                AuditEntries.Add(auditEntry);
                await SaveChangesAsync();
            }

            if (temporaryEntities != null && temporaryEntities.Any())
            {
                var tables = temporaryEntities.ForEach(t => setAdditionalFields(t, entityId)).Select(t => t.Item2).ToList();
                auditEntry.Tables.AddRange(tables);
                await SaveChangesAsync();
            }
            await Task.CompletedTask;
        }

        void setAdditionalFields(Tuple<EntityEntry, AuditEntryTable> t, int entityId)
        {
            t.Item2.KeyValues = string.Join("; ", t.Item1.Properties.Where(p => p.Metadata.IsPrimaryKey()).Select(p => p.CurrentValue));
        }
    }
}
public static class Extensions
{
    public static IDictionary<TKey, TValue> NullIfEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null || !dictionary.Any())
        {
            return null;
        }
        return dictionary;
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T element in source)
        {
            action(element);
        }
        return source;
    }
}
