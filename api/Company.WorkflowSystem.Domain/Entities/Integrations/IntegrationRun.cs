using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Domain.Util;

namespace Company.WorkflowSystem.Domain.Entities.Integrations
{
    public class IntegrationRun : BaseEntity
    {
        public IntegrationRun()
        {

        }
        public IntegrationRun(IntegrationTypeEnum type, object payload, int? userId)
        {
            Type = type;
            Started = DateUtils.GetDateTimeOffsetNow();
            Status = IntegrationRunStatusEnum.Running;
            UserId = userId;
            SetPayload(payload);
        }

        public IntegrationTypeEnum Type { get; set; }
        public DateTimeOffset Started { get; set; }
        public DateTimeOffset Ended { get; set; }
        public IntegrationRunStatusEnum Status { get; set; }
        public string Payload { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public List<IntegrationRunEntry> Entries { get; set; } = new List<IntegrationRunEntry>();
        public void SetPayload(object payload) => Payload = JsonConvert.SerializeObject(payload);

        public void AddInfoEntry(string message, object payload = null, FunctionalityEnum? functionalityOfAffectedId = null, int? affectedId = null) =>
                    AddEntry(
                        IntegrationRunEntryTypeEnum.Info,
                        message,
                        functionalityOfAffectedId,
                        affectedId,
                        payload);
        public void AddSuccessEntry(string message, object payload = null, FunctionalityEnum? functionalityOfAffectedId = null, int? affectedId = null, string details = null) =>
                    AddEntry(
                        IntegrationRunEntryTypeEnum.Success,
                        message,
                        functionalityOfAffectedId,
                        affectedId,
                        payload,
                        details);
        public void AddWarningEntry(string message, FunctionalityEnum? functionalityOfAffectedId = null, int? affectedId = null) =>
                    AddEntry(
                        IntegrationRunEntryTypeEnum.Warning,
                        message,
                        functionalityOfAffectedId,
                        affectedId);
        public void AddErrorEntry(string message, object payload = null, FunctionalityEnum? functionalityOfAffectedId = null, int? affectedId = null) =>
                    AddEntry(
                        IntegrationRunEntryTypeEnum.Error,
                        message,
                        functionalityOfAffectedId,
                        affectedId,
                        payload);

        public void AddErrorEntry(Exception exception, object payload = null, FunctionalityEnum? functionalityOfAffectedId = null, int? affectedId = null) =>
            AddEntry(
                IntegrationRunEntryTypeEnum.Error,
                "An exception has occurred during the process.",
                functionalityOfAffectedId,
                affectedId,
                payload,
                getExceptionMessage(exception)
                );

        public void AddErrorEntry(string message, Exception exception, object payload = null, FunctionalityEnum? functionalityOfAffectedId = null, int? affectedId = null) =>
           AddEntry(
               IntegrationRunEntryTypeEnum.Error,
               message,
               functionalityOfAffectedId,
               affectedId,
               payload,
               getExceptionMessage(exception)
               );

        string getExceptionMessage(Exception exception, bool first = true)
        {
            if (exception == null)
                return "";

            var message = exception.Message;
            message += "\n\n" + getExceptionMessage(exception.InnerException);
            if (first)
                message += "\n\n" + exception.StackTrace;

            return message;
        }


        public void EndRun()
        {
            Ended = DateUtils.GetDateTimeOffsetNow();
            Status = (
                Entries.Any(e => e.Type == IntegrationRunEntryTypeEnum.Error) ? IntegrationRunStatusEnum.Error :
                Entries.Any(e => e.Type == IntegrationRunEntryTypeEnum.Warning) ? IntegrationRunStatusEnum.Warning :
                IntegrationRunStatusEnum.Success
                );
        }

        void AddEntry(IntegrationRunEntryTypeEnum type, string message, FunctionalityEnum? functionalityOfAffectedId = null, int? affectedId = null, object payload = null, string details = null)
        {
            Entries.Add(new IntegrationRunEntry
            {
                Type = type,
                Message = message,
                Details = details,
                FunctionalityOfAffectedId = functionalityOfAffectedId,
                AffectedId = affectedId,
                Payload = (payload == null ? null : payload is string ? (string)payload : JsonConvert.SerializeObject(payload)),
                DateTime = DateUtils.GetDateTimeOffsetNow(),
            });
        }
        public List<object> CurrentEntities
        {
            get {
                var exceptionEntries = new List<object>();
                exceptionEntries.AddRange(Entries);
                exceptionEntries.Add(this);
                return exceptionEntries;
            }
        }   
    }
}
