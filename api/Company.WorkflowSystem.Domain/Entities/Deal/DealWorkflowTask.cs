using System;

namespace Company.WorkflowSystem.Domain.Entities
{
    public class DealWorkflowTask : BaseEntity
    {
        /// <summary>
        /// deal status that this task belongs to
        /// </summary>
        public int DealWorkflowStatusId { get; set; }
        /// <summary>
        /// deal status that this task belongs to
        /// </summary>
        public DealWorkflowStatus DealWorkflowStatus { get; set; }
        /// <summary>
        /// workflow task in the workflow master data
        /// </summary>
        public int WorkflowTaskId { get; set; }
        /// <summary>
        /// workflow task in the workflow master data
        /// </summary>
        public WorkflowTask WorkflowTask { get; set; }
        /// <summary>
        /// workflow task name, copied from the master data
        /// </summary>
        public string WorkflowTaskDescription { get; set; }
        /// <summary>
        /// The answer that was given to this task (if the task was a question)
        /// </summary>
        public int? WorkflowTaskAnswerId { get; set; }
        /// <summary>
        /// The answer that was given to this task (if WorkflowTaskTypeEnum.AnswerToQuestion)
        /// </summary>
        public WorkflowTaskAnswer WorkflowTaskAnswer { get; set; }
        /// <summary>
        /// The text answer (copied from master data) that was given to this task (if WorkflowTaskTypeEnum.AnswerToQuestion)
        /// </summary>
        public string WorkflowTaskAnswerText { get; set; }
        /// <summary>
        /// String format information (if WorkflowTaskTypeEnum.EnterTextInformation)
        /// </summary>
        public string TextInformation { get; set; }
        /// <summary>
        /// date format information (if WorkflowTaskTypeEnum.EnterDateInformation or WorkflowTaskTypeEnum.EnterDateTimeInformation)
        /// </summary>
        public DateTimeOffset? DateInformation { get; set; }
        /// <summary>
        /// number/decimal format information (if WorkflowTaskTypeEnum.EnterNumberInformation)
        /// </summary>
        public decimal? NumberInformation { get; set; }
        /// <summary>
        /// whether this task was completed
        /// </summary>
        public bool Done { get; set; }
    }
}