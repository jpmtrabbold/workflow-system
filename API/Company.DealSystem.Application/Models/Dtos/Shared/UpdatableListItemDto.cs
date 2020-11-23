﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Company.DealSystem.Application.Models.Dtos.Shared
{
    public class UpdatableListItemDto
    {
        public bool Deleted { get; set; } = false;
        public bool Updated { get; set; } = false;
        public int? Id { get; set; }
    }
}
