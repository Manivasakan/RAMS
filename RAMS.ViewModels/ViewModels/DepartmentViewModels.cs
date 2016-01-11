﻿using RAMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.ViewModels
{
    /// <summary>
    /// DepartmentListViewModel view model declares properties for _DepartmentList partial view
    /// </summary>
    public class DepartmentListViewModel
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public int NumOfAgents { get; set; }

        public virtual List<Agent> Agents { get; set; }

        public DepartmentListViewModel()
        {
            this.Agents = new List<Agent>();
        }
    }

    /// <summary>
    /// DepartmentAddViewModel view model declares properties for _NewDepartment partial view
    /// </summary>
    public class DepartmentAddViewModel
    {
        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Department Name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// DepartmentEditViewModel view model declares properties for _EditDepartment partial view
    /// </summary>
    public class DepartmentEditViewModel : DepartmentAddViewModel
    {
        public int DepartmentId { get; set; }

        public byte[] Timestamp { get; set; }

        public virtual List<Agent> Agents { get; set; }

        public DepartmentEditViewModel()
        {
            this.Agents = new List<Agent>();
        }
    }
}
