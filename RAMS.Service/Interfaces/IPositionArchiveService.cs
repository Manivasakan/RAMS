﻿using RAMS.Enums;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// IPositionArchiveService interface declares the PositionArchive specific service operations
    /// </summary>
    public interface IPositionArchiveService
    {
        void CreateArchivePosition(PositionArchive positionArchive);

        void SaveChanges();
    }
}
