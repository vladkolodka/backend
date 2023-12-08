﻿using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Core.ValueObjects;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Companies.Dto
{
    public record CompanyDto
    {
        public string Name { get; set; }
        public LocalizedString Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public AddressDto Address { get; set; }
        public IList<string> Certificates { get; set; }
    }
}