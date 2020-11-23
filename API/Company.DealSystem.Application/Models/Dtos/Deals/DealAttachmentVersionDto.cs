using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Company.DealSystem.Application.Extensions;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Dtos.Deals
{
    public class DealAttachmentVersionDto : UpdatableListItemDto, IPersistableDto<DealAttachmentVersion, DealService>
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileBase64 { get; set; }
        public long FileSizeInBytes { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public int CreationUserId { get; set; }
        public string CreationUserName { get; set; }
        public bool IsLocked { get; set; }

        internal static Expression<Func<DealAttachmentVersion, DealAttachmentVersionDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new DealAttachmentVersionDto()
                {
                    Id = entity.Id,
                    FileName = entity.FileName,
                    FileExtension = entity.FileExtension,
                    FileSizeInBytes = entity.FileSizeInBytes,
                    CreatedDate = entity.CreatedDate,
                    CreationUserId = entity.CreationUserId,
                    CreationUserName = entity.CreationUser.Name,
                    IsLocked = entity.IsLocked,
                };
            }
        }

        /// <summary>
        /// creates or updates entity based on current dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DealAttachmentVersion ToEntity(DealAttachmentVersion entity, DealService service)
        {
            if (entity == null)
            {
                var withoutHeader = Regex.Replace(FileBase64, "^data:(.+);base64,", string.Empty);
                var file = withoutHeader.Base64StringToBinary();

                var fileSize = file.Length;

                entity = new DealAttachmentVersion
                {
                    FileName = FileName,
                    FileExtension = FileExtension,
                    FileSizeInBytes = fileSize,
                    CreatedDate = CreatedDate,
                    CreationUserId = CreationUserId,
                    File = file,
                    IsLocked = false,
                };
            }

            return entity;
        }
    }
}