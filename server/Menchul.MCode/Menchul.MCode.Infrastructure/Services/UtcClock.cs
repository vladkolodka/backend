using Menchul.MCode.Application.Services.Interfaces;
using System;

namespace Menchul.MCode.Infrastructure.Services
{
    internal class UtcClock : IClock
    {
        public DateTime Current() => DateTime.UtcNow;
    }
}